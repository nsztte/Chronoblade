using UnityEngine;

public class PausedState : GameBaseState
{
    private TimeState previousTimeState;

    public override void Enter()
    {
        Debug.Log("[GameState] PausedState Enter");
        UIManager.Instance.ShowPauseMenu();

        previousTimeState = TimeManager.Instance.CurrentTimeState;
        TimeManager.Instance.SetTimeScale(0f);
    }

    public override void Exit()
    {
        UIManager.Instance.HidePauseMenu();
        TimeManager.Instance.SetTimeState(previousTimeState);
        GameManager.Instance.ChangeState(GameManager.Instance.PreviousGameState);
    }
}
