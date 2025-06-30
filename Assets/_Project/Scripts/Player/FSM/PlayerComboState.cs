using UnityEngine;

public class PlayerComboState : PlayerBaseState
{
    private PlayerController playerController;
    private float enterTime;

    public PlayerComboState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        playerController = stateMachine.GetComponent<PlayerController>();
    }

    public override void Enter()
    {
        enterTime = Time.time;
        Debug.Log("PlayerComboState 진입");
    }

    public override void Exit()
    {
        Debug.Log("PlayerComboState 종료");
    }

    public override void Update()
    {
        // 기본 물리 업데이트
        playerController.LocomotionUpdate();
        
        // 임시로 1초 후 LocomotionState로 복귀
        if (Time.time - enterTime > 1f)
        {
            stateMachine.ChangeState(new PlayerLocomotionState(stateMachine));
        }
    }
}
