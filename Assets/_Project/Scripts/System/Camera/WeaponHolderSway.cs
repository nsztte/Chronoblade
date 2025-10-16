using UnityEngine;

public class WeaponHolderSway : MonoBehaviour
{
    [Header("기본 설정")]
    [SerializeField] private float walkAmplitude = 0.02f; // 걷기 흔들림 크기
    [SerializeField] private float runAmplitude = 0.04f;  // 뛰기 흔들림 크기
    [SerializeField] private float frequency = 8f;        // 흔들림 속도
    [SerializeField] private CharacterController cc;
    private Vector3 basePos;
    private float phase;

    private void Start()
    {
        basePos = transform.localPosition;
    }

    private void LateUpdate()
    {
        if (!cc) return;

        // 플레이어의 수평 이동 속도 계산
        Vector3 hv = new Vector3(cc.velocity.x, 0f, cc.velocity.z);
        float speed = hv.magnitude;

        // 달리기일수록 진폭 보간
        bool isRunning = PlayerManager.Instance != null && PlayerManager.Instance.PlayerController.IsRunning;
        float amp = isRunning ? runAmplitude : walkAmplitude;

        // 속도 없으면 제자리 복귀
        if (speed < 0.1f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, basePos, Time.deltaTime * 5f);
            return;
        }

        // 좌우 흔들림 (sin 파형)
        phase += frequency * Time.deltaTime * (isRunning ? 1.5f : 1f);
        float offsetX = Mathf.Sin(phase) * amp;

        Vector3 target = basePos + new Vector3(offsetX, 0f, 0f);
        transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * 10f);
    }
}
