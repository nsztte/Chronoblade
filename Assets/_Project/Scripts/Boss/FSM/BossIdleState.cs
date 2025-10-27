using UnityEngine;
using System.Collections;

public class BossIdleState : BaseBossState
{
    public BossIdleState(BossController boss, BossStateMachine stateMachine) : base(boss, stateMachine)
    {
    }

    public override void Enter()
    {
        if(boss.IsEndingTest)
        {
            stateMachine.ChangeState(new BossEndingState(boss, stateMachine));
            return;
        }

        boss.PlayAnimation("Idle");

        if(boss.PhaseManager.CurrentPhase == BossPhase.Phase1)
        {
            boss.StartCoroutine(DecideNextPatternPhase1());
        }
        else if(boss.PhaseManager.CurrentPhase == BossPhase.Phase2)
        {
            boss.StartCoroutine(DecideNextPatternPhase2());
        }
    }

    private IEnumerator DecideNextPatternPhase1()
    {
        yield return new WaitForSeconds(1f);

        if(boss.PhaseManager.CurrentPhase == BossPhase.Puzzle1)
        {
            stateMachine.ChangeState(new PuzzlePhase1State(boss, stateMachine));
            yield break;
        }

        int randomIndex = Random.Range(0, 4);
        switch(randomIndex)
        {
            case 0:
                stateMachine.ChangeState(new TimeStopAttackState(boss, stateMachine));
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
        }
    }

    private IEnumerator DecideNextPatternPhase2()
    {
        yield return new WaitForSeconds(1f);

        if(boss.PhaseManager.CurrentPhase == BossPhase.FinalPuzzle)
        {
            stateMachine.ChangeState(new FinalPuzzleState(boss, stateMachine));
            yield break;
        }

        int randomIndex = Random.Range(0, 4);
        switch(randomIndex)
        {
            case 0:
                stateMachine.ChangeState(new DelayedMineState(boss, stateMachine));
                break;
            case 1:
                stateMachine.ChangeState(new DoubleSlashComboState(boss, stateMachine));
                break;
            case 2:
                stateMachine.ChangeState(new RapidEnergyShotState(boss, stateMachine));
                break;
            case 3:
                stateMachine.ChangeState(new LeapSmashState(boss, stateMachine));
                break;
        }
    }
}
