using UnityEngine;
using UnityEngine.UI;

public class EnemyHPUI : MonoBehaviour
{
    [Header("UI 참조")]
    [SerializeField] private Slider hpSlider;

    [Header("위치")]
    [SerializeField] private Vector3 offset;

    [Header("거리 알파 설정")]
    [SerializeField] private float fadeStartDistance = 12f;
    [SerializeField] private float fadeEndDistance = 20f;

    [SerializeField] private Transform followTarget;
    private CanvasGroup canvasGroup;
    private Camera mainCam;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        mainCam = Camera.main;
    }

    private void LateUpdate()
    {
        if (mainCam == null)
            mainCam = Camera.main;

        if (followTarget == null || mainCam == null) return;

        // 타겟 따라 위치 이동
        transform.position = followTarget.position + offset;

        // 카메라 정면 바라보도록 회전
        Vector3 camDir = (transform.position - mainCam.transform.position).normalized;
        transform.forward = camDir;

        // 거리 기반 알파 처리
        float distance = Vector3.Distance(mainCam.transform.position, transform.position);
        float alpha = Mathf.Clamp01((fadeEndDistance - distance) / (fadeEndDistance - fadeStartDistance));
        canvasGroup.alpha = alpha;
    }

    public void SetHP(int current, int max)
    {
        if (hpSlider != null && max > 0)
        {
            float ratio = Mathf.Clamp01((float)current / max);
            hpSlider.value = ratio;
            gameObject.SetActive(ratio > 0f); // 0이면 자동 비활성화
        }
    }

    public void SetFollowTarget(Transform target)
    {
        followTarget = target;
    }

    public void SetOffset(Vector3 newOffset)
    {
        offset = newOffset;
    }
}
