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
        
        // 새로운 ComboEvaluator 이벤트 구독
        ComboEvaluator.Instance.OnComboAttackExecuted += OnComboAttackExecuted;
        ComboEvaluator.Instance.OnComboCompleted += OnComboCompleted;
        ComboEvaluator.Instance.OnComboFailed += OnComboFailed;
        ComboEvaluator.Instance.OnNormalAttackExecuted += OnNormalAttackExecuted;
        
        TimingComboManager.Instance.StartBeatRoutine();
    }

    public override void Exit()
    {
        var weapon = WeaponManager.Instance.CurrentWeapon;
        if (weapon != null)
        {
            weapon.SetAttackingFalse(); // 상태 전이 시 무조건 공격 상태 해제
        }
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
        
        // 새로운 ComboEvaluator 이벤트 구독 해제
        ComboEvaluator.Instance.OnComboAttackExecuted -= OnComboAttackExecuted;
        ComboEvaluator.Instance.OnComboCompleted -= OnComboCompleted;
        ComboEvaluator.Instance.OnComboFailed -= OnComboFailed;
        ComboEvaluator.Instance.OnNormalAttackExecuted -= OnNormalAttackExecuted;
        
        // TimingComboManager.Instance.StopBeatRoutine();
        // ComboEvaluator.Instance.ClearInputBuffer();
    }

    public override void Update()
    {
        if (isComboTriggered) return; // 콤보 상태 전이 중이면 아무것도 하지 않음
        playerController.LocomotionUpdate();
        
        var weapon = WeaponManager.Instance.CurrentWeapon;
        if (weapon != null)
        {
            if(weapon.weaponData.weaponType != WeaponType.Sword)
            {
                var gun = weapon as GunWeaponController;
                if(gun.GetCurrentAmmoCount() <= 0)
                {
                    stateMachine.ChangeState(new PlayerLocomotionState(stateMachine));
                    return;
                }
            }

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

    private void OnComboAttackExecuted(ComboSequence combo, int step, ComboAttackData attackData)
    {
        if (isComboTriggered) return;
        // 콤보 공격 실행 (애니메이션, 데미지 등)
        Debug.Log($"[PlayerAttackState] 콤보 공격 실행: {combo.comboName} - {step + 1}타");
        // 여기에 실제 공격 실행 로직 추가
    }

    private void OnComboCompleted(ComboSequence combo)
    {
        if (isComboTriggered) return;
        isComboTriggered = true;
        stateMachine.ChangeState(new PlayerComboState(stateMachine, combo));
    }

    private void OnComboFailed(ComboSequence combo)
    {
        if (isComboTriggered) return;
        // 콤보 실패 시 LocomotionState로 전환
        stateMachine.ChangeState(new PlayerLocomotionState(stateMachine));
    }

    private void OnNormalAttackExecuted(AttackType attackType)
    {
        if (isComboTriggered) return;
        // 일반 공격 실행
        Debug.Log($"[PlayerAttackState] 일반 공격 실행: {attackType}");
        // 여기에 일반 공격 실행 로직 추가
    }
}
