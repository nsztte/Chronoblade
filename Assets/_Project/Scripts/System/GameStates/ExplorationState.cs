using UnityEngine;

[CreateAssetMenu(menuName="GameState/Exploration")]
public class ExplorationState : GameBaseState
{
    public override void Enter()
    {
        Debug.Log("[GameState] ExplorationState Enter");
        UIManager.Instance?.UpdatePlayerHud(true);
        TimeManager.Instance.InitializeTimeState();
        UIManager.Instance?.ClearTimeState();
    }

    public override void Exit()
    {
        // UIManager.Instance.HideHUD();
    }
}
