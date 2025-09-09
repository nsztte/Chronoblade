using System;
using UnityEngine;

public class QuickSlotSaveProxy : SaveableBehaviour
{
    [Serializable]
    private class Data
    {
        public string[] slots = new string[4];   // 각 슬롯에 바인딩된 itemId (null 가능)
        public int selectedIndex = -1;           // 선택 슬롯(없으면 -1)
    }

    private QuickSlotManager QuickSlot => QuickSlotManager.Instance;

    public override string CaptureStateJson()
    {
        var d = new Data();

        // 슬롯 바인딩 덤프
        for (int i = 0; i < d.slots.Length; i++)
        {
            var item = QuickSlot.GetSlotItem(i);    // 아이템 데이터 가져오기
            d.slots[i] = item != null ? item.itemID : null; // 아이템 아이디 저장
        }

        // 선택 슬롯 추정: 장착 무기와 일치하는 슬롯 인덱스(무기 슬롯일 때만)
        d.selectedIndex = QuickSlot.GetCurrentWeaponSlotIndex();     // 없으면 -1 반환
        return JsonUtility.ToJson(d);
    }

    public override void RestoreStateJson(string json)
    {
        if (string.IsNullOrEmpty(json)) return;

        var d = JsonUtility.FromJson<Data>(json);

        // 각 슬롯 아이템 복원 (id -> ItemData 역매핑)
        for (int i = 0; i < d.slots.Length; i++)
        {
            var id = d.slots[i];
            var item = string.IsNullOrEmpty(id) ? null : ItemManager.Instance.GetItemByID(id);
            QuickSlot.AssignItemToSlot(i, item); // 내부에서 RefreshUI/Visuals/Highlight 처리
        }
    }
}
