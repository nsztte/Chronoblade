using System.Collections.Generic;
using UnityEngine;

public class QuickSlotManager : MonoBehaviour
{
    [SerializeField] private List<QuickSlotSlot> quickSlots;
    private ItemData[] slotItems = new ItemData[4];

    #region Singleton
    public static QuickSlotManager Instance { get; private set; }

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
    }
    #endregion

    public void AssignItemToSlot(int index, ItemData item)
    {
        if(index < 0 || index >= slotItems.Length) return;

        slotItems[index] = item;
        RefreshUI();
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

        // 무기 장착 시도
        WeaponManager.Instance.EquipWeaponByItem(item);

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
}
