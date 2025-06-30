using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private PlayerController playerController;
    // private float attackStartTime;
    // private const float ATTACK_DURATION = 0.5f; // 기본 공격 지속 시간
    // private const float MAX_ATTACK_DURATION = 3f; // 최대 공격 지속 시간 (안전장치)
    private WeaponType? cachedWeaponType = null;

    // 연속 입력 체크용 변수
    private static float lastAttackInputTime = -999f;
    private const float COMBO_INPUT_WINDOW = 0.5f; // 0.5초 이내 연속 입력 시 콤보 진입

    public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        playerController = stateMachine.GetComponent<PlayerController>();
    }

    public override void Enter()
    {
        // attackStartTime = Time.time;
        var weapon = WeaponManager.Instance.CurrentWeapon;
        if (weapon != null)
        {
            cachedWeaponType = weapon.weaponData.weaponType;
            if (cachedWeaponType == WeaponType.Sword)
            {
                InputManager.Instance.OnLightAttackPressed += OnLightAttack;
                InputManager.Instance.OnHeavyAttackPressed += OnHeavyAttack;
            }
            else if(cachedWeaponType == WeaponType.Rifle)
            {
                InputManager.Instance.OnAttackHeld += OnAttackHeld;
            }
            else
            {
                InputManager.Instance.OnAttackPressed += OnAttackPressed;
            }
        }
        Debug.Log("PlayerAttackState 진입");
    }

    public override void Exit()
    {
        if (cachedWeaponType == WeaponType.Sword)
        {
            InputManager.Instance.OnLightAttackPressed -= OnLightAttack;
            InputManager.Instance.OnHeavyAttackPressed -= OnHeavyAttack;
        }
        else if(cachedWeaponType == WeaponType.Rifle)
        {
            InputManager.Instance.OnAttackHeld -= OnAttackHeld;
        }
        else
        {
            InputManager.Instance.OnAttackPressed -= OnAttackPressed;
        }

        Debug.Log("PlayerAttackState 종료");
    }

    public override void Update()
    {
        playerController.LocomotionUpdate();
        // 연사 총기(라이플)는 버튼을 떼면 상태 전환
        if (cachedWeaponType != WeaponType.Sword)
        {
            if (Input.GetMouseButtonUp(0))
            {
                stateMachine.ChangeState(new PlayerLocomotionState(stateMachine));
            }
        }
    }

    private void OnLightAttack()
    {
        playerController.PerformLightAttack();
        if (cachedWeaponType == WeaponType.Sword && ShouldEnterCombo())
        {
            Debug.Log("[콤보] 콤보 조건 충족, PlayerComboState로 전환");
            stateMachine.ChangeState(new PlayerComboState(stateMachine));
        }
        else
        {
            // 콤보가 아니면 공격 입력 후 LocomotionState로 전환
            stateMachine.ChangeState(new PlayerLocomotionState(stateMachine));
        }
    }

    private void OnHeavyAttack()
    {
        playerController.PerformHeavyAttack();
        if (cachedWeaponType == WeaponType.Sword && ShouldEnterCombo())
        {
            Debug.Log("[콤보] 콤보 조건 충족, PlayerComboState로 전환");
            stateMachine.ChangeState(new PlayerComboState(stateMachine));
        }
        else
        {
            // 콤보가 아니면 공격 입력 후 LocomotionState로 전환
            stateMachine.ChangeState(new PlayerLocomotionState(stateMachine));
        }
    }

    private void OnAttackPressed()
    {
        Debug.Log("[공격] 원거리 무기 공격 입력");
        playerController.PerformWeaponAttack();
    }

    private void OnAttackHeld()
    {
        Debug.Log("[공격] 원거리 무기 공격(홀드) 입력");
        playerController.PerformWeaponAttack();
    }

    // 일정 시간 내 연속 입력 시 콤보 진입
    private bool ShouldEnterCombo()
    {
        float now = Time.time;
        bool isCombo = (now - lastAttackInputTime) < COMBO_INPUT_WINDOW;
        lastAttackInputTime = now;
        return isCombo;
    }
}
