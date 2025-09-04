using System.Collections.Generic;
using UnityEngine;

public class QuickSlotManager : MonoBehaviour
{
    [SerializeField] private List<QuickSlotSlot> quickSlots;
    private ItemData[] slotItems = new ItemData[4];

    #region Singleton
    public static QuickSlotManager Instance { get; private set; }

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

    public void BindQuickSlotSlot(List<QuickSlotSlot> slots)
    {
        quickSlots = slots;

        for(int i = 0; i < quickSlots.Count; i++)
        {
            quickSlots[i].SetIndex(i);
        }
    }

    public void AssignItemToSlot(int index, ItemData item)
    {
        if(index < 0 || index >= slotItems.Length) return;

        slotItems[index] = item;
        RefreshUI();
        RefreshAllSlotVisuals();
    }

    public void ActivateSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slotItems.Length) return;

        ItemData item = slotItems[slotIndex];
        if (item == null)
        {
            Debug.LogWarning($"[QuickSlotManager] 슬롯 {slotIndex + 1}에 아이템이 없습니다.");
            return;
        }

        var currentWeapon = WeaponManager.Instance.CurrentWeapon;
        if(item.itemType == ItemType.Equipment && currentWeapon?.ItemData == item)
        {
            WeaponManager.Instance.UnEquipWeapon();
            HighlightSlot(-1);
            return;
        }

        ItemManager.Instance.HandleItemAction(item);

        // UI 강조 표시
        HighlightSlot(slotIndex);
    }

    private void RefreshUI()
    {
        for (int i = 0; i < quickSlots.Count; i++)
        {
            RefreshSlotUI(i);
        }
    }

    private void RefreshSlotUI(int index)
    {
        if (index < 0 || index >= quickSlots.Count) return;

        var item = slotItems[index];
        quickSlots[index].SetItem(item);
    }

    public void ClearSlot(int index)
    {
        if (index < 0 || index >= slotItems.Length) return;

        slotItems[index] = null;
        quickSlots[index].SetItem(null);
    }

    private void HighlightSlot(int index)
    {
        for (int i = 0; i < quickSlots.Count; i++)
        {
            quickSlots[i].SetHighlight(i == index);
        }
    }

    public ItemData GetSlotItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slotItems.Length) return null;
        return slotItems[slotIndex];
    }

    public void RefreshAllSlotVisuals()
    {
        for (int i = 0; i < slotItems.Length; i++)
        {
            if (quickSlots[i] != null)
                quickSlots[i].UpdateVisual();
        }
    }

    public int GetCurrentWeaponSlotIndex()
    {
        var equippedWeapon = WeaponManager.Instance.CurrentWeapon;
        if(equippedWeapon == null) return -1;

        for(int i = 0; i < slotItems.Length; i++)
        {
            if(slotItems[i] == equippedWeapon.ItemData)
                return i;
        }

        return -1;
    }

    public int GetNextWeaponSlotIndex(int currentIndex, int direction)
    {
        int count = slotItems.Length;
        int index = currentIndex;

        for(int i = 0; i < count; i++)
        {
            index = (index + direction + count) % count;

            if(slotItems[index] != null && slotItems[index].itemType == ItemType.Equipment)
                return index;
        }

        return currentIndex;
    }
}
