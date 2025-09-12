using UnityEngine;
using UnityEngine.UI;
using System.Collections;
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

    [Header("무기 관련")]
    [SerializeField] GameObject weaponPanel;
    [SerializeField] GameObject ammoPanel;
    [SerializeField] GameObject crosshairPanel;
    [SerializeField] private Image pistolCrosshair;
    [SerializeField] private Image shotgunCrosshair;
    [SerializeField] private Image rifleCrosshair;

    [Header("시간 아이콘")]
    [SerializeField] private Image rewindIcon;
    [SerializeField] private Image stopIcon;
    [SerializeField] private Image slowIcon;
    [SerializeField] private Image fastIcon;
    [SerializeField] private float timeBlinkSpeed = 3f;

    [Header("상호작용 프롬프트")]
    [SerializeField] private GameObject promptGroup;
    [SerializeField] private TMP_Text promptText;

    private bool isBlinking;
    private Coroutine lowHpBlinkRoutine;
    private Coroutine blinkRoutine;
    private Coroutine crosshairFireRoutine;

    private void Awake()
    {
        if (hpFillImage == null && hpBar != null && hpBar.fillRect != null)
            hpFillImage = hpBar.fillRect.GetComponent<Image>();
        
        if(mpFillImage == null && mpBar != null && mpBar.fillRect != null)
            mpFillImage = mpBar.fillRect.GetComponent<Image>();

        if(staminaFillImage == null && staminaBar != null && staminaBar.fillRect != null)
            staminaFillImage = staminaBar.fillRect.GetComponent<Image>();
    }

    public void SetPlayerHud(bool isActive)
    {
        if(gameObject.activeSelf != isActive)
            gameObject.SetActive(isActive);
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
        bool isValid = weapon != null;
        weaponPanel.SetActive(isValid);

        if (isValid)
        {
            Image weaponIcon = weaponPanel.GetComponent<Image>();
            weaponIcon.sprite = weapon;
        }
    }

    public void SetAmmoVisible(bool value)
    {
        ammoPanel.SetActive(value);
    }

    public void SetCrosshairVisible(bool value)
    {
        crosshairPanel.SetActive(value);
    }

    // 무기 장착 상태에 따라 크로스헤어 종류 적용
    public void SetCrosshairType(WeaponType type)
    {
        pistolCrosshair.enabled = (type == WeaponType.Pistol);
        shotgunCrosshair.enabled = (type == WeaponType.Shotgun);
        rifleCrosshair.enabled = (type == WeaponType.Rifle);
    }

    // 줌인 / 줌아웃 시 크기 조절
    public void SetCrosshairZoom(bool isZoomed)
    {
        float scale = isZoomed ? 0.6f : 1f;
        crosshairPanel.transform.localScale = Vector3.one * scale;
    }

    // 발사 시 확장 효과 (코루틴)
    public void TriggerCrosshairFireEffect()
    {
        if (crosshairFireRoutine != null)
            StopCoroutine(crosshairFireRoutine);

        crosshairFireRoutine = StartCoroutine(CrosshairFireEffect());
    }

    private IEnumerator CrosshairFireEffect()
    {
        Vector3 originalScale = crosshairPanel.transform.localScale;
        Vector3 expandedScale = originalScale * 1.3f;
        float t = 0f;
        float duration = 0.15f;
        Vector3 targetScale = expandedScale;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float lerp = t / duration;
            crosshairPanel.transform.localScale = Vector3.Lerp(expandedScale, originalScale, lerp);
            yield return null;
        }

        crosshairPanel.transform.localScale = originalScale;
    }

    // 적 조준 색상 전환
    public void SetCrosshairColor(Color c)
    {
        if (pistolCrosshair.enabled) pistolCrosshair.color = c;
        if (shotgunCrosshair.enabled) shotgunCrosshair.color = c;
        if (rifleCrosshair.enabled) rifleCrosshair.color = c;
    }
    #endregion

    #region 상호작용 프롬프트
    public void ShowPrompt(string text)
    {
        promptGroup.SetActive(true);
        promptText.text = text;
    }

    public void HidePrompt()
    {
        promptGroup.SetActive(false);
        promptText.text = "";
    }
    #endregion
}
