using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ShopOrbitMover : MonoBehaviour
{
    [Header("기본 설정")]
    [SerializeField] private Transform center;
    [SerializeField] private float radius = 5f;
    [SerializeField] private float height = 2f;
    [SerializeField] private float degPerSec = 6f;
    [SerializeField] private float accel = 2f;
    [SerializeField] private float stopRange = 3f;
    [SerializeField] private float rotationSpeed = 2f;

    [Header("수직 보블")]
    [SerializeField] private float bobAmplitude = 0.5f;   // 위아래 진폭
    [SerializeField] private float bobFrequency = 1.0f;   // 초당 왕복 주기(Hz)
    [SerializeField, Tooltip("정지 시에도 남길 최소 보블 비율 (0=완전 정지)")]
    private float bobMinScaleOnStop = 0.25f;

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
        bool wantStop = dist <= stopRange;

        // 이동/정지 상태 업데이트
        float targetScale = wantStop ? 0f : 1f;
        animator.SetBool("IsInteract", wantStop);

        // 속도 보간
        speedScale = Mathf.MoveTowards(speedScale, targetScale, accel * Time.deltaTime);

        // 각도 업데이트
        angle += degPerSec * speedScale * Time.deltaTime;
        
        // 위치 이동 및 수직 진폭폭
        Vector3 pos = PositionOnOrbit(angle);
        pos.y += GetBobOffset(wantStop);
        transform.position = pos;

        // 회전 처리
        if (wantStop && player)
        {
            // 플레이어 바라보기 (수평 회전)
            Vector3 lookDir = player.position - transform.position;
            lookDir.y = 0f;
            if (lookDir.sqrMagnitude > 0.01f)
            {
                Quaternion targetRot = Quaternion.LookRotation(lookDir.normalized, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            // 공전 접선 방향 회전
            Vector3 tangent = new Vector3(-Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
            Quaternion targetRot = Quaternion.LookRotation(tangent, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }
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

    private float GetBobOffset(bool wantStop)
    {
        // 보블 스케일: 정지 때는 최소(bobMinScaleOnStop)~이동 때 1 사이로 보간
        float bobScale = Mathf.Lerp(bobMinScaleOnStop, 1f, speedScale);
        return Mathf.Sin(Time.time * (Mathf.PI * 2f) * bobFrequency) * bobAmplitude * bobScale;
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
