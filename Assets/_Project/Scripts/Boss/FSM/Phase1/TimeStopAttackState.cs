using UnityEngine;
using System.Collections;

public class TimeStopAttackState : BaseBossState
{
    public TimeStopAttackState(BossController boss, BossStateMachine stateMachine) : base(boss, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("TimeStopAttackState: 타임 스탑 공격 시작");
        boss.PlayAnimation("TimeStop"); 

        boss.StartTimeStopEffect();

        boss.StartCoroutine(HandleTimeStopAttack());
    }

    private IEnumerator HandleTimeStopAttack()
    {
        yield return new WaitForSecondsRealtime(2f);    // 칼 들어올리는 애니메이션 길이에 따라 조정

        boss.EndTimeStopEffect();

        yield return new WaitForSeconds(0.5f);

        stateMachine.ChangeState(new CheckPhase1EndState(boss, stateMachine));
    }
}
