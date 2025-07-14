using UnityEngine;

public class GameOverState : GameBaseState
{
    public override void Enter()
    {
        Debug.Log("[GameState] GameOverState Enter");
        UIManager.Instance.ShowGameOverScreen();
        TimeManager.Instance.SetTimeScale(0f);
    }

    public override void Exit()
    {
        UIManager.Instance.HideGameOverScreen();
    }
}
