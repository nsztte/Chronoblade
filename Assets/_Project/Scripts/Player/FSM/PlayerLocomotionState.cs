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
        ComboEvaluator.Instance.OnComboMatched += OnComboMatched;

        TimingComboManager.Instance.StopBeatRoutine();
    }

    public override void Exit()
    {
        InputManager.Instance.OnMoveInput -= OnMoveInput;
        InputManager.Instance.OnJumpPressed -= OnJumpPressed;
        InputManager.Instance.OnRunStarted -= OnRunStarted;
        InputManager.Instance.OnRunCanceled -= OnRunCanceled;
        InputManager.Instance.OnCrouchPressed -= OnCrouchPressed;
        InputManager.Instance.OnAttackPressed -= OnAttackPressed;
        ComboEvaluator.Instance.OnComboMatched -= OnComboMatched;
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
        
        // LocomotionState: 기본 공격 시작 (AttackState로 전환)
        stateMachine.ChangeState(new PlayerAttackState(stateMachine));
    }

    private void OnComboMatched(ComboSequence combo)
    {
        stateMachine.ChangeState(new PlayerComboState(stateMachine, combo));
    }
}
