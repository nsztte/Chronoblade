using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    #region 싱글톤 및 초기화
    public static InputManager Instance { get; private set; }

    public void Initialize()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning($"{GetType().Name} 인스턴스 감지됨, 초기화 스킵");
            return;
        }
        Instance = this;

        LoadControlSettings();
    }
    #endregion

    #region Events
    public event Action<Vector2> OnMoveInput;         // WASD
    public event Action<Vector2> OnLookInput;         // 마우스
    public event Action OnJumpPressed;                // Space
    public event Action OnRunStarted;                 // 왼쪽 Shift 시작
    public event Action OnRunCanceled;                // 왼쪽 Shift 종료
    public event Action OnCrouchPressed;              // 왼쪽 Ctrl
    public event Action OnAttackPressed;              // 마우스 좌클릭
    public event Action OnAttackHeld;                 // 마우스 좌클릭 유지
    public event Action OnReloadPressed;              // R
    public event Action OnAimStarted;                 // 마우스 오른쪽 클릭
    public event Action OnAimCanceled;                // 마우스 오른쪽 클릭 종료
    // public event Action<int> OnWeaponSwitch;          // 숫자 키 1~4
    public event Action OnInteract;                   // F
    public event Action OnPause;                      // Esc
    public event Action OnDashPressed;                // Left Alt
    public event Action OnBlockStarted;               // 마우스 우클릭 시작
    public event Action OnBlockCanceled;              // 마우스 우클릭 종료
    public event Action OnLightAttackPressed;
    public event Action OnHeavyAttackPressed;

    public event Action OnInventoryChanged;          // I
    public event Action OnQuickSave;                  // F5
    #endregion

    #region 옵션 무브 값
    private bool toggleSprint;
    private bool toggleCrouch;
    private bool invertMouseY;
    private float mouseSensitivityX = 1f;
    private float mouseSensitivityY = 1f;
    private bool sprintState;
    private bool crouchState;
    #endregion
    
    #region playerprefs 전용 상수
    private const string PREF_TOGGLE_SPRINT = "Controls_Toggle_Sprint";
    private const string PREF_TOGGLE_AIM = "Controls_Toggle_Aim";
    private const string PREF_TOGGLE_CROUCH = "Controls_Toggle_Crouch";
    private const string PREF_INVERT_Y = "Controls_Invert_Y";
    private const string PREF_INVERT_SCROLL = "Controls_Invert_Scroll";
    private const string PREF_SENS_X = "Controls_Mouse_SensX";
    private const string PREF_SENS_Y = "Controls_Mouse_SensY";
    #endregion
    private float attackKeyDownTime;
    private const float LIGHT_ATTACK_THRESHOLD = 0.2f;

    private bool isInputEnabled = true;

    void Update()
    {
        if (!isInputEnabled)
            return;

        // 1) UI 열려 있으면 허용 키만 처리하고 종료
        if (HandleUIBlockingInput())
            return;

        // 2) 아무 UI도 안 열렸을 때 전역 핫키(ESC/I)는 항상 처리
        if (Input.GetKeyDown(KeyCode.Escape))
            OnPause?.Invoke();

        if (Input.GetKeyDown(KeyCode.I))
            OnInventoryChanged?.Invoke();

        // 3) 커서 잠금 해제면 게임플레이 입력 차단
        if (Cursor.lockState != CursorLockMode.Locked)
            return;
          
        // 빙의시 상호작용 외에 다른 움직임 차단
        if (PlayerManager.Instance.IsPossessed)
        {
            if (Input.GetKeyDown(KeyCode.F))
                OnInteract?.Invoke();

            return;
        }

        // 시간 정지 상태에서는 모든 입력 및 시점 회전 차단
        if(PlayerManager.Instance.IsFrozen) return; 

        // 시점 회전 입력 (마우스)
        Vector2 rawLookInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector2 adjustedLook = new Vector2(
            rawLookInput.x * mouseSensitivityX,
            (invertMouseY ? -1 : 1) * rawLookInput.y * mouseSensitivityY
        );
        OnLookInput?.Invoke(adjustedLook);

        // 마비 상태에서는 시점 회전만 적용, 나머지 조작 차단
        if(PlayerManager.Instance.IsParalyzed) return;

        // 이동 입력 (WASD)
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        OnMoveInput?.Invoke(moveInput);

        // 점프 입력
        if (Input.GetKeyDown(KeyCode.Space))
            OnJumpPressed?.Invoke();
            
        // 달리기 입력
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (toggleSprint)
            {
                sprintState = !sprintState;
                if (sprintState) OnRunStarted?.Invoke();
                else OnRunCanceled?.Invoke();
            }
            else
            {
                OnRunStarted?.Invoke();
            }
        }
        if (!toggleSprint && Input.GetKeyUp(KeyCode.LeftShift))
        {
            OnRunCanceled?.Invoke();
        }

        // 웅크리기 입력
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (toggleCrouch)
            {
                crouchState = !crouchState;
                if (crouchState) OnCrouchPressed?.Invoke(); // 토글 ON
                else OnCrouchPressed?.Invoke();
            }
            else
            {
                OnCrouchPressed?.Invoke();
            }
        }
        if (!toggleCrouch && Input.GetKeyUp(KeyCode.LeftControl))
        {
            OnCrouchPressed?.Invoke();
        }

        // 총기류 공격 입력 (좌클릭)
        if (Input.GetMouseButtonDown(0))
        {
            OnAttackPressed?.Invoke(); // 총기류 공격용
            attackKeyDownTime = Time.time;  // 근접 공격 입력 시간 기록
        }
        if (Input.GetMouseButton(0))
        {
            OnAttackHeld?.Invoke(); // 총기류 공격용
        }
        if (Input.GetMouseButtonUp(0))
        {
            float duration = Time.time - attackKeyDownTime;
            // 근접 공격 입력(누르는 시간에 따라)
            if (duration < LIGHT_ATTACK_THRESHOLD)
            {
                OnLightAttackPressed?.Invoke(); // 근접 공격용
            }
            else
            {
                OnHeavyAttackPressed?.Invoke(); // 근접 공격용
            }
        }

        // 재장전 입력 (R)
        if (Input.GetKeyDown(KeyCode.R))
            OnReloadPressed?.Invoke();

        // 총기류 조준 입력 (마우스 오른쪽 클릭)
        if (Input.GetMouseButtonDown(1))
        {
            OnAimStarted?.Invoke();
            OnBlockStarted?.Invoke();
        }

        // 총기류 조준 취소 입력 (마우스 오른쪽 클릭 취소)
        if (Input.GetMouseButtonUp(1))
        {
            OnAimCanceled?.Invoke();
            OnBlockCanceled?.Invoke();
        }

        // 퀵슬롯 입력
        HandleQuickSlotActivation();

        // 상호작용(F키)
        if (Input.GetKeyDown(KeyCode.F))
            OnInteract?.Invoke();

        // 대쉬 입력 (Left Alt)
        if (Input.GetKeyDown(KeyCode.LeftAlt))
            OnDashPressed?.Invoke();

        if(Input.GetKeyDown(KeyCode.I))
            OnInventoryChanged?.Invoke();

        if(Input.GetKeyDown(KeyCode.F5))
            OnQuickSave?.Invoke();
    }

    private void LoadControlSettings()
    {
        toggleSprint = PlayerPrefs.GetInt(PREF_TOGGLE_SPRINT, 0) == 1;
        toggleCrouch = PlayerPrefs.GetInt(PREF_TOGGLE_CROUCH, 0) == 1;
        invertMouseY = PlayerPrefs.GetInt(PREF_INVERT_Y, 0) == 1;

        mouseSensitivityX = PlayerPrefs.GetFloat(PREF_SENS_X, 1f);
        mouseSensitivityY = PlayerPrefs.GetFloat(PREF_SENS_Y, 1f);
    }

    public void SetInputEnabled(bool enabled)
    {
        isInputEnabled = enabled;
    }

    public void TriggerPause()
    {
        OnPause?.Invoke();
    }

    private bool HandleUIBlockingInput()
    {
        var ui = UIManager.Instance;
        if (ui != null)
        {
            if (ui.IsPauseOpen)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                    OnPause?.Invoke();
                return true;
            }

            if (ui.IsInventoryOpen)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                    OnPause?.Invoke();
                if (Input.GetKeyDown(KeyCode.I))
                    OnInventoryChanged?.Invoke();
                return true;
            }

            if (ui.IsShopOpen)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                    OnPause?.Invoke();
                return true;
            }
        }

        return false;
    }

    private void HandleQuickSlotActivation()
    {
        // 1~4번 퀵슬롯 키 입력 → 아이템 사용 또는 장착
        if (Input.GetKeyDown(KeyCode.Alpha1))
            QuickSlotManager.Instance.ActivateSlot(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            QuickSlotManager.Instance.ActivateSlot(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            QuickSlotManager.Instance.ActivateSlot(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            QuickSlotManager.Instance.ActivateSlot(3);

        // 마우스 휠 → 퀵슬롯에 등록된 무기 중 다음/이전 무기로 전환
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheel == 0f) return;

        int currentIndex = QuickSlotManager.Instance.GetCurrentWeaponSlotIndex(); // 현재 장착 중인 무기의 퀵슬롯 인덱스
        int direction = scrollWheel > 0f ? -1 : 1;

        int nextWeaponIndex = QuickSlotManager.Instance.GetNextWeaponSlotIndex(currentIndex, direction);
        if (nextWeaponIndex != currentIndex)
        {
            QuickSlotManager.Instance.ActivateSlot(nextWeaponIndex);
        }
    }


    // private void HandleWeaponSwitching()
    // {
    //     int currentWeaponIndex = WeaponManager.Instance.GetCurrentWeaponIndex();
    //     int maxWeaponCount = WeaponManager.Instance.GetMaxWeaponCount();
    //     var weaponSlots = WeaponManager.Instance.GetWeaponSlots();

    //     // 숫자 키로 직접 무기 선택
    //     if (Input.GetKeyDown(KeyCode.Alpha1))
    //     {
    //         SwitchWeapon(0, currentWeaponIndex, maxWeaponCount);
    //     }
    //     else if (Input.GetKeyDown(KeyCode.Alpha2))
    //     {
    //         SwitchWeapon(1, currentWeaponIndex, maxWeaponCount);
    //     }
    //     else if (Input.GetKeyDown(KeyCode.Alpha3))
    //     {
    //         SwitchWeapon(2, currentWeaponIndex, maxWeaponCount);
    //     }
    //     else if (Input.GetKeyDown(KeyCode.Alpha4))
    //     {
    //         SwitchWeapon(3, currentWeaponIndex, maxWeaponCount);
    //     }

    //     // 마우스 휠로 무기 전환
    //     float scrollWheel = Input.GetAxis("Mouse ScrollWheel");

    //     if(currentWeaponIndex < 0) return;

    //     if (scrollWheel > 0f) // 휠 위로
    //     {
    //         int nextIndex = GetNextObtainedWeaponIndex(currentWeaponIndex, -1, weaponSlots);
    //         if(nextIndex != currentWeaponIndex)
    //             SwitchWeapon(nextIndex, currentWeaponIndex, maxWeaponCount);
    //     }
    //     else if (scrollWheel < 0f) // 휠 아래로
    //     {
    //         int nextIndex = GetNextObtainedWeaponIndex(currentWeaponIndex, 1, weaponSlots);
    //         if(nextIndex != currentWeaponIndex)
    //             SwitchWeapon(nextIndex, currentWeaponIndex, maxWeaponCount);
    //     }
    // }

    // private int GetNextObtainedWeaponIndex(int startIndex, int direction, List<WeaponController> slots)
    // {
    //     int count = slots.Count;
    //     int index = startIndex;

    //     for(int i = 0; i < count; i++)
    //     {
    //         index = (index + direction + count) % count;

    //         var data = slots[index].ItemData;
    //         if (InventoryManager.Instance.IsWeaponObtained(data))
    //         {
    //             return index;
    //         }
    //     }

    //     return startIndex;
    // }

    // private void SwitchWeapon(int weaponIndex, int currentIndex, int maxCount)
    // {
    //     if (weaponIndex >= 0 && weaponIndex < maxCount)
    //     {
    //         // 공격/콤보 상태일 때 무기 교체 금지
    //         var playerManager = PlayerManager.Instance;
    //         if (playerManager != null && playerManager.PlayerStateMachine != null)
    //         {
    //             var state = playerManager.PlayerStateMachine.CurrentState;
    //             if (state != null && (state is PlayerAttackState || state is PlayerComboState))
    //             {
    //                 Debug.Log("[InputManager] 공격/콤보 상태에서 무기 교체 시도 차단");
    //                 return;
    //             }
    //         }
            
    //         // 현재 무기가 공격 중일 때 무기 교체 금지
    //         var currentWeapon = WeaponManager.Instance.CurrentWeapon;
    //         if (currentWeapon != null && currentWeapon.IsAttacking)
    //         {
    //             Debug.Log("[InputManager] 공격 중 무기 교체 시도 차단");
    //             return;
    //         }

    //         if (weaponIndex == currentIndex)
    //         {
    //             // 이미 장착된 무기일 경우 장착 해제
    //             WeaponManager.Instance.UnEquipWeapon();
    //         }
    //         else
    //         {
    //             // 다른 무기로 전환
    //             OnWeaponSwitch?.Invoke(weaponIndex);
    //         }
    //     }
    // }
}
