using UnityEngine;
using System.Collections;

public class TimeStopAttackState : BaseBossState
{
    public bool isWindingUp = true;
    private bool hasHandled = false;
    
    public TimeStopAttackState(BossController boss, BossStateMachine stateMachine) : base(boss, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("TimeStopAttackState: 타임 스탑 공격 시작");

        isWindingUp = true;

        boss.PlayAnimation("TimeStop"); 
        boss.StartTimeStopEffect();
    }

    public override void Update()
    {
        float rotationSpeed = isWindingUp ? 12f : 6f;
        boss.LookAtPlayer(rotationSpeed);

        if(!isWindingUp && !hasHandled)
        {
            hasHandled = true;
            boss.StartCoroutine(HandleTimeStopAttack());
        }
    }

    private IEnumerator HandleTimeStopAttack()
    {
        boss.EndTimeStopEffect();

        yield return new WaitForSeconds(2.5f);

        stateMachine.ChangeState(new CheckPhase1EndState(boss, stateMachine));
    }
}
