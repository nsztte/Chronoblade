using UnityEngine;
using System.Collections;

public class BossIdleState : BaseBossState
{
    public BossIdleState(BossController boss, BossStateMachine stateMachine) : base(boss, stateMachine)
    {
    }

    public override void Enter()
    {
        boss.StartCoroutine(DecideNextPattern());
    }

    private IEnumerator DecideNextPattern()
    {
        yield return new WaitForSeconds(1f);

        int randomIndex = Random.Range(0, 5);
        switch(randomIndex)
        {
            case 0:
                stateMachine.ChangeState(new EnergyBoltState(boss, stateMachine));
                break;
            case 1:
                stateMachine.ChangeState(new HorizontalSlashState(boss, stateMachine));
                break;
            case 2:
                stateMachine.ChangeState(new VerticalSmashState(boss, stateMachine));
                break;
            case 3:
                stateMachine.ChangeState(new SpawnSlowZoneState(boss, stateMachine));
                break;
            case 4:
                stateMachine.ChangeState(new TimeStopAttackState(boss, stateMachine));
                break;
        }
    }
}
