using UnityEngine;

public class FinalPuzzleState : BaseBossState
{
    public FinalPuzzleState(BossController boss, BossStateMachine stateMachine) : base(boss, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("FinalPuzzleState: 파이널퍼즐 시작");
    }
}
