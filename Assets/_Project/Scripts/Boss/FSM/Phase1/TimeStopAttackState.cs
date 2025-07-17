using UnityEngine;

public class TimeStopAttackState : BaseBossState
{
    public TimeStopAttackState(BossController boss, BossStateMachine stateMachine) : base(boss, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("TimeStopAttackState: 타임 스탑 공격 시작");
        boss.PlayAnimation("TimeStop");

        float duration = boss.GetAnimationClipLengthFromState("TimeStop");
        WaitAndChangeToState(duration, new CheckPhase1EndState(boss, stateMachine));
    }
}
