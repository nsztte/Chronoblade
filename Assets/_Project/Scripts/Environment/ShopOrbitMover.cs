using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ShopOrbitMover : MonoBehaviour
{
    [SerializeField] private Transform center;
    [SerializeField] private float radius = 5f;
    [SerializeField] private float height = 2f;
    [SerializeField] private float degPerSec = 6f;
    [SerializeField] private float accel = 2f;
    [SerializeField] private float stopRange = 3f;

    private Animator animator;
    private Transform player;

    private float angle; // 런타임 진행 각도(도)
    private float speedScale; // 0~1

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = PlayerManager.Instance.PlayerTransform;
    }

    private void Update()
    {
        if (!center) return;

        float dist = player ? Vector3.Distance(player.position, PositionOnOrbit(angle)) : 999f;
        float targetScale;
        if (dist <= stopRange)
        {
            targetScale = 0f;
            animator.SetBool("IsInteract", true);
        }
        else
        {
            targetScale = 1f;
            
            if (animator.GetBool("IsInteract"))
                animator.SetBool("IsInteract", false);
        }
        
        // float targetScale = (dist <= stopRange) ? 0f : 1f;
        speedScale = Mathf.MoveTowards(speedScale, targetScale, accel * Time.deltaTime);

        angle += degPerSec * speedScale * Time.deltaTime;
        transform.position = PositionOnOrbit(angle);

        // 접선 방향을 바라보게
        Vector3 tangent = new Vector3(-Mathf.Sin(angle*Mathf.Deg2Rad), 0, Mathf.Cos(angle*Mathf.Deg2Rad));
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(tangent, Vector3.up), 6f * Time.deltaTime);
    }

    private Vector3 PositionOnOrbit(float ang)
    {
        var c = center ? center.position : Vector3.zero;
        return new Vector3(
            c.x + Mathf.Cos(ang * Mathf.Deg2Rad) * radius,
            c.y + height,
            c.z + Mathf.Sin(ang * Mathf.Deg2Rad) * radius
        );
    }

    #if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (!center) return;

        // 중심점 표시
        Vector3 centerPos = center.position + Vector3.up * height;
        Gizmos.color = new Color(1f, 0.85f, 0.2f, 0.9f);
        Gizmos.DrawSphere(centerPos, 0.12f);

        // 궤도 원(오렌지), 정지 범위(블루)
        Handles.color = new Color(1f, 0.55f, 0.1f, 0.9f);
        Handles.DrawWireDisc(centerPos, Vector3.up, radius);

        Handles.color = new Color(0.2f, 0.6f, 1f, 0.9f);
        Handles.DrawWireDisc(centerPos, Vector3.up, stopRange);

        // 현재 위치/진행 방향 화살표
        Vector3 pos = Application.isPlaying ? transform.position : PositionOnOrbit(angle);
        Vector3 tangent = new Vector3(-Mathf.Sin(angle*Mathf.Deg2Rad), 0, Mathf.Cos(angle*Mathf.Deg2Rad));
        Handles.color = new Color(1f, 1f, 1f, 0.9f);
        Handles.ArrowHandleCap(0, pos, Quaternion.LookRotation(tangent, Vector3.up), 0.8f, EventType.Repaint);

        // 라벨
        Handles.Label(centerPos + Vector3.up * 0.25f, $"Orbit R={radius:F1}  Stop={stopRange:F1}");
    }
    #endif
}
