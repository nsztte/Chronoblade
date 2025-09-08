using System.Collections;
using UnityEngine;

public class FadeUI : MonoBehaviour
{
    [SerializeField] private float defaultDuration = 0.3f;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    public Coroutine Show(float duration = -1f) =>
        StartCoroutine(FadeTo(1f, duration < 0 ? defaultDuration : duration));

    public Coroutine Hide(float duration = -1f) =>
        StartCoroutine(FadeTo(0f, duration < 0 ? defaultDuration : duration));

    private IEnumerator FadeTo(float targetAlpha, float duration)
    {
        float start = canvasGroup.alpha;
        float t = 0f;

        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / duration;
            canvasGroup.alpha = Mathf.Lerp(start, targetAlpha, t);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;

        // 완전히 숨겨졌을 때만 입력 차단 해제
        if (Mathf.Approximately(targetAlpha, 0f))
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
    }
}
