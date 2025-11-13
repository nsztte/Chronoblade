using System;
using UnityEngine;

[RequireComponent(typeof(BossAltar)), RequireComponent(typeof(SaveId))]
public class BossAltarSaveProxy : SaveableBehaviour
{
    [Serializable]
    private class AltarData
    {
        public int insertedKeyCount;
        public bool[] slotInserted;
    }

    private BossAltar altar;

    private void Awake()
    {
        altar = GetComponent<BossAltar>();
    }

    public override string CaptureStateJson()
    {
        if (altar == null) return null;

        int n = altar.SlotCount;
        var d = new AltarData
        {
            insertedKeyCount = altar.InsertedKeyCount,
            slotInserted = new bool[n]
        };
        altar.GetSlotInsertedSnapshot(d.slotInserted);
        return JsonUtility.ToJson(d);
    }

    public override void RestoreStateJson(string json)
    {
        if (altar == null || string.IsNullOrEmpty(json)) return;

        var d = JsonUtility.FromJson<AltarData>(json);
        if (d == null) return;

        // 복원 (이벤트/컷씬 발행 금지)
        altar.ApplyState(d.insertedKeyCount, d.slotInserted);
    }
}
