using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class SaveUI : MonoBehaviour
{
    public enum SaveUIMode { LoadOnly, SaveOnly }

    [SerializeField] private int slotsCount = 4;
    [SerializeField] private Transform slotParent;
    [SerializeField] private GameObject saveSlotPrefab;
    [SerializeField] private TMP_Text modeTitleText;

    private List<SaveSlot> slotInstances = new();
    private SaveUIMode currentMode;

    private void OnEnable()
    {
        RefreshSlots();
    }

    public void Open(SaveUIMode mode)
    {
        currentMode = mode;
        modeTitleText.text = mode switch {
            SaveUIMode.LoadOnly => "불러오기",
            SaveUIMode.SaveOnly => "저장하기",
            _ => "저장"
        };

        RefreshSlots();
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    private void RefreshSlots()
    {
        // 정렬된 데이터 가져오기
        var slotData = (currentMode == SaveUIMode.SaveOnly)
            ? GetManualSlotsOnly()
            : GetDisplaySlots();

        // 기존 슬롯 파괴 및 재생성
        foreach (var old in slotInstances)
            Destroy(old.gameObject);
        slotInstances.Clear();

        foreach (var (slotIndex, meta) in slotData)
        {
            var go = Instantiate(saveSlotPrefab, slotParent);
            var slot = go.GetComponent<SaveSlot>();
            slot.Init(slotIndex, meta);

            // 모드에 따른 동작 결정
            slot.OnClicked += (i, m) =>
            {
                if (currentMode == SaveUIMode.LoadOnly)
                {
                    if (m != null)
                        SaveManager.Instance.DefaultLoad(i); // confirm 없이 바로
                }
                else if (currentMode == SaveUIMode.SaveOnly)
                {
                    if (m != null && m.saveType == "Manual")
                    {
                        UIManager.Instance.ShowConfirm("저장하기", "이 슬롯에 덮어쓰시겠습니까?",
                            onConfirm: () => SaveManager.Instance.DefaultSave(i, SaveIntent.Manual),
                            onCancel: () => { });
                    }
                    else
                    {
                        // 빈 슬롯 또는 다른 타입 (Manual이 아닌 경우) → 바로 저장
                        SaveManager.Instance.DefaultSave(i, SaveIntent.Manual);
                    }
                }
            };

            // 로드 전용에서 빈 슬롯 비활성
            if (currentMode == SaveUIMode.LoadOnly)
                slot.SetInteractable(meta != null);
            else
                slot.SetInteractable(true);

            slotInstances.Add(slot);
        }
    }

    private List<(int, SaveManager.SaveMeta)> GetDisplaySlots()
    {
        var allSlots = SaveManager.Instance.GetAllMeta();

        var quick = allSlots.FirstOrDefault(s => s.Item2.saveType == "Quick");
        var others = allSlots
            .Where(s => s.Item2.saveType != "Quick")                    // 퀵 제외한
            .OrderByDescending(s => DateTime.Parse(s.Item2.savedAt))    // 저장시점 내림차순으로
            .Take(slotsCount)                                           // 앞에서부터 n개까지만
            .ToList();

        var result = new List<(int, SaveManager.SaveMeta)>();
        if (!quick.Equals(default)) result.Add(quick);
        result.AddRange(others);
        return result;
    }

    private List<(int, SaveManager.SaveMeta)> GetManualSlotsOnly()
    {
        var all = SaveManager.Instance.GetAllMeta();

        var manualIndices = Enumerable.Range(5, 5);

        var result = new List<(int, SaveManager.SaveMeta)>();
        foreach (var idx in manualIndices)
        {
            var pair = all.FirstOrDefault(p => p.slotIndex == idx);
            // 해당 슬롯에 파일이 없으면 meta=null로 채움
            if (pair.slotIndex == 0) result.Add((idx, null));
            else result.Add(pair);
        }

        return result;
    }
}
