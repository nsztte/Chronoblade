using UnityEngine;

[CreateAssetMenu(menuName="GameState/Combat")]
public class CombatState : GameBaseState
{
    public override void Enter()
    {
        Debug.Log("[GameState] CombatState Enter");
        TimeManager.Instance.InitializeTimeState();
        TimingComboManager.Instance.StartBeatRoutine();

        SaveGuard.Instance?.Block(SaveBlockTag.Combat);

        // 볼륨 스냅샷 적용
        VolumeSnapshotController.Current?.SetSnapshot(VolumeSnapshotController.Snapshot.Combat);
    }

    public override void Exit()
    {
        TimingComboManager.Instance.StopBeatRoutine();

        SaveGuard.Instance?.ClearTag(SaveBlockTag.Combat);

        // Exploration 전환은 EnemyManager 쪽에서 일어나므로 여기선 명시하지 않음
    }
}
