using UnityEngine;

public class VerticalSmashState : BaseBossAttackState
{
    public int damage = 15;
    public VerticalSmashState(BossController boss, BossStateMachine stateMachine) : base(boss, stateMachine, "VerticalSmash", "VerticalSmash")
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
