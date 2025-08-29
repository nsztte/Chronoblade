using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public enum InventoryOpenContext { Standalone, Shop }
public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform slotParent;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private ItemDetailPanel detailPanel;
    [SerializeField] private int minLeftPadding = 30;
    [SerializeField] private int maxLeftPadding = 90;

    private List<InventorySlot> spawnedSlots = new();
    private InventoryOpenContext context;

    private void OnEnable()
    {
        InputManager.Instance.OnPause += Close;
        GameManager.Instance.EnterPaused();
        UIManager.Instance.SetCursorLockState(CursorLockMode.None);

        slotParent.GetComponent<GridLayoutGroup>().padding.left = context == InventoryOpenContext.Standalone ? maxLeftPadding : minLeftPadding;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnPause -= Close;
        UIManager.Instance.SetCursorLockState(CursorLockMode.Locked);

        if(detailPanel.gameObject.activeSelf)
            detailPanel.gameObject.SetActive(false);
    }

    public void Open(InventoryOpenContext context = InventoryOpenContext.Standalone, ItemDetailPanel overrideDetailPanel = null)
    {
        this.context = context;
        gameObject.SetActive(true);
        Refresh(overrideDetailPanel);
    }

    public void UpdateOrAddSlot(ItemData item, ItemDetailPanel overrideDetailPanel = null)
    {
        var existingSlot = spawnedSlots.FirstOrDefault(s => s.item == item);
        if (existingSlot != null)
        {
            // 아이템 수량이 0이면 제거
            if (InventoryManager.Instance.GetItemCount(item) <= 0)
            {
                Destroy(existingSlot.gameObject);
                spawnedSlots.Remove(existingSlot);
            }
            else
            {
                existingSlot.Set(item); // count 업데이트
            }
        }
        else
        {
            AddSlot(item, overrideDetailPanel);
        }
    }


    private void Refresh(ItemDetailPanel overrideDetailPanel = null)
    {
        ClearSlots();

        // 아이템 목록 가져오기
        foreach (var kvp in InventoryManager.Instance.GetAllItems())
        {
            var itemData = kvp.Key;
            AddSlot(itemData, overrideDetailPanel);
        }
    }

    private void ClearSlots()
    {
        foreach (var slot in spawnedSlots)
        {
            Destroy(slot.gameObject);
        }
        spawnedSlots.Clear();
    }

    private void SetSelectedSlot(InventorySlot selected)
    {
        foreach (var slot in spawnedSlots)
        {
            slot.SetSelected(slot == selected);
        }
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }

    private void AddSlot(ItemData itemData, ItemDetailPanel overrideDetailPanel = null)
    {
        GameObject go = Instantiate(slotPrefab, slotParent);
        var slot = go.GetComponent<InventorySlot>();
        slot.Set(itemData);
        slot.Bind();

        var detail = context == InventoryOpenContext.Standalone
            ? detailPanel
            : overrideDetailPanel;

        // 선택 시 디테일 패널 연동
        slot.onClick = (s) =>
        {
            detail.Show(s.item);
            SetSelectedSlot(s);
        };

        spawnedSlots.Add(slot);
    }
}
