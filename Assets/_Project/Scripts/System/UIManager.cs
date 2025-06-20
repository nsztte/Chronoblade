using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("HUD")]
    public PlayerHUD playerHUD;
    public CrosshairUI crosshairUI;
    public QuickSlotUI quickSlotUI;


    #region Singleton
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    // === 체력, MP, 스태미나 ===
    public void UpdateHP(int current, int max)
    {
        playerHUD?.UpdateHP(current, max);
    }

    public void UpdateMP(int current, int max)
    {
        playerHUD?.UpdateMP(current, max);
    }

    public void UpdateStamina(int current, int max)
    {
        playerHUD?.UpdateStamina(current, max);
    }

    // === 탄약 ===
    public void UpdateAmmo(int current, int total)
    {
        playerHUD?.UpdateAmmo(current, total);
    }

    // === 골드 ===
    public void UpdateGold(int amount)
    {
        playerHUD?.UpdateGold(amount);
    }

    // === 크로스헤어, 퀵슬롯 ===
    public void SetCrosshairActive(bool isActive)
    {
        crosshairUI?.SetActive(isActive);
    }

    public void SetQuickSlotSelectedIndex(int index)
    {
        quickSlotUI?.SetSelectedIndex(index);
    }
}
