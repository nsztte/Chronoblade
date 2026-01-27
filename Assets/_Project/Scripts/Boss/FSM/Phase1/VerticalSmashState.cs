using UnityEngine;

public class VerticalSmashState : BaseBossAttackState
{
    public int damage = 15;
    public VerticalSmashState(BossController boss, BossStateMachine stateMachine) : base(boss, stateMachine, "VerticalSmash", "VerticalSmash")
    {
    }

    public override void Enter()
    {
        if (!boss.IsPlayerInAttackRange())
        {
            stateMachine.ChangeState(new BossDashState(boss, stateMachine, this, dashSpeed: 13f, stoppingDistance: 4f));
            return;
        }

        base.Enter();
    }
}
