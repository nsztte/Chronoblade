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
    public event Action OnAttackPressed;              // 좌클릭
    public event Action OnSkillPressed;               // 우클릭
    public event Action<int> OnWeaponSwitch;          // 숫자 키 1~2
    #endregion
    void Update()
    {
        // 이동 입력 (WASD)
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        OnMoveInput?.Invoke(moveInput);

        // 시점 회전 입력 (마우스)
        Vector2 lookInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        OnLookInput?.Invoke(lookInput);

        // 점프 입력
        if (Input.GetKeyDown(KeyCode.Space))
            OnJumpPressed?.Invoke();

        // 공격 입력
        if (Input.GetMouseButtonDown(0))
            OnAttackPressed?.Invoke();

        // 스킬 입력
        if (Input.GetMouseButtonDown(1))
            OnSkillPressed?.Invoke();

        // 무기 전환 입력
        HandleWeaponSwitching();
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
