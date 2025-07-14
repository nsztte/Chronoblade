using UnityEngine;

public class LoadingState : GameBaseState
{
    public override void Enter()
    {
        Debug.Log("[GameState] LoadingState Enter");
        UIManager.Instance.ShowLoadingScreen();
        TimeManager.Instance.SetTimeScale(0f);

        // TODO: 게임데이터 로딩
    }

    public override void Exit()
    {
        UIManager.Instance.HideLoadingScreen();

        // 게임 저장시에 EnterExploration에서만 저장 가능하도록 구현
        GameManager.Instance.EnterExploration();
    }
}
