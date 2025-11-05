using System.Collections;
using UnityEngine;

public class CCTVPlayerDetector : MonoBehaviour, ITimeControllable, IRewindable
{
    [Header("시야 설정")]
    [SerializeField] private float viewDistance = 10f;
    [SerializeField] private float fieldOfViewAngle = 45f;
    [SerializeField] private Transform cameraPoint;
    [SerializeField] private LayerMask detectionLayer;

    [Header("감지 설정")]
    [SerializeField] private float detectionThreshold = 3f;
    [SerializeField] private Transform cctvBody; // CCTV가 회전하는 본체
    [SerializeField] private DoorController door; // 문 애니메이션 포함 오브젝트
    [SerializeField] private EnemySpawnPoint spawnPoint; // 와쳐 소환용
    [SerializeField] private int spawnCount = 5;
    [SerializeField] private float detectedTimer = 0f;

    // 리와인드 설정
    private bool isRewinding = false;

    private float timeScale = 1f;
    private Transform player;
    private Animator animator;

    private void Start()
    {
        player = PlayerManager.Instance.PlayerTransform;
        animator = GetComponent<Animator>();

        TimeManager.Instance.RegisterControllable(this);
        TimeManager.Instance.RegisterRewindable(this);

        float delay = Random.Range(0,5);
        Invoke("StartAnimation", delay);
    }

    private void OnDisable()
    {
        TimeManager.Instance.UnregisterControllable(this);
        TimeManager.Instance.UnregisterRewindable(this);

    }

    private void Update()
    {
        bool inSight = player != null && IsPlayerInSight();

        if(isRewinding)
        {
            animator.SetFloat("Speed", -1);
            return;
        }

        if(inSight)
        {
            Debug.Log("플레이어 감지");
            detectedTimer += Time.deltaTime;
            animator.speed = 0f;
            RotateToPlayer();

            if(detectedTimer >= detectionThreshold)
            {
                TriggerAlarm();
            }
        }
        else
        {
            detectedTimer = 0f;
            animator.SetFloat("Speed", 1);
            animator.speed = timeScale;
        }
    }

    private void StartAnimation()
    {
        animator.SetTrigger("Start");
    }

    private bool IsPlayerInSight()
    {
        Vector3 toPlayer = player.position - cameraPoint.position;
        float distance = toPlayer.magnitude;

        if (distance > viewDistance)
            return false;

        float angle = Vector3.Angle(cameraPoint.forward, toPlayer.normalized);
        if (angle > fieldOfViewAngle * 0.5f)
            return false;

        // 시야 내 Raycast
        if (Physics.Raycast(cameraPoint.position, toPlayer.normalized, out RaycastHit hit, viewDistance, detectionLayer))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    private void RotateToPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        cctvBody.rotation = Quaternion.Slerp(cctvBody.rotation, lookRotation, Time.deltaTime * 20f);
    }


    private void TriggerAlarm()
    {
        // TODO:경고등 이펙트, 사운드 등 추가
        if(door != null) door.OpenDoor();
        
        if (spawnPoint != null)
        {
            if (spawnPoint.ActiveEnemies.Count == 0)
                spawnPoint.TrySpawnEnemies(spawnCount);

            foreach (var e in spawnPoint.ActiveEnemies)
            {
                e.DetectPlayer();
            }
        }
    }

    public void SetTimeScale(float timeScale)
    {
        this.timeScale = timeScale;
        if(animator != null) animator.speed = timeScale;
    }

    public float GetTimeScale()
    {
        return timeScale;
    }

    private void OnDrawGizmosSelected()
    {
        if (cameraPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(cameraPoint.position, cameraPoint.forward * viewDistance);

        Vector3 left = Quaternion.Euler(0, -fieldOfViewAngle * 0.5f, 0) * cameraPoint.forward;
        Vector3 right = Quaternion.Euler(0, fieldOfViewAngle * 0.5f, 0) * cameraPoint.forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(cameraPoint.position, left * viewDistance);
        Gizmos.DrawRay(cameraPoint.position, right * viewDistance);
    }

    public void StartRewind()
    {
        isRewinding = true;
    }

    public void StopRewind()
    {
        isRewinding = false;
    }
}
