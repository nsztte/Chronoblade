using UnityEngine;

public class MainMenuState : GameBaseState
{
    public override void Enter()
    {
        Debug.Log("[GameState] MainMenuState Enter");
        // UIManager.Instance.UpdateUI(true);
        TitleUIManager.Instance.ShowTitle();
        TimeManager.Instance.SetTimeScale(0f);

        SaveGuard.Instance?.Block(SaveBlockTag.UI);
    }

    public override void Exit()
    {
        TitleUIManager.Instance.HideTitle();
        TitleUIManager.Instance.HideMainMenu();
        // UIManager.Instance.UpdateUI(false);

        SaveGuard.Instance?.Unblock(SaveBlockTag.UI);
    }
}
