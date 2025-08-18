using UnityEngine;

public class FinalPuzzleState : BaseBossState
{
    public FinalPuzzleState(BossController boss, BossStateMachine stateMachine) : base(boss, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("FinalPuzzleState: 파이널 퍼즐 시작");

        GameManager.Instance.EnterPuzzle();

        boss.PuzzleClockManager.OnPuzzleSuccess += OnPuzzleSuccess;
        boss.PuzzleClockManager.OnPuzzleFail += OnPuzzleFail;

        boss.StartPuzzle();

        // CutsceneCameraManager.Instance.StartPuzzle();
        CutsceneCameraManager.Instance.StartCutscene(boss.ClockPuzzleCamera);
    }

    public override void Exit()
    {
        boss.PuzzleClockManager.OnPuzzleSuccess -= OnPuzzleSuccess;
        boss.PuzzleClockManager.OnPuzzleFail -= OnPuzzleFail;

        GameManager.Instance.EnterCombat();
    }

    private void OnPuzzleSuccess()
    {
        // TODO: 퍼즐 성공 -> 엔딩
        boss.SetClockPartsTarget(true);
        // CutsceneCameraManager.Instance.EndPuzzle(() => boss.PuzzleClockManager.LaunchAllClockParts());
        CutsceneCameraManager.Instance.EndCutscene(boss.ClockPuzzleCamera, () => boss.PuzzleClockManager.LaunchAllClockParts());
        boss.WaitPartsArrival(() => {
            boss.EndPuzzle();
            boss.SetHPWithPercent(0);
            ChangeToEnding();
        });
    }

    private void OnPuzzleFail()
    {
        // TODO: 퍼즐 실패 -> 보스 체력 20% 회복 -> 페이즈2 재시작
        boss.SetClockPartsTarget(true);
        // CutsceneCameraManager.Instance.EndPuzzle(() => boss.PuzzleClockManager.LaunchAllClockParts());
        CutsceneCameraManager.Instance.EndCutscene(boss.ClockPuzzleCamera, () => boss.PuzzleClockManager.LaunchAllClockParts());
        boss.WaitPartsArrival(() => {
            boss.EndPuzzle();
            boss.SetHPWithPercent(20);
            ChangeToPhase2();
        });
    }

    private void ChangeToPhase2()
    {
        boss.PhaseManager.SetPhase(BossPhase.Phase2);
        stateMachine.ChangeState(new BossIdleState(boss, stateMachine));
    }

    private void ChangeToEnding()
    {
        boss.PhaseManager.SetPhase(BossPhase.Ending);
        stateMachine.ChangeState(new BossEndingState(boss, stateMachine));
    }
}
