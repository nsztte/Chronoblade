using UnityEngine;

public class MainMenuState : GameBaseState
{
    public override void Enter()
    {
        Debug.Log("[GameState] MainMenuState Enter");
        UIManager.Instance.ShowMainMenu();
        TimeManager.Instance.SetTimeScale(0f);
    }

    public override void Exit()
    {
        UIManager.Instance.HideMainMenu();
    }
}
