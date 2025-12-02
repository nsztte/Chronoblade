using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialUI : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] TextMeshProUGUI messageText;

    [Header("타이밍")]
    [SerializeField] float fadeDuration = 0.2f;
    [SerializeField] float defaultHoldTime = 2.5f;

    private class TutorialRequest
    {
        public string id;
        public string text;
        public float holdTime;
        public bool showOnce;
    }

    private readonly Queue<TutorialRequest> queue = new Queue<TutorialRequest>();
    private readonly HashSet<string> shownIds = new HashSet<string>();

    private Coroutine currentRoutine;

    private void Awake()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    /// <summary>
    /// id: 튜토리얼 고유 키(중복 방지용), text: 표시할 문자열
    /// holdTime: 유지 시간(음수면 defaultHoldTime 사용)
    /// showOnce: true면 한 번 표시 이후 다시 호출해도 무시
    /// </summary>
    public void ShowTutorial(string id, string text, float holdTime = -1f, bool showOnce = true)
    {
        if (showOnce && shownIds.Contains(id))
            return;

        var request = new TutorialRequest
        {
            id = id,
            text = text,
            holdTime = holdTime > 0f ? holdTime : defaultHoldTime,
            showOnce = showOnce
        };

        queue.Enqueue(request);

        if (currentRoutine == null)
        {
            currentRoutine = StartCoroutine(ProcessQueue());
        }
    }

    private IEnumerator ProcessQueue()
    {
        while (queue.Count > 0)
        {
            var req = queue.Dequeue();

            if (req.showOnce)
                shownIds.Add(req.id);

            messageText.text = req.text;

            // Fade in
            yield return Fade(0f, 1f);

            // Hold
            float t = 0f;
            while (t < req.holdTime)
            {
                t += Time.unscaledDeltaTime;
                yield return null;
            }

            // Fade out
            yield return Fade(1f, 0f);
        }

        currentRoutine = null;
    }

    private IEnumerator Fade(float from, float to)
    {
        float t = 0f;

        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = false;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float ratio = Mathf.Clamp01(t / fadeDuration);
            canvasGroup.alpha = Mathf.Lerp(from, to, ratio);
            yield return null;
        }

        canvasGroup.alpha = to;

        if (Mathf.Approximately(to, 0f))
        {
            canvasGroup.blocksRaycasts = false;
        }
    }
}
