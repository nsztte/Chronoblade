using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private PlayerController playerController;
    private WeaponType? cachedWeaponType = null;

    // 연속 입력 체크용 변수
    private static float lastAttackInputTime = -999f;
    private const float COMBO_INPUT_WINDOW = 0.5f; // 0.5초 이내 연속 입력 시 콤보 진입

    private bool wasAttacking = false;

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
        wasAttacking = false;
        // 콤보 매칭 이벤트 구독
        ComboEvaluator.Instance.OnComboMatched += OnComboMatched;
        // 공격 상태 진입 시 비트 루프 시작
        TimingComboManager.Instance.StartBeatRoutine();
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
        // 콤보 매칭 이벤트 구독 해제
        ComboEvaluator.Instance.OnComboMatched -= OnComboMatched;
    }

    public override void Update()
    {
        playerController.LocomotionUpdate();

        var weapon = WeaponManager.Instance.CurrentWeapon;
        if (weapon != null)
        {
            if (wasAttacking && !weapon.IsAttacking)
            {
                stateMachine.ChangeState(new PlayerLocomotionState(stateMachine));
                return;
            }
            wasAttacking = weapon.IsAttacking;
        }
    }

    private void OnLightAttack()
    {
        playerController.PerformLightAttack();
    }

    private void OnHeavyAttack()
    {
        playerController.PerformHeavyAttack();
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

    private void OnComboMatched(ComboSequence combo)
    {
        stateMachine.ChangeState(new PlayerComboState(stateMachine, combo));
    }
}
