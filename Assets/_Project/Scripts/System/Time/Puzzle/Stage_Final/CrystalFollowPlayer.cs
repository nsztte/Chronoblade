using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CrystalFollowPlayer : MonoBehaviour, ITimeControllable, IRewindable
{
    [Header("기본 설정")]
    [SerializeField] private int damage = 30;
    private float baseSpeed = 3.5f;
    private float baseAngularSpeed  = 120f;

    [Header("넉백 설정")]
    [SerializeField] private float knockbackSpeed = 5f;
    [SerializeField] private float knockbackDistance = 5f;

    [Header("되감기 설정")]
    [SerializeField] private float recordInterval = 0.1f;
    [SerializeField] private int maxStoredPosition = 100;
    private List<Vector3> positionHistory = new();
    private Vector3 targetRewindPosition;
    private float recordTimer;

    private bool isRewinding = false;
    private int rewindIndex = 0;

    private NavMeshAgent agent;
    private ParticleSystem[] crystalParticles;
    private Transform target;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        crystalParticles = GetComponentsInChildren<ParticleSystem>();

        target = PlayerManager.Instance.PlayerTransform;

        baseSpeed = agent.speed;
        baseAngularSpeed = agent.angularSpeed;
    }

    private void OnEnable()
    {
        TimeManager.Instance.RegisterControllable(this);
        TimeManager.Instance.RegisterRewindable(this);
    }

    private void OnDisable()
    {
        TimeManager.Instance.UnregisterControllable(this);
        TimeManager.Instance.UnregisterRewindable(this);
    }

    private void Update()
    {
        if(!isRewinding)
        {
            recordTimer += Time.deltaTime;
            if(recordTimer >= recordInterval)
            {
                positionHistory.Add(transform.position);
                recordTimer = 0f;

                if(positionHistory.Count > maxStoredPosition)
                {
                    positionHistory.RemoveAt(0);
                }
            }

            if(target != null)
            {
                agent.SetDestination(target.position);
            }
        }
        else
        {
            if(rewindIndex >= 0 && rewindIndex < positionHistory.Count)
            {
                targetRewindPosition = positionHistory[rewindIndex];

                transform.position = Vector3.MoveTowards(transform.position, targetRewindPosition, baseSpeed * Time.deltaTime);
                
                if (Vector3.Distance(transform.position, targetRewindPosition) < 0.05f)
                {
                    rewindIndex--;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player")) return;

        if(other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage);

            ApplyKnockback(other.transform);
        }
    }

    private void ApplyKnockback(Transform target)
    {
        Vector3 dir = (transform.position - target.position).normalized;
        dir.y = 0f;
        StartCoroutine(KnockbackRoutine(dir));
    }

    private IEnumerator KnockbackRoutine(Vector3 dir)
    {
        agent.isStopped = true;

        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + dir * knockbackDistance;

        while (Vector3.Distance(transform.position, targetPos) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, knockbackSpeed * Time.deltaTime);
            yield return null;
        }

        agent.isStopped = false;
    }

    public float GetTimeScale()
    {
        return Time.timeScale;
    }

    public void SetTimeScale(float timeScale)
    {
        agent.speed = baseSpeed * timeScale;
        agent.angularSpeed = baseAngularSpeed * timeScale;

        if (crystalParticles.Length > 0)
        {
            foreach(var ps in crystalParticles)
            {
                var main = ps.main;
                main.simulationSpeed = timeScale;
            }
        }
    }

    public void StartRewind()
    {
        isRewinding = true;
        rewindIndex = positionHistory.Count - 1;

        agent.isStopped = true;
        agent.enabled = false;
    }

    public void StopRewind()
    {
        isRewinding = false;

        agent.enabled = true;
        agent.isStopped = false;
    }
}
