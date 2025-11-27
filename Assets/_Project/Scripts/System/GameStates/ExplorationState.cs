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

        // 볼륨 스냅샷 적용
        VolumeSnapshotController.Current?.SetSnapshot(VolumeSnapshotController.Snapshot.Exploration);
    }

    public override void Exit()
    {
        // UIManager.Instance.HideHUD();
    }
}
