using UnityEngine;

public class CutsceneState : GameBaseState
{
    public override void Enter()
    {
        Debug.Log("[GameState] CutsceneState Enter");
        UIManager.Instance.ShowCutsceneOverlay();
        TimeManager.Instance.SetTimeScale(0f);
    }

    public override void Exit()
    {
        UIManager.Instance.HideCutsceneOverlay();
        TimeManager.Instance.InitializeTimeState();
    }
}
