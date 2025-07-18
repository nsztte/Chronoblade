using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    #region Singleton
    public static InputManager Instance { get; private set; }

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
    public event Action<int> OnWeaponSwitch;          // 숫자 키 1~4
    public event Action OnInteract;                   // F
    public event Action OnPause;                      // Esc
    public event Action OnDashPressed;                // Left Alt
    public event Action OnBlockStarted;               // 마우스 우클릭 시작
    public event Action OnBlockCanceled;              // 마우스 우클릭 종료
    public event Action OnLightAttackPressed;
    public event Action OnHeavyAttackPressed;

    #endregion

    private float attackKeyDownTime;
    private const float LIGHT_ATTACK_THRESHOLD = 0.2f;

    void Update()
    {
        if(PlayerManager.Instance.IsFrozen) return;

        // 이동 입력 (WASD)
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        OnMoveInput?.Invoke(moveInput);

        // 시점 회전 입력 (마우스)
        Vector2 lookInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        OnLookInput?.Invoke(lookInput);

        // 점프 입력
        if (Input.GetKeyDown(KeyCode.Space))
            OnJumpPressed?.Invoke();

        // 달리기 입력
        if (Input.GetKeyDown(KeyCode.LeftShift))
            OnRunStarted?.Invoke();
        if (Input.GetKeyUp(KeyCode.LeftShift))
            OnRunCanceled?.Invoke();

        // 웅크리기 입력
        if (Input.GetKeyDown(KeyCode.LeftControl))
            OnCrouchPressed?.Invoke();

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

        // 무기 전환 입력
        HandleWeaponSwitching();

        // 상호작용(F키)
        if (Input.GetKeyDown(KeyCode.F))
            OnInteract?.Invoke();

        // 일시정지(Esc)
        if (Input.GetKeyDown(KeyCode.Escape))
            OnPause?.Invoke();

        // 대쉬 입력 (Left Alt)
        if (Input.GetKeyDown(KeyCode.LeftAlt))
            OnDashPressed?.Invoke();
    }

    private void HandleWeaponSwitching()
    {
        int currentWeaponIndex = WeaponManager.Instance.GetCurrentWeaponIndex();
        int maxWeaponCount = WeaponManager.Instance.GetMaxWeaponCount();

        // 숫자 키로 직접 무기 선택
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(0, currentWeaponIndex, maxWeaponCount);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(1, currentWeaponIndex, maxWeaponCount);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchWeapon(2, currentWeaponIndex, maxWeaponCount);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchWeapon(3, currentWeaponIndex, maxWeaponCount);
        }

        // 마우스 휠로 무기 전환
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");

        if(currentWeaponIndex < 0) return;

        if (scrollWheel > 0f) // 휠 위로
        {
            int nextWeapon = (currentWeaponIndex - 1 + maxWeaponCount) % maxWeaponCount;
            SwitchWeapon(nextWeapon, currentWeaponIndex, maxWeaponCount);
        }
        else if (scrollWheel < 0f) // 휠 아래로
        {
            int nextWeapon = (currentWeaponIndex + 1) % maxWeaponCount;
            SwitchWeapon(nextWeapon, currentWeaponIndex, maxWeaponCount);
        }
    }

    private void SwitchWeapon(int weaponIndex, int currentIndex, int maxCount)
    {
        if (weaponIndex >= 0 && weaponIndex < maxCount)
        {
            // 공격/콤보 상태일 때 무기 교체 금지
            var playerManager = PlayerManager.Instance;
            if (playerManager != null && playerManager.PlayerStateMachine != null)
            {
                var state = playerManager.PlayerStateMachine.CurrentState;
                if (state != null && (state is PlayerAttackState || state is PlayerComboState))
                {
                    Debug.Log("[InputManager] 공격/콤보 상태에서 무기 교체 시도 차단");
                    return;
                }
            }
            
            // 현재 무기가 공격 중일 때 무기 교체 금지
            var currentWeapon = WeaponManager.Instance.CurrentWeapon;
            if (currentWeapon != null && currentWeapon.IsAttacking)
            {
                Debug.Log("[InputManager] 공격 중 무기 교체 시도 차단");
                return;
            }

            if (weaponIndex == currentIndex)
            {
                // 이미 장착된 무기일 경우 장착 해제
                WeaponManager.Instance.UnEquipWeapon();
            }
            else
            {
                // 다른 무기로 전환
                OnWeaponSwitch?.Invoke(weaponIndex);
            }
        }
    }
}
