using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PuzzleProgressManager), typeof(SaveId))]
public class PuzzleProgressManagerSaveProxy : SaveableBehaviour
{

    [Serializable]
    private class PPMData
    {
        public int[] clearedRooms;
        public int[] unlockedRooms;
        public int lastClearedRoomId;
        public int keyCount;
        public int maxCount;
        public bool allClearedRaised;
    }

    private PuzzleProgressManager ppm;

    private void Awake()
    {
        ppm = GetComponent<PuzzleProgressManager>();
    }

    public override string CaptureStateJson()
    {
        if (ppm == null) return null;

        var d = new PPMData
        {
            clearedRooms = ppm.ClearedRooms is null ? Array.Empty<int>() : ToArray(ppm.ClearedRooms),
            unlockedRooms = ppm.UnlockedRooms is null ? Array.Empty<int>() : ToArray(ppm.UnlockedRooms),
            lastClearedRoomId = ppm.LastClearedRoomId,
            keyCount = ppm.KeyCount,
            maxCount = ppm.MaxCount,
            allClearedRaised = ppm.AllClearedRaised
        };
        return JsonUtility.ToJson(d);
    }

    public override void RestoreStateJson(string json)
    {
        if (ppm == null || string.IsNullOrEmpty(json)) return;

        var d = JsonUtility.FromJson<PPMData>(json);
        if (d == null) return;

        // 이벤트 억제 모드로 상태만 복원
        ppm.ApplyDataOnly(
            d.clearedRooms ?? Array.Empty<int>(),
            d.unlockedRooms ?? Array.Empty<int>(),
            d.lastClearedRoomId,
            d.keyCount,
            d.maxCount,
            d.allClearedRaised
        );
    }

    private int[] ToArray(IReadOnlyCollection<int> src)
    {
        var arr = new int[src.Count];
        int i = 0;
        foreach (var v in src) arr[i++] = v;
        return arr;
    }
}
