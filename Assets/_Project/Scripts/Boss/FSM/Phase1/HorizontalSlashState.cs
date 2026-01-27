using UnityEngine;

public class HorizontalSlashState : BaseBossAttackState
{
    public int damage = 15;
    public HorizontalSlashState(BossController boss, BossStateMachine stateMachine) : base(boss, stateMachine, "HorizontalSlash", "HorizontalSlash")
    {
    }

    public override void Enter()
    {
        if (!boss.IsPlayerInAttackRange(boss.Phase1AttackRange))
        {
            stateMachine.ChangeState(new BossDashState(boss, stateMachine, this, dashSpeed: boss.Phase1DashSpeed, stoppingDistance: boss.Phase1DashStopDistance));
            return;
        }

        base.Enter();
    }
}
