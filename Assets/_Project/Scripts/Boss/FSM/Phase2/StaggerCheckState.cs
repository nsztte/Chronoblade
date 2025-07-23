using UnityEngine;

public class StaggerCheckState : BaseBossState
{
    public StaggerCheckState(BossController boss, BossStateMachine stateMachine) : base(boss, stateMachine)
    {
        
    }

    public override void Enter()
    {
        if(boss.IsPlayerInAttackRange())
        {
            stateMachine.ChangeState(new BossDashState(boss, stateMachine, this));
            return;
        }
    }
}
