using UnityEngine;

public class StaggerCheckState : BaseBossState
{
    public StaggerCheckState(BossController boss, BossStateMachine stateMachine) : base(boss, stateMachine)
    {
        
    }

    public override void Enter()
    {
        Debug.Log("StaggerCheckState 진입");

        boss.PlayAnimation("Stagger");

        WaitAndChangeToState(0.5f, new CheckPhaseEndState(boss, stateMachine));
    }
}
