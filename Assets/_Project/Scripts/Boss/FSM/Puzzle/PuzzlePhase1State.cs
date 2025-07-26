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

        // 카메라 연출 시작
        CutsceneCameraManager.Instance.StartPuzzle();
    }

    public override void Exit()
    {
        boss.PuzzleClockManager.OnPuzzleSuccess -= OnPuzzleSuccess;
        boss.PuzzleClockManager.OnPuzzleFail -= OnPuzzleFail;

        GameManager.Instance.EnterCombat();
    }

    private void OnPuzzleSuccess()
    {
        // TODO: 퍼즐 성공 연출
        // 취약점 노출

        // 퍼즐 종료 카메라 이동
        CutsceneCameraManager.Instance.EndPuzzle();

        // 퍼즐 종료 연출
        boss.SetClockPartsTarget(false);
        boss.PuzzleClockManager.LaunchAllClockParts();
        boss.WaitPartsArrival(() => {
            boss.EndPuzzle();
            boss.ExposeWeakPoint(5f, () => ChangeToPhase2());
        });
    }

    private void OnPuzzleFail()
    {
        // TODO: 퍼즐 실패 연출 / 플레이어 패널티 or 보스 체력 회복

        CutsceneCameraManager.Instance.EndPuzzle();
        boss.SetClockPartsTarget(true);
        boss.PuzzleClockManager.LaunchAllClockParts();
        boss.WaitPartsArrival(() => {
            boss.EndPuzzle();
            ChangeToPhase2();
        });
    }

    private void ChangeToPhase2()
    {
        boss.PhaseManager.SetPhase(BossPhase.Phase2);
        stateMachine.ChangeState(new BossIdleState(boss, stateMachine));
    }
}
