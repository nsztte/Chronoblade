using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerHUD : MonoBehaviour
{
    [Header("슬라이더")]
    [SerializeField] private Slider hpBar;
    [SerializeField] private Slider mpBar;
    [SerializeField] private Slider staminaBar;

    [Header("텍스트")]
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI goldText;

    [Header("이미지")]
    [SerializeField] private Image hpFillImage;
    [SerializeField] private Image mpFillImage;
    [SerializeField] private Image staminaFillImage;

    [Header("체력 경고 관련")]
    [Range(0f, 1f)] [SerializeField] private float lowHpThreshold = 0.3f;
    [SerializeField] private float blinkSpeed = 6f;
    [SerializeField] private Color hpNormalColor = Color.red;
    [SerializeField] private Color hpLowColor = new Color(1f, 0.35f, 0.35f);

    private bool isBlinking;
    private Coroutine lowHpBlinkRoutine;

    private void Awake()
    {
        if (hpFillImage == null && hpBar != null && hpBar.fillRect != null)
            hpFillImage = hpBar.fillRect.GetComponent<Image>();
        
        if(mpFillImage == null && mpBar != null && mpBar.fillRect != null)
            mpFillImage = mpBar.fillRect.GetComponent<Image>();

        if(staminaFillImage == null && staminaBar != null && staminaBar.fillRect != null)
            staminaFillImage = staminaBar.fillRect.GetComponent<Image>();
    }

    public void UpdateHP(int current, int max)
    {
        float t = Mathf.Clamp01(max > 0 ? (float)current / max : 0f);
        if (hpBar != null) hpBar.SetValueWithoutNotify(t);

        // 저체력 경고 처리
        if (t <= lowHpThreshold)
        {
            if (!isBlinking)
            {
                isBlinking = true;
                if (lowHpBlinkRoutine != null) StopCoroutine(lowHpBlinkRoutine);
                lowHpBlinkRoutine = StartCoroutine(LowHpBlink());
            }
        }
        else
        {
            if (isBlinking)
            { 
                isBlinking = false;

                if(lowHpBlinkRoutine != null)
                {
                    StopCoroutine(lowHpBlinkRoutine);
                    lowHpBlinkRoutine = null;
                }
            }

            if (hpFillImage != null)
            {
                hpFillImage.color = hpNormalColor;

                var cg = hpFillImage.GetComponentInParent<CanvasGroup>();
                if (cg != null) cg.alpha = 1f;
            }
        }
    }

    public void UpdateMP(int current, int max)
    {
        float t = Mathf.Clamp01(max > 0 ? (float)current / max : 0f);
        if (mpBar != null) mpBar.SetValueWithoutNotify(t);
    }

    public void UpdateStamina(int current, int max)
    {
        float t = Mathf.Clamp01(max > 0 ? (float)current / max : 0f);
        if (staminaBar != null) staminaBar.SetValueWithoutNotify(t);
    }

    public void UpdateAmmo(int current, int total)
    {
        if(ammoText != null)
        {
            ammoText.text = current >= 0 ? $"{current}/{total}" : $"{total}";
        }
    }

    public void UpdateGold(int amount)
    {
        if(goldText != null)
        {
            goldText.text = $"{amount.ToString()} G";
        }
    }

    System.Collections.IEnumerator LowHpBlink()
    {
        CanvasGroup cg = null;

        if (hpFillImage != null)
        {
            // 색상 경고
            hpFillImage.color = hpLowColor;
            cg = hpFillImage.GetComponentInParent<CanvasGroup>();
            
            if (cg == null)
                cg = hpFillImage.gameObject.AddComponent<CanvasGroup>();
        }

        float t = 0f;
        while (true)
        {
            t += Time.unscaledDeltaTime * blinkSpeed;
            if (cg != null)
                cg.alpha = 0.5f + 0.5f * Mathf.Abs(Mathf.Sin(t)); // 최소 수치 0.5, 최대 수치 1.0
            
            yield return null;
        }
    }
}
