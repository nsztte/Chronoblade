using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("HUD")]
    public PlayerHUD playerHUD;
    public CrosshairUI crosshairUI;
    public QuickSlotUI quickSlotUI;
    public ShopUI shopUI;


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

    #region 마우스 커서 업데이트
    public void SetCursorLockState(CursorLockMode mode)
    {
        Cursor.lockState = mode;
        Cursor.visible = mode == CursorLockMode.None;
    }
    #endregion

    #region 플레이어 상태 업데이트
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

    // // === 크로스헤어, 퀵슬롯 ===
    // public void SetCrosshairActive(bool isActive)
    // {
    //     crosshairUI?.SetActive(isActive);
    // }

    // public void SetQuickSlotSelectedIndex(int index)
    // {
    //     quickSlotUI?.SetSelectedIndex(index);
    // }
    #endregion

    #region 게임스테이트 디버그용 (추후 UI로 대체)
    public void ShowMainMenu() => Debug.Log("[UI] Show Main Menu");
    public void HideMainMenu() => Debug.Log("[UI] Hide Main Menu");

    public void ShowLoadingScreen() => Debug.Log("[UI] Show Loading Screen");
    public void HideLoadingScreen() => Debug.Log("[UI] Hide Loading Screen");

    public void ShowHUD() => Debug.Log("[UI] Show HUD");
    public void HideHUD() => Debug.Log("[UI] Hide HUD");

    public void ShowCombatHUD() => Debug.Log("[UI] Show Combat HUD");
    public void HideCombatHUD() => Debug.Log("[UI] Hide Combat HUD");

    public void ShowPauseMenu() => Debug.Log("[UI] Show Pause Menu");
    public void HidePauseMenu() => Debug.Log("[UI] Hide Pause Menu");

    public void ShowGameOverScreen() => Debug.Log("[UI] Show GameOver Screen");
    public void HideGameOverScreen() => Debug.Log("[UI] Hide GameOver Screen");

    public void ShowPuzzleUI() => Debug.Log("[UI] Show Puzzle UI");
    public void HidePuzzleUI() => Debug.Log("[UI] Hide Puzzle UI");

    public void ShowCutsceneOverlay() => Debug.Log("[UI] Show Cutscene Overlay");
    public void HideCutsceneOverlay() => Debug.Log("[UI] Hide Cutscene Overlay");
    #endregion
}
