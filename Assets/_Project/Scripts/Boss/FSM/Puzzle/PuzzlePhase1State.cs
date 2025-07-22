using UnityEngine;

public class PuzzlePhase1State : BaseBossState
{
    public PuzzlePhase1State(BossController boss, BossStateMachine stateMachine) : base(boss, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("PuzzlePhase1State: 퍼즐 페이즈1 시작");

        GameManager.Instance.EnterPuzzle();

        boss.PuzzleClockManager.OnPuzzleSuccess += OnPuzzleSuccess;
        boss.PuzzleClockManager.OnPuzzleFail += OnPuzzleFail;

        // TODO: 보스 주술 애니메이션 트리거

        // 퍼즐 시작
        boss.StartPuzzle();
    }

    public override void Exit()
    {
        boss.PuzzleClockManager.OnPuzzleSuccess -= OnPuzzleSuccess;
        boss.PuzzleClockManager.OnPuzzleFail -= OnPuzzleFail;

        boss.EndPuzzle();
    }

    private void OnPuzzleSuccess()
    {
        // TODO: 퍼즐 성공 연출

        // 취약점 노출
        boss.ExposeWeakPoint(5f, () => ChangeToPhase2());
    }

    private void OnPuzzleFail()
    {
        // TODO: 퍼즐 실패 연출 / 플레이어 패널티 or 보스 체력 회복

        ChangeToPhase2();
    }

    private void ChangeToPhase2()
    {
        boss.PhaseManager.SetPhase(BossPhase.Phase2);
        GameManager.Instance.EnterCombat();
        stateMachine.ChangeState(new BossIdleState(boss, stateMachine));
    }
}
