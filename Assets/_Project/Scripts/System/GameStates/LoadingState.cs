using UnityEngine;

public class LoadingState : GameBaseState
{
    public override void Enter()
    {
        Debug.Log("[GameState] LoadingState Enter");
        UIManager.Instance.ShowLoadingScreen();
        TimeManager.Instance.SetTimeScale(0f);

        // TODO: 실제 로딩 처리
        // TODO: 씬 매니저 연동
        // TODO: 세이브/로드 시스템 연동
    }

    public override void Exit()
    {
        UIManager.Instance.HideLoadingScreen();

        // 게임 저장시에 EnterExploration에서만 저장 가능하도록 구현
        GameManager.Instance.EnterExploration();
    }
}
