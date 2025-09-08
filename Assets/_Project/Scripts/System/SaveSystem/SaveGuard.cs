using System;
using System.Collections.Generic;
using UnityEngine;

public enum SaveBlockTag
{
    Default,
    Puzzle,
    Cutscene,
    Boss
}

public class SaveGuard : MonoBehaviour
{
    #region Singleton
    public static SaveGuard Instance { get; private set; }

    public void Initialize()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning($"{GetType().Name} 인스턴스 감지됨, 초기화 스킵");
            return;
        }
        Instance = this;
    }
    #endregion

    public bool CanSave => totalBlocks == 0;

    public event Action<bool> OnCanSaveChanged; // true = 저장 가능, false = 불가

    private Dictionary<SaveBlockTag, int> counts = new Dictionary<SaveBlockTag, int>();
    private int totalBlocks = 0;

    public void Block(SaveBlockTag tag = SaveBlockTag.Default)
    {
        if (counts.TryGetValue(tag, out var c)) counts[tag] = c + 1;
        else counts[tag] = 1;

        totalBlocks++;
        if (totalBlocks == 1)
            OnCanSaveChanged?.Invoke(false);
    }

    public void Unblock(SaveBlockTag tag = SaveBlockTag.Default)
    {
        if (!counts.TryGetValue(tag, out var c)) return;

        c--;
        if (c <= 0) counts.Remove(tag);
        else counts[tag] = c;

        totalBlocks = Mathf.Max(0, totalBlocks - 1);
        if (totalBlocks == 0)
            OnCanSaveChanged?.Invoke(true);
    }

    public void ClearTag(SaveBlockTag tag = SaveBlockTag.Default)
    {
        if (!counts.TryGetValue(tag, out var c)) return;
        counts.Remove(tag);
        totalBlocks = Mathf.Max(0, totalBlocks - c);
        if (totalBlocks == 0)
            OnCanSaveChanged?.Invoke(true);
    }

    public void ClearAll()
    {
        bool wasBlocked = totalBlocks > 0;
        counts.Clear();
        totalBlocks = 0;
        if (wasBlocked)
            OnCanSaveChanged?.Invoke(true);
    }
}
