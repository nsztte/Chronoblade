using UnityEngine;

public class TimeStopAttackState : BaseBossAttackState
{
    private bool hasHandled = false;
    public int damage = 15;
    
    public TimeStopAttackState(BossController boss, BossStateMachine stateMachine) : base(boss, stateMachine, "TimeStop", "TimeStop")
    {
    }

    public override void Enter()
    {
        // Debug.Log("TimeStopAttackState: 타임 스탑 공격 시작");
        if (!boss.IsPlayerInAttackRange())
        {
            stateMachine.ChangeState(new BossDashState(boss, stateMachine, this, dashSpeed: 13f, stoppingDistance: 4f));
            return;
        }

        hasHandled = false;

        base.Enter();

        boss.StartTimeStopEffect();
    }

    public override void Update()
    {
        base.Update();

        if(!isWindingUp && !hasHandled)
        {
            hasHandled = true;
            boss.EndTimeStopEffect();
        }
    }
}
