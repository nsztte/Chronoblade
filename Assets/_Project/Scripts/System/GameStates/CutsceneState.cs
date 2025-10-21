using UnityEngine;

[CreateAssetMenu(menuName="GameState/Cutscene")]
public class CutsceneState : GameBaseState
{
    public override void Enter()
    {
        Debug.Log("[GameState] CutsceneState Enter");
        // UIManager.Instance.ShowCutsceneOverlay();

        UIManager.Instance.UpdatePlayerHud(false);
        TimeManager.Instance.SetTimeScale(0f);

        SaveGuard.Instance?.Block(SaveBlockTag.Cutscene);
    }

    public override void Exit()
    {
        Debug.Log("CutsceneState Exit");
        // UIManager.Instance.HideCutsceneOverlay();
        UIManager.Instance.UpdatePlayerHud(true);

        if(TimeManager.Instance.CurrentTimeState == TimeState.Normal)
            TimeManager.Instance.SetTimeScale(1f);
        else
            TimeManager.Instance.InitializeTimeState();

        SaveGuard.Instance?.ClearTag(SaveBlockTag.Cutscene);
        
        // Exploration 전환은 컷씬Manager 쪽에서 일어나므로 여기선 명시하지 않음
    }
}
