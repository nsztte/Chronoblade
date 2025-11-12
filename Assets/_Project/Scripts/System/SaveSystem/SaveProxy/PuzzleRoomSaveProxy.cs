using System;
using UnityEngine;

[RequireComponent(typeof(PuzzleRoomManager), typeof(SaveId))]
public class PuzzleRoomSaveProxy : SaveableBehaviour
{
    [Serializable]
    private class RoomData
    {
        public int roomId;
        public bool isCleared;
        public bool isActivated;
    }

    private PuzzleRoomManager room;

    private void Awake()
    {
        room = GetComponent<PuzzleRoomManager>();
    }

    public override string CaptureStateJson()
    {
        if (room == null) return null;

        var d = new RoomData
        {
            roomId = room.RoomId,
            isCleared = room.IsCleared,
            isActivated = room.IsActivated
        };
        return JsonUtility.ToJson(d);
    }

    public override void RestoreStateJson(string json)
    {
        if (room == null || string.IsNullOrEmpty(json)) return;

        var d = JsonUtility.FromJson<RoomData>(json);
        if (d == null) return;

        // 동기화
        room.ApplyState(d.isCleared, d.isActivated);
    }
}
