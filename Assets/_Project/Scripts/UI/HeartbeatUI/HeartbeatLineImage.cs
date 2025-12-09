using UnityEngine;

public class HeartbeatLineImage : MonoBehaviour
{
    public RectTransform RectTransform => (RectTransform)transform;

    private CanvasGroup canvasGroup;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Flash(float peakAlpha = 1f, float fadeTo = 0.3f, float duration = 0.2f)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }
        fadeCoroutine = StartCoroutine(FadeOut(peakAlpha, fadeTo, duration));
    }

    private System.Collections.IEnumerator FadeOut(float from, float to, float time)
    {
        canvasGroup.alpha = from;
        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, t / time);
            yield return null;
        }
        canvasGroup.alpha = to;
        fadeCoroutine = null;
    }
}
