using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StatusIconUI : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private Image iconImage;       // 상태이상 아이콘 이미지
    private CanvasGroup canvasGroup;

    [System.Serializable]
    public class StatusIconData
    {
        public StatusEffectType type;
        public Sprite icon;
    }

    [Header("아이콘 스프라이트 데이터")]
    [SerializeField] private List<StatusIconData> iconDatabase;

    private Coroutine blinkRoutine;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        gameObject.SetActive(false);
    }

    // 상태이상 아이콘 표시
    public void Show(StatusEffectType type, float duration)
    {
        // 아이콘 설정
        iconImage.sprite = GetSprite(type);
        iconImage.enabled = true;
        canvasGroup.alpha = 1f;
        gameObject.SetActive(true);

        // 기존 깜빡임 중단
        if (blinkRoutine != null)
            StopCoroutine(blinkRoutine);

        // 지속시간이 있을 경우 깜빡임 시작
        if (duration > 0f)
            blinkRoutine = StartCoroutine(BlinkThenHide(duration));
    }

    public void Hide()
    {
        if (blinkRoutine != null)
        {
            StopCoroutine(blinkRoutine);
            blinkRoutine = null;
        }

        canvasGroup.alpha = 1f;
        iconImage.enabled = false;
        gameObject.SetActive(false);
    }

    // 아이콘 점멸 후 숨김
    private IEnumerator BlinkThenHide(float totalDuration, float blinkTime = 1f)
    {
        float t = 0f;
        float blinkStart = totalDuration - blinkTime;

        while (t < totalDuration)
        {
            t += Time.deltaTime;

            if (t > blinkStart)
            {
                float alpha = Mathf.PingPong(Time.time * 6f, 1f); // 빠르게 점멸
                canvasGroup.alpha = alpha;
            }

            yield return null;
        }

        Hide();
    }

    private Sprite GetSprite(StatusEffectType type)
    {
        var match = iconDatabase.FirstOrDefault(x => x.type == type);
        return match?.icon;
    }
}
