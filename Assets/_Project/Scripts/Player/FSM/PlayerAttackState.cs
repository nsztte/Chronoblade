using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private PlayerController playerController;
    private WeaponType? cachedWeaponType = null;
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
}
