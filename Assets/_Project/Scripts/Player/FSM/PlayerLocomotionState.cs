using UnityEngine;
using System.Collections.Generic;

public class PlayerLocomotionState : PlayerBaseState
{
    private PlayerController playerController;

    public PlayerLocomotionState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        playerController = stateMachine.GetComponent<PlayerController>();
    }

    public override void Enter()
    {
        InputManager.Instance.OnMoveInput += OnMoveInput;
        InputManager.Instance.OnJumpPressed += OnJumpPressed;
        InputManager.Instance.OnRunStarted += OnRunStarted;
        InputManager.Instance.OnRunCanceled += OnRunCanceled;
        InputManager.Instance.OnCrouchPressed += OnCrouchPressed;
        InputManager.Instance.OnAttackPressed += OnAttackPressed;
        InputManager.Instance.OnAttackHeld += OnAttackHeld;
        InputManager.Instance.OnLightAttackPressed += OnLightAttackPressed;
        InputManager.Instance.OnHeavyAttackPressed += OnHeavyAttackPressed;
        InputManager.Instance.OnDashPressed += OnDashPressed;
        InputManager.Instance.OnBlockStarted += OnBlockStarted;
    }

    public override void Exit()
    {
        InputManager.Instance.OnMoveInput -= OnMoveInput;
        InputManager.Instance.OnJumpPressed -= OnJumpPressed;
        InputManager.Instance.OnRunStarted -= OnRunStarted;
        InputManager.Instance.OnRunCanceled -= OnRunCanceled;
        InputManager.Instance.OnCrouchPressed -= OnCrouchPressed;
        InputManager.Instance.OnAttackPressed -= OnAttackPressed;
        InputManager.Instance.OnAttackHeld -= OnAttackHeld;
        InputManager.Instance.OnLightAttackPressed -= OnLightAttackPressed;
        InputManager.Instance.OnHeavyAttackPressed -= OnHeavyAttackPressed;
        InputManager.Instance.OnDashPressed -= OnDashPressed;
        InputManager.Instance.OnBlockStarted -= OnBlockStarted;
    }

    public override void Update()
    {
        playerController.LocomotionUpdate();
    }

    private void OnMoveInput(Vector2 input)
    {
        playerController.SetMoveInput(input);
    }

    private void OnJumpPressed()
    {
        if (playerController.IsGrounded())
        {
            stateMachine.ChangeState(new PlayerJumpState(stateMachine));
        }
    }

    private void OnRunStarted()
    {
        playerController.SetRunning(true);
    }

    private void OnRunCanceled()
    {
        playerController.SetRunning(false);
    }

    private void OnCrouchPressed()
    {
        playerController.ToggleCrouch();
    }

    private void OnDashPressed()
    {
        if (!PlayerManager.Instance.CanDash) return;
        
        if(PlayerManager.Instance.UseStaminaIfAvailable(PlayerManager.Instance.DashStaminaCost))
        {
            stateMachine.ChangeState(new PlayerDashState(stateMachine));
        }
    }

    private void OnBlockStarted()
    {
        var weapon = WeaponManager.Instance.CurrentWeapon;
        if(weapon?.weaponData.weaponType == WeaponType.Sword)
        {
            stateMachine.ChangeState(new PlayerBlockState(stateMachine));
        }
    }

    // 총기류 공격 - 타이밍 판정 없이 즉시 공격
    private void OnAttackPressed()
    {
        if(WeaponManager.Instance.CurrentWeapon == null) return;
        
        var weapon = WeaponManager.Instance.CurrentWeapon;
        if (weapon.weaponData.weaponType != WeaponType.Sword && weapon.weaponData.weaponType != WeaponType.Rifle)
        {
            // 총기류는 즉시 공격 실행
            Debug.Log($"[PlayerLocomotionState] 총기류 공격 실행");
            playerController.PerformWeaponAttack();
        }
    }

    private void OnAttackHeld()
    {
        if(WeaponManager.Instance.CurrentWeapon == null) return;
        
        var weapon = WeaponManager.Instance.CurrentWeapon;
        if(weapon.weaponData.weaponType == WeaponType.Rifle)
        {
            playerController.PerformWeaponAttack();
        }
    }

    // 소드 Light 공격 - 타이밍 판정 후 콤보 또는 일반 공격
    private void OnLightAttackPressed()
    {
        if(WeaponManager.Instance.CurrentWeapon == null) return;
        
        var weapon = WeaponManager.Instance.CurrentWeapon;
        if (weapon.weaponData.weaponType != WeaponType.Sword)
        {
            // 소드가 아니면 무시
            return;
        }
        
        // 소드일 때 스태미너 체크
        if (PlayerManager.Instance.CurrentStamina < PlayerManager.Instance.StaminaCost)
        {
            Debug.Log("스태미너 부족! 공격 불가");
            return;
        }
        
        // 타이밍 판정 (UI/EVENT 미발생 사전 판정)
        var (result, damageMultiplier, absOffset) = TimingComboManager.Instance.JudgeTiming(Time.time, emitEvents: false);
        
        // 비트 루틴이 시작되었고 타이밍이 맞으면 콤보 시도
        if (result != TimingComboManager.TimingResult.Unavailable && result != TimingComboManager.TimingResult.Miss)
        {
            // 첫 입력에 맞는 모든 콤보 후보군을 가져옴
            var candidates = ComboEvaluator.Instance.GetMatchingCombos(new List<AttackType> { AttackType.Light });
            if (candidates.Count > 0)
            {
                Debug.Log($"[PlayerLocomotionState] 콤보 시작: 후보군 {candidates.Count}개 (타이밍: {result})");
                stateMachine.ChangeState(new PlayerComboState(stateMachine, candidates[0]));
                return;
            }
        }
        
        // 타이밍이 맞지 않거나 콤보가 없으면 일반 Light 공격
        Debug.Log($"[PlayerLocomotionState] 일반 Light 공격 실행 (타이밍: {result})");
        playerController.PerformLightAttack();
    }

    // 소드 Heavy 공격 - 타이밍 판정 후 콤보 또는 일반 공격
    private void OnHeavyAttackPressed()
    {
        if(WeaponManager.Instance.CurrentWeapon == null) return;
        
        var weapon = WeaponManager.Instance.CurrentWeapon;
        if (weapon.weaponData.weaponType != WeaponType.Sword)
        {
            // 소드가 아니면 무시
            return;
        }
        
        // 소드일 때 스태미너 체크 (Heavy는 2배 소모)
        if (PlayerManager.Instance.CurrentStamina < PlayerManager.Instance.StaminaCost * 2)
        {
            Debug.Log("스태미너 부족! 공격 불가");
            return;
        }
        
        // 타이밍 판정 (UI/EVENT 미발생 사전 판정)
        var (result, damageMultiplier, absOffset) = TimingComboManager.Instance.JudgeTiming(Time.time, emitEvents: false);
        
        // 비트 루틴이 시작되었고 타이밍이 맞으면 콤보 시도
        if (result != TimingComboManager.TimingResult.Unavailable && result != TimingComboManager.TimingResult.Miss)
        {
            // 첫 입력에 맞는 모든 콤보 후보군을 가져옴
            var candidates = ComboEvaluator.Instance.GetMatchingCombos(new List<AttackType> { AttackType.Heavy });
            if (candidates.Count > 0)
            {
                Debug.Log($"[PlayerLocomotionState] 콤보 시작: 후보군 {candidates.Count}개 (타이밍: {result})");
                stateMachine.ChangeState(new PlayerComboState(stateMachine, candidates[0]));
                return;
            }
        }
        
        // 타이밍이 맞지 않거나 콤보가 없으면 일반 Heavy 공격
        Debug.Log($"[PlayerLocomotionState] 일반 Heavy 공격 실행 (타이밍: {result})");
        playerController.PerformHeavyAttack();
    }
}
