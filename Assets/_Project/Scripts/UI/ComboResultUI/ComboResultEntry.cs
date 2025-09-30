using UnityEngine;
using TMPro;
using DG.Tweening;

public class ComboResultEntry : MonoBehaviour
{
    private TextMeshProUGUI resultText;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    private void Awake()
    {
        resultText = GetComponent<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void Play(string text, Color color)
    {
        resultText.text = text;
        resultText.color = color;

        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.localScale = Vector3.one * 0.6f;
        canvasGroup.alpha = 0;

        Sequence seq = DOTween.Sequence();
        seq.Append(canvasGroup.DOFade(1f, 0.1f))
           .Join(rectTransform.DOScale(1f, 0.15f).SetEase(Ease.OutBack))
           .AppendInterval(0.5f)
           .Append(canvasGroup.DOFade(0f, 0.4f))
           .Join(rectTransform.DOAnchorPosY(60f, 0.4f).SetRelative(true))
           .OnComplete(() => gameObject.SetActive(false));
    }

    public void CleanupBeforeReturn()
    {
        DOTween.Kill(this);
        canvasGroup.alpha = 0;
        rectTransform.anchoredPosition = Vector2.zero;
    }
}
