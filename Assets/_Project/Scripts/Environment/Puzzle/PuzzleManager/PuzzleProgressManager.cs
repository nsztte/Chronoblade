using System;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleProgressManager : MonoBehaviour
{
    public event Action<int> OnRoomUnlocked;
    public event Action<int> OnRoomCleared;
    public event Action<int> OnKeyInserted;

    private readonly HashSet<int> unlocked = new();
    private readonly HashSet<int> cleared  = new();
    private int keyCount = 0;

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

        Unlock(3);
    }
    #endregion

    public bool IsUnlocked(int roomId) => unlocked.Contains(roomId);
    public bool IsCleared (int roomId) => cleared.Contains(roomId);
    public int GetKeyCount() => keyCount;

    public void MarkCleared(int roomId)
    {
        if (!cleared.Add(roomId)) return;
        OnRoomCleared?.Invoke(roomId);
        if (unlockMap.TryGetValue(roomId, out var next)) Unlock(next);
    }

    public void ReportKeyInserted(int total)
    {
        keyCount = total;
        OnKeyInserted?.Invoke(keyCount);
    }

    private void Unlock(int roomId)
    {
        if (!unlocked.Add(roomId)) return;
        OnRoomUnlocked?.Invoke(roomId);
    }
}
