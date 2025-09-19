using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayerHUD playerHUD;
    [SerializeField] private BossHUD bossHUD;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private ShopUI shopUI;
    [SerializeField] private ToastUI toastUI;
    [SerializeField] private ConfirmModalUI confirmModal;
    [SerializeField] private PauseUI pauseUI;
    [SerializeField] private TooltipUI tooltipUI;
    [SerializeField] private FadeUI fadeUI;
    [SerializeField] private OptionUI optionUI;

    [Header("오버레이")]
    [SerializeField] private GameObject overlayBackground;
    private int overlayCount = 0;

    [Header("퀵슬롯")]
    [SerializeField] private List<QuickSlotSlot> quickSlots;

    public InventoryUI InventoryUI => inventoryUI;
    public ShopUI ShopUI => shopUI;
    public TooltipUI TooltipUI => tooltipUI;
    public FadeUI FadeUI => fadeUI;
    public OptionUI OptionUI => optionUI;

    public bool IsPauseOpen => pauseUI != null && pauseUI.gameObject.activeSelf;
    public bool IsInventoryOpen => inventoryUI != null && inventoryUI.gameObject.activeSelf;
    public bool IsShopOpen => shopUI != null && shopUI.gameObject.activeSelf;
    public bool IsOptionOpen => optionUI != null && optionUI.gameObject.activeSelf;
    public bool IsAnyUIOpen => IsPauseOpen || IsInventoryOpen || IsShopOpen || IsOptionOpen;


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

    public void Start()
    {
        InputManager.Instance.OnInventoryChanged += ToggleInventoryUI;

        QuickSlotManager.Instance.BindQuickSlotSlot(quickSlots);
    }

    public void OnDestroy()
    {
        InputManager.Instance.OnInventoryChanged -= ToggleInventoryUI;
    }

    public void UpdateUI(bool value)
    {
        if(gameObject.activeSelf != value)
            gameObject.SetActive(value);
    }

    private void ToggleInventoryUI()
    {
        if (IsShopOpen) return;

        bool isActive = inventoryUI.gameObject.activeSelf;

        if (isActive)
        {
            inventoryUI.gameObject.SetActive(false);
            // InputManager.Instance.TriggerPause();
        }
        else
        {
            inventoryUI.Open();
        }
    }

    #region 마우스 커서 업데이트
    public void SetCursorLockState(CursorLockMode mode)
    {
        Cursor.lockState = mode;
        Cursor.visible = mode == CursorLockMode.None;
    }
    #endregion

    #region 플레이어 상태 업데이트
    public void UpdatePlayerHud(bool value) => playerHUD?.SetPlayerHud(value);
    // === 체력, MP, 스태미나 ===
    public void UpdateHP(int current, int max) => playerHUD?.UpdateHP(current, max);

    public void UpdateMP(int current, int max) => playerHUD?.UpdateMP(current, max);

    public void UpdateStamina(int current, int max) => playerHUD?.UpdateStamina(current, max);

    // === 무기 ===
    public void UpdateAmmo(int current, int total) => playerHUD?.UpdateAmmo(current, total);

    public void ActiveWeaponPanel(Sprite weaponIcon = null) => playerHUD?.SetWeaponImage(weaponIcon);

    public void ActiveAmmoPanel(bool value) => playerHUD?.SetAmmoVisible(value);

    // === 골드 ===
    public void UpdateGold(int amount) => playerHUD?.UpdateGold(amount);

    // public void SetQuickSlotSelectedIndex(int index)
    // {
    //     quickSlotUI?.SetSelectedIndex(index);
    // }
    #endregion

    #region 보스 HP바
    public void ShowBoss(float cur, float max) => bossHUD?.Show(cur, max);
    public void SetBossHP(float cur, float max) => bossHUD?.SetHP(cur, max);
    public void HideBoss() => bossHUD?.Hide();
    #endregion

    #region 크로스헤어
    public void SetCrosshairActive(bool value) => playerHUD?.SetCrosshairVisible(value);

    // 무기 타입에 따라 크로스헤어 교체
    public void UpdateCrosshair(WeaponType type) => playerHUD?.SetCrosshairType(type);

    // 조준 상태에 따라 크기 조절
    public void SetCrosshairZoom(bool isZoomed) => playerHUD?.SetCrosshairZoom(isZoomed);

    // 총 발사 시 크로스헤어 확대 효과
    public void TriggerCrosshairFireEffect() => playerHUD?.TriggerCrosshairFireEffect();

    // 조준 대상이 사정거리 내일 때 색상 변경
    public void SetCrosshairColor(Color color) => playerHUD?.SetCrosshairColor(color);
    #endregion

    #region 상호작용 프롬프트
    public void ShowPrompt(string text) => playerHUD?.ShowPrompt(text);

    public void HidePrompt() => playerHUD?.HidePrompt();
    #endregion

    #region 타임 스테이트 업데이트
    public void ShowTimeState(TimeState state) => playerHUD?.ShowTimeState(state);

    public void ClearTimeState() => playerHUD?.ClearTimeState();
    #endregion
    
    #region 오버레이, 토스트UI, 컨펌모달UI, 일시정지UI
    public void ShowOverlayBackground()
    {
        overlayCount++;
        if (overlayCount == 1)
        {
            overlayBackground?.SetActive(true);
            SetCursorLockState(CursorLockMode.None);
            GameManager.Instance.EnterPaused();
        }
    }

    public void HideOverlayBackground()
    {
        overlayCount = Mathf.Max(overlayCount - 1, 0);
        if (overlayCount == 0)
        {
            overlayBackground?.SetActive(false);
            SetCursorLockState(CursorLockMode.Locked);
            GameManager.Instance.EnterPreviousState();
        }
    }

    public void ShowToast(string message) => toastUI?.Show(message);

    public void ShowConfirm(string title, string message, Action onConfirm, Action onCancel) => confirmModal?.Show(title, message, onConfirm, onCancel);

    public void ShowPause() => pauseUI?.Show();

    public void ClosePause() => pauseUI?.Hide();
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
