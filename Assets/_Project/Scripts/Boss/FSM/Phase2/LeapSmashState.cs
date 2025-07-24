using UnityEngine;

public class LeapSmashState : BaseBossAttackState
{
    private bool hasAttacked = false;
    public int damage = 2;
    public LeapSmashState(BossController boss, BossStateMachine stateMachine) : base(boss, stateMachine, "LeapSmash", "LeapSmash")
    {
    }

    public override void Enter()
    {
        if(!boss.IsPlayerInAttackRange())
        {
            stateMachine.ChangeState(new BossDashState(boss, stateMachine, this));
            return;
        }

        hasAttacked = false;
        boss.SetInvincibility(true);

        base.Enter();
    }

    public override void Update()
    {
        if(!isWindingUp && !hasAttacked)
        {
            hasAttacked = true;
            boss.SetInvincibility(false);
        }
    }

    public override void Exit()
    {
        boss.SetInvincibility(false);
        base.Exit();
    }
}
