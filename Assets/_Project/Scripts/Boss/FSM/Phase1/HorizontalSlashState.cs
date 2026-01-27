using UnityEngine;

public class HorizontalSlashState : BaseBossAttackState
{
    public int damage = 15;
    public HorizontalSlashState(BossController boss, BossStateMachine stateMachine) : base(boss, stateMachine, "HorizontalSlash", "HorizontalSlash")
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
