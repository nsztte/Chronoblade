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

        // Exploration 전환은 EnemyManager 쪽에서 일어나므로 여기선 명시하지 않음
    }
}
