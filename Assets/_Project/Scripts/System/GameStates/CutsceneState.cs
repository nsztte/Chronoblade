using UnityEngine;

public class CutsceneState : GameBaseState
{
    public override void Enter()
    {
        Debug.Log("[GameState] CutsceneState Enter");
        UIManager.Instance.ShowCutsceneOverlay();
        TimeManager.Instance.SetTimeScale(0f);

        // TODO: 실제 컷씬 재생
        // TODO: 컷씬 매니저 연동
    }

    public override void Exit()
    {
        UIManager.Instance.HideCutsceneOverlay();
        TimeManager.Instance.InitializeTimeState();
        GameManager.Instance.ChangeState(GameManager.Instance.PreviousGameState);
    }
}
