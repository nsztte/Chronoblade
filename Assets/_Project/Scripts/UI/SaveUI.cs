using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class SaveUI : MonoBehaviour
{
    public enum SaveUIMode { LoadOnly, SaveOnly }

    [SerializeField] private TMP_Text modeTitleText;
    [SerializeField] private List<SaveSlot> slotList = new();
    [SerializeField] private SaveUIMode currentMode;

    private const int QuickSlotIndex  = 1;
    private const int ManualSlotStart = 5;
    private const int ManualSlotCount = 5;

    private void OnEnable()
    {
        for(int i = 0; i < ManualSlotCount; i++)
        {
            if(slotList[i].SlotIndex == 0)
                slotList[i].SetSlotIndex(i + ManualSlotStart);
        }

        RefreshSlots();

        SaveManager.Instance.OnSaved += RefreshSlots;
    }

    private void OnDisable()
    {
        SaveManager.Instance.OnSaved -= RefreshSlots;
    }

    public void Open(SaveUIMode mode)
    {
        gameObject.SetActive(true);

        currentMode = mode;
        modeTitleText.text = mode switch {
            SaveUIMode.LoadOnly => "불러오기",
            SaveUIMode.SaveOnly => "저장하기",
            _ => "저장"
        };

        RefreshSlots();
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

        foreach (var (slotIndex, meta) in slotData)
        {
            var slot = slotList.FirstOrDefault(s => s.SlotIndex == slotIndex);
            if (slot == null)
            {
                Debug.LogWarning($"SlotIndex {slotIndex}에 대응하는 슬롯이 slotList에 없습니다.");
                continue;
            }
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

            // 로드 전용에서는 빈 슬롯 비활성화
            slot.SetInteractable(currentMode == SaveUIMode.SaveOnly || meta != null);
        }
    }

    private List<(int, SaveManager.SaveMeta)> GetDisplaySlots()
    {
        var all = SaveManager.Instance.GetAllMeta(); // List<(slotIndex, meta)>
        var result = new List<(int slotIndex, SaveManager.SaveMeta meta)>();

        // 1) 퀵 슬롯 검사
        var quick = all.FirstOrDefault(p => p.meta != null && p.meta.saveType == "Quick");
        bool hasQuick = quick.meta != null;

        if (hasQuick)
            result.Add(quick);

        // 2) 나머지 최신 정렬 (자동/수동 혼합)
        int need = hasQuick ? slotList.Count - 1 : slotList.Count ;
        var others = all
            .Where(p => p.meta != null && p.meta.saveType != "Quick")
            .OrderByDescending(p =>
            {
                // savedAt 파싱 안전 처리
                DateTime dt;
                return DateTime.TryParse(p.meta.savedAt, out dt) ? dt : DateTime.MinValue;
            })
            .Take(need)
            .ToList();

        result.AddRange(others);

        // 3) 부족하면 (5~9)로 null 패딩
        var manualIndices = Enumerable.Range(ManualSlotStart, ManualSlotCount); // 5..9
        foreach (var idx in manualIndices)
        {
            if (result.Count >= 5) break;
            if (!result.Any(r => r.slotIndex == idx))
                result.Add((idx, null));
        }

        return result;
    }

    private List<(int, SaveManager.SaveMeta)> GetManualSlotsOnly()
    {
        var all = SaveManager.Instance.GetAllMeta();

        var manualIndices = Enumerable.Range(ManualSlotStart, ManualSlotCount);     // 슬롯 5부터 5개

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
