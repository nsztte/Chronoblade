using UnityEngine;

public class CheckPhaseEndState : BaseBossState
{
    public CheckPhaseEndState(BossController boss, BossStateMachine stateMachine) : base(boss, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("CheckPhaseEndState: 페이즈 종료 체크");

        var currentPhase = boss.PhaseManager.CurrentPhase;

        if(currentPhase == BossPhase.Puzzle1)
        {
            stateMachine.ChangeState(new PuzzlePhase1State(boss, stateMachine));
        }
        else if(currentPhase == BossPhase.FinalPuzzle)
        {
            stateMachine.ChangeState(new FinalPuzzleState(boss, stateMachine));
        }
        else
        {
            stateMachine.ChangeState(new BossIdleState(boss, stateMachine));
        }
    }
}
