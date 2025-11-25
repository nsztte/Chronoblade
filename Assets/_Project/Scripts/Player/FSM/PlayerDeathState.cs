using UnityEngine;

public class PlayerDeathState : PlayerBaseState
{
    protected override float MovementFactor => 0f;

    public PlayerDeathState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        // 애니메이션 없이 바로 게임 오버 상태로 전환
        GameManager.Instance.EnterGameOver();
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {

    }
}
