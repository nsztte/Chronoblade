using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrystalFollowPlayer : MonoBehaviour, ITimeControllable, IRewindable
{
    private NavMeshAgent agent;
    private ParticleSystem[] crystalParticles;
    private Transform target;

    // 속도
    private float baseSpeed = 3.5f;
    private float baseAngularSpeed  = 120f;

    [Header("되감기 설정")]
    [SerializeField] private float recordInterval = 0.1f;
    [SerializeField] private int maxStoredPosition = 100;
    private List<Vector3> positionHistory = new();
    private Vector3 targetRewindPosition;
    private float recordTimer;

    private bool isRewinding = false;
    private int rewindIndex = 0;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        crystalParticles = GetComponentsInChildren<ParticleSystem>();

        target = PlayerManager.Instance.PlayerTransform;

        baseSpeed = agent.speed;
        baseAngularSpeed = agent.angularSpeed;

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
