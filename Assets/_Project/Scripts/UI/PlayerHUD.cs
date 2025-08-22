using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
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

    [Header("무기 패널")]
    public GameObject WeaponPanel;
    public GameObject AmmoPanel;

    [Header("시간 아이콘")]
    [SerializeField] private Image rewindIcon;
    [SerializeField] private Image stopIcon;
    [SerializeField] private Image slowIcon;
    [SerializeField] private Image fastIcon;
    [SerializeField] private float timeBlinkSpeed = 3f;

    private bool isBlinking;
    private Coroutine lowHpBlinkRoutine;
    private Coroutine blinkRoutine;

    private void Awake()
    {
        if (hpFillImage == null && hpBar != null && hpBar.fillRect != null)
            hpFillImage = hpBar.fillRect.GetComponent<Image>();
        
        if(mpFillImage == null && mpBar != null && mpBar.fillRect != null)
            mpFillImage = mpBar.fillRect.GetComponent<Image>();

        if(staminaFillImage == null && staminaBar != null && staminaBar.fillRect != null)
            staminaFillImage = staminaBar.fillRect.GetComponent<Image>();
    }

    #region 플레이어 상태 관련
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

    private IEnumerator LowHpBlink()
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
    #endregion

    #region 시간 관련
    public void ShowTimeState(TimeState state)
    {
        ResetAll();

        Image target = IconOf(state);
        if (target != null)
        {
            if (blinkRoutine != null) StopCoroutine(blinkRoutine);
            blinkRoutine = StartCoroutine(Blink(target));
        }
    }

    public void ClearTimeState()
    {
        if (blinkRoutine != null)
        { 
            StopCoroutine(blinkRoutine); 
            blinkRoutine = null; 
        }

        ResetAll();
    }

    private IEnumerator Blink(Image img)
    {
        while (true)
        {
            float a = 0.35f + 0.65f * (0.5f * (Mathf.Sin(Time.unscaledTime * timeBlinkSpeed) + 1f));
            var c = img.color; c.a = a; img.color = c;
            yield return null;
        }
    }

    private void ResetAll()
    {
        SetAlpha(rewindIcon, 0f);
        SetAlpha(stopIcon, 0f);
        SetAlpha(slowIcon, 0f);
        SetAlpha(fastIcon, 0f);
    }

    private void SetAlpha(Image img, float a)
    {
        if (!img) return;
        var c = img.color;
        c.a = a;
        img.color = c;
    }

    private Image IconOf(TimeState s)
    {
        switch (s)
        {
            case TimeState.Rewind: return rewindIcon;
            case TimeState.Stop: return stopIcon;
            case TimeState.Slow: return slowIcon;
            case TimeState.FastForward: return fastIcon;
            default: return null;
        }
    }
    #endregion

    #region 무기 관련
    public void SetWeaponImage(Sprite weapon)
    {
        Image weaponIcon = WeaponPanel.GetComponent<Image>();
        weaponIcon.sprite = weapon;
    }
    #endregion
}
