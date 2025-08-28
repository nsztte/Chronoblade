using UnityEngine;

public class PausedState : GameBaseState
{
    public override void Enter()
    {
        Debug.Log("[GameState] PausedState Enter");

        if(!UIManager.Instance.IsAnyUIOpen)
            UIManager.Instance.ShowPause();

        TimeManager.Instance.SetTimeScale(0f);
    }

    public override void Exit()
    {
        TimeManager.Instance.SetTimeScale(1f);
        GameManager.Instance.ChangeState(GameManager.Instance.PreviousGameState);
    }
}
