using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
[DisallowMultipleComponent]
public class BossHUD : MonoBehaviour
{
    [SerializeField] private Image hpFill;
    private CanvasGroup group;

    public void Awake()
    {
        group = GetComponent<CanvasGroup>();
    }

    public void Show(float cur, float max)
    {
        gameObject.SetActive(true);
        group.alpha = 1f;
        SetHP(cur, max);
    }

    public void Hide()
    {
        group.alpha = 0f;
        gameObject.SetActive(false);
    }

    public void SetHP(float cur, float max)
    {
        hpFill.fillAmount = max > 0 ? cur / max : 0f;
    }
}
