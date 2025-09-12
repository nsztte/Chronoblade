using UnityEngine;

public class LoadingState : GameBaseState
{
    public override void Enter()
    {
        Debug.Log("[GameState] LoadingState Enter");
        UIManager.Instance.UpdateUI(false);
        // UIManager.Instance.ShowLoadingScreen();
        TimeManager.Instance.SetTimeScale(0f);

        SaveGuard.Instance?.Block(SaveBlockTag.UI);

        // TODO: 실제 로딩 처리
        // TODO: 씬 매니저 연동
        // TODO: 세이브/로드 시스템 연동
    }

    public override void Exit()
    {
        // UIManager.Instance.HideLoadingScreen();
        UIManager.Instance.UpdateUI(false);

        SaveGuard.Instance?.Unblock(SaveBlockTag.UI);

        GameManager.Instance.EnterExploration();
    }
}
