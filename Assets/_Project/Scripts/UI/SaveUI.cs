using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System.Globalization;

public class SaveUI : MonoBehaviour
{
    public enum SaveUIMode { LoadOnly, SaveOnly }

    [SerializeField] private TMP_Text modeTitleText;
    [SerializeField] private List<SaveSlot> slotList = new();
    [SerializeField] private SaveUIMode currentMode;

    private const int ManualSlotStart = 5;

    private void OnEnable()
    {
        for(int i = 0; i < slotList.Count; i++)
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
            ? GetManualSlotsOnly().ToList()
            : GetDisplaySlots();

        for (int i = 0; i < slotList.Count; i++)
        {
            var slot = slotList[i];
            var meta = slotData[i];
            int slotIndex = slot.SlotIndex;

            slot.Init(slotIndex, meta);

            // 3. 클릭 이벤트 설정
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
                        SaveManager.Instance.DefaultSave(i, SaveIntent.Manual);
                    }
                }
            };

            // 4. 활성화 조건 설정 (로드 시 빈 슬롯 비활성)
            bool isActive = (currentMode == SaveUIMode.SaveOnly || meta != null);
            slot.SetInteractable(isActive);
        }
    }

    private List<SaveManager.SaveMeta> GetDisplaySlots()
    {
        var all = SaveManager.Instance.GetAllMeta(); // List<(slotIndex, meta)>
        var result = new List<SaveManager.SaveMeta>();

        var quick = all.FirstOrDefault(p => p.meta != null && p.meta.saveType == "Quick");
        if (quick.meta != null)
            result.Add(quick.meta);

        int need = slotList.Count - result.Count;

        var others = all
            .Where(p => p.meta != null && p.meta.saveType != "Quick")
            .OrderByDescending(p => 
            {
                DateTime dt;
                return DateTime.TryParse(p.meta.savedAt, out dt) ? dt : DateTime.MinValue;
            })
            .Take(need)
            .Select(p => p.meta)
            .ToList();

        result.AddRange(others);

        // 부족분 null 패딩
        while (result.Count < slotList.Count)
            result.Add(null);

        return result;
    }

    private List<SaveManager.SaveMeta> GetManualSlotsOnly()
    {
        var all = SaveManager.Instance.GetAllMeta();
        var result = new List<SaveManager.SaveMeta>();

        var manualIndices = Enumerable.Range(ManualSlotStart, slotList.Count); // 5 ~ 9

        foreach (var idx in manualIndices)
        {
            var pair = all.FirstOrDefault(p => p.slotIndex == idx);
            result.Add(pair.meta); // 파일 없으면 meta == null
        }

        return result;
    }
}
