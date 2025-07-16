using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    private PlayerController playerController;
    private Vector3 dashDirection;
    private float dashSpeed = 20f;
    private float dashDuration = 0.25f;
    private float timer;
    public PlayerDashState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        playerController = stateMachine.PlayerController;
    }

    public override void Enter()
    {
        timer = 0f;

        // 플레이어 무적 시간 부여
        PlayerManager.Instance.SetInvincible(true, dashDuration);

        // 애니메이션 트리거
        // PlayerManager.Instance.SetAnimatorTrigger("IsDashing");

        Vector2 input = playerController.LastMoveInput;
        dashDirection = (playerController.transform.forward * input.y + playerController.transform.right * input.x).normalized;
        if(dashDirection == Vector3.zero)
        {
            dashDirection = playerController.transform.forward;
        }

        Debug.Log("DashState 시작");
    }

    public override void Update()
    {
        timer += Time.deltaTime;
        playerController.MoveDirectly(dashDirection * dashSpeed);

        if(timer >= dashDuration)
        {
            stateMachine.ChangeState(new PlayerLocomotionState(stateMachine));
        }
    }

    public override void Exit()
    {
        Debug.Log("DashState 종료");
    }
}
