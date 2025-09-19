using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class SaveTabController : MonoBehaviour
{
    public enum SaveUIMode { LoadOnly, SaveOnly }

    [SerializeField] private GameObject saveLoadPanel;
    [SerializeField] private TMP_Text modeTitleText;
    [SerializeField] private List<SaveSlot> slotList = new();
    [SerializeField] private SaveUIMode currentMode;

    private const int ManualSlotStart = 5;

    private void OnEnable()
    {
        SaveManager.Instance.OnSaved += RefreshSlots;
    }

    private void OnDisable()
    {
        SaveManager.Instance.OnSaved -= RefreshSlots;
    }

    public void Open(SaveUIMode mode)
    {
        saveLoadPanel.SetActive(true);

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
        saveLoadPanel.SetActive(false);
    }

    private void RefreshSlots()
    {
        // 정렬된 데이터 가져오기 — 이제 (slotIndex, meta) 쌍을 받는다.
        var slotData = (currentMode == SaveUIMode.SaveOnly)
            ? GetManualSlotsOnly()
            : GetDisplaySlots();

        for (int i = 0; i < slotList.Count; i++)
        {
            var slot = slotList[i];
            var pair = slotData[i];                 // (int slotIndex, SaveMeta meta)
            int realSlotIndex = pair.slotIndex;
            var meta = pair.meta;

            // 실제 인덱스를 넣어서 초기화 — SaveSlot.Init(index, meta)가 slotIndex를 설정한다.
            slot.Init(realSlotIndex, meta);

            // 클릭 이벤트 설정 — OnClicked의 첫 파라미터는 SaveSlot이 전달하는 실제 슬롯 인덱스임.
            slot.OnClicked += (clickedSlotIndex, m) =>
            {
                if (currentMode == SaveUIMode.LoadOnly)
                {
                    if (m != null)
                        SaveManager.Instance.DefaultLoad(clickedSlotIndex); // 실제 슬롯으로 로드
                }
                else if (currentMode == SaveUIMode.SaveOnly)
                {
                    if (m != null && m.saveType == "Manual")
                    {
                        UIManager.Instance.ShowConfirm("저장하기", "이 슬롯에 덮어쓰시겠습니까?",
                            onConfirm: () => SaveManager.Instance.DefaultSave(clickedSlotIndex, SaveIntent.Manual),
                            onCancel: () => { });
                    }
                    else
                    {
                        SaveManager.Instance.DefaultSave(clickedSlotIndex, SaveIntent.Manual);
                    }
                }
            };

            // 빈 칸이면 비활성 (로드 모드에서 meta == null이면 비활성)
            bool isActive = (currentMode == SaveUIMode.SaveOnly || meta != null);
            slot.SetInteractable(isActive);
        }
    }

    private List<(int slotIndex, SaveManager.SaveMeta meta)> GetDisplaySlots()
    {
        var all = SaveManager.Instance.GetAllMeta(); // List<(slotIndex, meta)>
        var result = new List<(int, SaveManager.SaveMeta)>();

        // Quick은 있으면 맨 앞에
        var quick = all.FirstOrDefault(p => p.meta != null && p.meta.saveType == "Quick");
        if (quick.meta != null)
            result.Add((quick.slotIndex, quick.meta));

        int need = slotList.Count - result.Count;

        var others = all
            .Where(p => p.meta != null && p.meta.saveType != "Quick")
            .OrderByDescending(p =>
            {
                DateTime dt;
                return DateTime.TryParse(p.meta.savedAt, out dt) ? dt : DateTime.MinValue;
            })
            .Take(need)
            .Select(p => (p.slotIndex, p.meta))
            .ToList();

        result.AddRange(others);

        // 부족분 null 패딩: 실제 슬롯 인덱스는 -1로 표시 (버튼 비활성화 됨)
        while (result.Count < slotList.Count)
            result.Add((-1, null));

        return result;
    }

    private List<(int slotIndex, SaveManager.SaveMeta meta)> GetManualSlotsOnly()
    {
        var all = SaveManager.Instance.GetAllMeta();
        var result = new List<(int, SaveManager.SaveMeta)>();

        // Manual 슬롯 고정 인덱스(예: 5~9)
        var manualIndices = Enumerable.Range(ManualSlotStart, slotList.Count); // 5 ~ 9

        foreach (var idx in manualIndices)
        {
            // 파일이 없으면 pair.meta == null. 그래도 실제 인덱스는 idx로 전달
            var pair = all.FirstOrDefault(p => p.slotIndex == idx);
            result.Add((idx, pair.meta));
        }

        return result;
    }
}
