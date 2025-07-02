using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private PlayerController playerController;
    private WeaponType? cachedWeaponType = null;

    // 연속 입력 체크용 변수
    private static float lastAttackInputTime = -999f;
    private const float COMBO_INPUT_WINDOW = 0.5f; // 0.5초 이내 연속 입력 시 콤보 진입

    private bool wasAttacking = false;
    private bool isComboTriggered = false; // 콤보 상태 전이 방지 플래그

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
        isComboTriggered = false; // 콤보 트리거 초기화
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
        // 비트 루프 종료
        // TimingComboManager.Instance.StopBeatRoutine();
        // // 입력 버퍼 클리어
        // ComboEvaluator.Instance.ClearInputBuffer();
    }

    public override void Update()
    {
        if (isComboTriggered) return; // 콤보 상태 전이 중이면 아무것도 하지 않음
        playerController.LocomotionUpdate();
        var weapon = WeaponManager.Instance.CurrentWeapon;
        if (weapon != null)
        {
            // 콤보가 트리거된 경우에는 LocomotionState로 전이하지 않음
            if (!isComboTriggered && wasAttacking && !weapon.IsAttacking)
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
        if (isComboTriggered) return;
        isComboTriggered = true;
        stateMachine.ChangeState(new PlayerComboState(stateMachine, combo));
    }
}
