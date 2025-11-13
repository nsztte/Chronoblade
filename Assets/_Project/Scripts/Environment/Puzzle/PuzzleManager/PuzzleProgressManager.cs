using System;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleProgressManager : MonoBehaviour
{
    // 테스트용
    [SerializeField] private bool puzzle3Cleared = false;
    [SerializeField] private bool puzzle4Cleared = false;
    [SerializeField] private bool puzzle5Cleared = false;
    [SerializeField] private bool puzzle6Cleared = false;

    public event Action<int> OnRoomUnlocked;
    public event Action<int> OnRoomCleared;
    public event Action<int> OnKeyInserted;
    public event Action OnAllCleared;

    private readonly HashSet<int> unlocked = new();
    private readonly HashSet<int> cleared  = new();
    private int keyCount = 0;
    private int maxCount = 0;
    private int lastClearedRoomId = -1;
    private bool allClearedRaised = false;
    // private bool suppressEvents = false;

    #region Getter
    public IReadOnlyCollection<int> ClearedRooms => cleared;
    public IReadOnlyCollection<int> UnlockedRooms => unlocked;
    public int LastClearedRoomId => lastClearedRoomId;
    public int KeyCount => keyCount;
    public int MaxCount => maxCount;
    public bool AllClearedRaised => allClearedRaised;
    #endregion

    private static readonly Dictionary<int, int> unlockMap = new()
    {
        {3, 5}, {5, 4}, {4, 6}
    };

    #region Singleton
    public static PuzzleProgressManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // suppressEvents = true;
        Unlock(3);
        // suppressEvents = false;
    }
    #endregion

    private void Update()
    {
        if(puzzle3Cleared)
            MarkCleared(3);

        if(puzzle5Cleared)
            MarkCleared(5);

        if(puzzle4Cleared) 
            MarkCleared(4);

        if(puzzle6Cleared)
            MarkCleared(6);

        if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            foreach(var c in cleared)
                Debug.Log(c);
        }
    }

    public bool IsUnlocked(int roomId) => unlocked.Contains(roomId);
    public bool IsCleared (int roomId) => cleared.Contains(roomId);
    public int GetKeyCount() => keyCount;

    public void MarkCleared(int roomId)
    {
        if (!cleared.Add(roomId)) return;
        OnRoomCleared?.Invoke(roomId);
        lastClearedRoomId = roomId;
        if (unlockMap.TryGetValue(roomId, out var next)) Unlock(next);
    }

    public void ReportKeyInserted(int total, int max)
    {
        keyCount = total;
        maxCount = max;

        if(!allClearedRaised && keyCount == maxCount)
        {
            allClearedRaised = true;
            OnAllCleared?.Invoke();
        }
        else
        {
            if (unlockMap.TryGetValue(lastClearedRoomId, out var nextRoomId))
                OnKeyInserted?.Invoke(nextRoomId);
        }
    }

    private void Unlock(int roomId)
    {
        if (!unlocked.Add(roomId)) return;
        OnRoomUnlocked?.Invoke(roomId);
    }

    // 저장/복원 전용 API
    public void ApplyData(
    IEnumerable<int> clearedIn,
    IEnumerable<int> unlockedIn,
    int lastClearedId,
    int keyCnt,
    int maxCnt,
    bool allCleared)
    {
        // 상태 초기화
        unlocked.Clear();
        cleared.Clear();

        // 값 주입
        if (unlockedIn != null) foreach (var r in unlockedIn) Unlock(r);
        if (clearedIn  != null) foreach (var r in clearedIn) cleared.Add(r);

        lastClearedRoomId = lastClearedId;
        keyCount = keyCnt;
        maxCount = maxCnt;
        allClearedRaised = allCleared;
    }
}
