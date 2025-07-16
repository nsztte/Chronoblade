using UnityEngine;

public class PlayerBlockState : PlayerBaseState
{
    private PlayerController playerController;

    public PlayerBlockState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        playerController = stateMachine.PlayerController;
    }

    public override void Enter()
    {
        // PlayerManager.Instance.SetAnimatorBool("IsBlocking", true);
        PlayerManager.Instance.IsBlocking = true;
        InputManager.Instance.OnBlockCanceled += OnBlockCanceled;

        Debug.Log("BlockState 시작");
    }

    public override void Exit()
    {
        // PlayerManager.Instance.SetAnimatorBool("IsBlocking", false);
        PlayerManager.Instance.IsBlocking = false;
        InputManager.Instance.OnBlockCanceled -= OnBlockCanceled;
        PlayerManager.Instance.LastBlockEndTime = Time.time;

        Debug.Log("BlockState 종료");
    }

    public override void Update()
    {
        playerController.LocomotionUpdate();
    }

    private void OnBlockCanceled()
    {
        stateMachine.ChangeState(new PlayerLocomotionState(stateMachine));
    }
}