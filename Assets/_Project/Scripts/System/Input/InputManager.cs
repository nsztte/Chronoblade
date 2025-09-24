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
    public event Action OnQuickSave;                 // F5
    public event Action OnPressAnyKey;
    #endregion

    #region 옵션 설정값
    // playerprefs 키값
    public static class Prefs
    {
        public const string MOUSE_SENS_X = "Controls_Mouse_SensX";
        public const string MOUSE_SENS_Y = "Controls_Mouse_SensY";
        public const string INVERT_Y = "Controls_Invert_Y";
        public const string TOGGLE_SPRINT = "Controls_Toggle_Sprint";
        public const string TOGGLE_CROUCH = "Controls_Toggle_Crouch";
    }

    public float MouseSensitivityX { get; private set; } = 1f;
    public float MouseSensitivityY { get; private set; } = 1f;
    public bool InvertMouseY { get; private set; } = false;
    public bool ToggleSprint { get; private set; } = false;
    public bool ToggleCrouch { get; private set; } = false;

    private bool sprintState;
    private bool crouchState;
    #endregion
    
    private float attackKeyDownTime;
    private const float LIGHT_ATTACK_THRESHOLD = 0.2f;

    private bool isInputEnabled = true;

    void Update()
    {
        if(Input.anyKeyDown)
            OnPressAnyKey?.Invoke();
            
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
            rawLookInput.x * MouseSensitivityX,
            (InvertMouseY ? -1 : 1) * rawLookInput.y * MouseSensitivityY
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
            if (ToggleSprint)
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
        if (!ToggleSprint && Input.GetKeyUp(KeyCode.LeftShift))
        {
            OnRunCanceled?.Invoke();
        }

        // 웅크리기 입력
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (ToggleCrouch)
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
        if (!ToggleCrouch && Input.GetKeyUp(KeyCode.LeftControl))
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
        MouseSensitivityX = PlayerPrefs.GetFloat(Prefs.MOUSE_SENS_X, 1f);
        MouseSensitivityY = PlayerPrefs.GetFloat(Prefs.MOUSE_SENS_Y, 1f);
        InvertMouseY = PlayerPrefs.GetInt(Prefs.INVERT_Y, 0) == 1;
        ToggleSprint = PlayerPrefs.GetInt(Prefs.TOGGLE_SPRINT, 0) == 1;
        ToggleCrouch = PlayerPrefs.GetInt(Prefs.TOGGLE_CROUCH, 0) == 1;
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

    #region 옵션용 Setter
    public void SetMouseSensitivity(float x, float y)
    {
        MouseSensitivityX = x;
        MouseSensitivityY = y;
        PlayerPrefs.SetFloat(Prefs.MOUSE_SENS_X, x);
        PlayerPrefs.SetFloat(Prefs.MOUSE_SENS_Y, y);
        PlayerPrefs.Save();
    }

    public void SetInvertMouseY(bool invert)
    {
        InvertMouseY = invert;
        PlayerPrefs.SetInt(Prefs.INVERT_Y, invert ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetToggleSprint(bool v)
    {
        ToggleSprint = v;
        PlayerPrefs.SetInt(Prefs.TOGGLE_SPRINT, v ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetToggleCrouch(bool v)
    {
        ToggleCrouch = v;
        PlayerPrefs.SetInt(Prefs.TOGGLE_CROUCH, v ? 1 : 0);
        PlayerPrefs.Save();
    }
    #endregion
}
