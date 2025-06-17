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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnWeaponSwitch?.Invoke(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnWeaponSwitch?.Invoke(1);
        }
    }
}
