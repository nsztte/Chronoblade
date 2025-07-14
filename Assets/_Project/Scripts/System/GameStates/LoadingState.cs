using UnityEngine;

public class LoadingState : GameBaseState
{
    public override void Enter()
    {
        Debug.Log("[GameState] LoadingState Enter");
        UIManager.Instance.ShowLoadingScreen();
        TimeManager.Instance.SetTimeScale(0f);
    }

    public override void Exit()
    {
        UIManager.Instance.HideLoadingScreen();
    }
}
