using UnityEngine;

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
        // ComboEvaluator.Instance.OnComboMatched += OnComboMatched; // PlayerAttackState에서만 관리

        // TimingComboManager.Instance.StopBeatRoutine(); // PlayerAttackState에서만 관리
    }

    public override void Exit()
    {
        InputManager.Instance.OnMoveInput -= OnMoveInput;
        InputManager.Instance.OnJumpPressed -= OnJumpPressed;
        InputManager.Instance.OnRunStarted -= OnRunStarted;
        InputManager.Instance.OnRunCanceled -= OnRunCanceled;
        InputManager.Instance.OnCrouchPressed -= OnCrouchPressed;
        InputManager.Instance.OnAttackPressed -= OnAttackPressed;
        // ComboEvaluator.Instance.OnComboMatched -= OnComboMatched; // PlayerAttackState에서만 관리
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

    private void OnAttackPressed()
    {
        if(WeaponManager.Instance.CurrentWeapon == null) return;
        
        var weapon = WeaponManager.Instance.CurrentWeapon;
        if (weapon.weaponData.weaponType == WeaponType.Sword)
        {
            // 소드일 때 스태미너 체크
            if (PlayerManager.Instance.CurrentStamina < 25) // MeleeWeaponController의 staminaCost와 동일
            {
                Debug.Log("스태미너 부족! 공격 불가");
                return;
            }
            
            // 소드일 때만 콤보 시도
            var (result, damageMultiplier, absOffset) = TimingComboManager.Instance.JudgeTiming(Time.time);
            
            if (result != TimingComboManager.TimingResult.Miss)
            {
                if (ComboEvaluator.Instance.CanStartCombo(AttackType.Light))
                {
                    var combo = ComboEvaluator.Instance.GetStartableCombo(AttackType.Light);
                    if (combo != null)
                    {
                        Debug.Log($"[PlayerLocomotionState] 콤보 시작: {combo.comboName} (타이밍: {result})");
                        // 바로 ComboState로 전환
                        stateMachine.ChangeState(new PlayerComboState(stateMachine, combo));
                        return;
                    }
                }
            }
        }
        // Sword가 아니거나 콤보가 아니면 AttackState로
        Debug.Log($"[PlayerLocomotionState] 일반 공격으로 전환");
        stateMachine.ChangeState(new PlayerAttackState(stateMachine));
    }
}
