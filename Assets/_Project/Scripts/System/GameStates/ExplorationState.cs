using UnityEngine;

public class ExplorationState : GameBaseState
{
    public override void Enter()
    {
        Debug.Log("[GameState] ExplorationState Enter");
        UIManager.Instance.ShowHUD();
        TimeManager.Instance.InitializeTimeState();
    }

    public override void Exit()
    {
        UIManager.Instance.HideHUD();
    }
}
