using UnityEngine;

public class CheckPhase1EndState : BaseBossState
{
    public CheckPhase1EndState(BossController boss, BossStateMachine stateMachine) : base(boss, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("CheckPhase1EndState: 페이즈1 종료 체크");

        var currentPhase = boss.PhaseManager.CurrentPhase;

        if(currentPhase == BossPhase.Puzzle1)
        {
            stateMachine.ChangeState(new PuzzlePhase1State(boss, stateMachine));
        }
        else
        {
            stateMachine.ChangeState(new BossIdleState(boss, stateMachine));
        }
    }
}
