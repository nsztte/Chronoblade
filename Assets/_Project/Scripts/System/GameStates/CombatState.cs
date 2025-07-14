using UnityEngine;

public class CombatState : GameBaseState
{
    public override void Enter()
    {
        Debug.Log("[GameState] CombatState Enter");
        UIManager.Instance.ShowCombatHUD();
        TimeManager.Instance.InitializeTimeState();
        TimingComboManager.Instance.StartBeatRoutine();
    }

    public override void Exit()
    {
        UIManager.Instance.HideCombatHUD();
        TimingComboManager.Instance.StopBeatRoutine();
    }
}
