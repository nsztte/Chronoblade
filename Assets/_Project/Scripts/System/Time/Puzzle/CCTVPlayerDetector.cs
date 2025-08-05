using UnityEngine;

public class CCTVPlayerDetector : MonoBehaviour, ITimeControllable, IRewindable
{
    [Header("시야 설정")]
    [SerializeField] private float viewDistance = 10f;
    [SerializeField] private float fieldOfViewAngle = 45f;
    [SerializeField] private Transform cameraPoint;
    [SerializeField] private LayerMask detectionLayer;

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
    }

    private void OnDisable()
    {
        TimeManager.Instance.UnregisterControllable(this);
        TimeManager.Instance.UnregisterRewindable(this);

    }

    private void Update()
    {
        if(player != null && IsPlayerInSight())
        {
            Debug.Log("플레이어 감지");
        }
        
        if(isRewinding)
            animator.SetFloat("Speed", -1);
        else
            animator.SetFloat("Speed", 1);
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
