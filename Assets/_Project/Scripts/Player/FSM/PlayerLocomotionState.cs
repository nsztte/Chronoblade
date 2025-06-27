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
    }

    public override void Exit()
    {
        InputManager.Instance.OnMoveInput -= OnMoveInput;
        InputManager.Instance.OnJumpPressed -= OnJumpPressed;
        InputManager.Instance.OnRunStarted -= OnRunStarted;
        InputManager.Instance.OnRunCanceled -= OnRunCanceled;
        InputManager.Instance.OnCrouchPressed -= OnCrouchPressed;
    }

    public override void Update()
    {
        playerController.LocomotionUpdate(); // 이동 관련 Update (PlayerController에 메서드가 있다고 가정)
    }

    private void OnMoveInput(Vector2 input)
    {
        playerController.SetMoveInput(input);
    }

    private void OnJumpPressed()
    {
        playerController.TryJump();
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
}
