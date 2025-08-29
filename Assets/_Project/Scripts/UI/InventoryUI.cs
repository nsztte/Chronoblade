using UnityEngine;
using System.Collections.Generic;

public enum InventoryOpenContext { Standalone, Shop }
public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform slotParent;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private ItemDetailPanel detailPanel;

    private List<InventorySlot> spawnedSlots = new();

    private void OnEnable()
    {
        InputManager.Instance.OnPause += Close;
        GameManager.Instance.EnterPaused();
        UIManager.Instance.SetCursorLockState(CursorLockMode.None);

        // Refresh();
    }

    private void OnDisable()
    {
        InputManager.Instance.OnPause -= Close;
        UIManager.Instance.SetCursorLockState(CursorLockMode.Locked);

        if(detailPanel.gameObject.activeSelf)
            detailPanel.gameObject.SetActive(false);
    }

    public void Open(ItemDetailPanel overrideDetailPanel = null)
    {
        gameObject.SetActive(true);
        Refresh(overrideDetailPanel);
    }

    private void Refresh(ItemDetailPanel overrideDetailPanel = null)
    {
        ClearSlots();

        // 아이템 목록 가져오기
        foreach (var kvp in InventoryManager.Instance.GetAllItems())
        {
            var itemData = kvp.Key;
            var count = kvp.Value;

            GameObject go = Instantiate(slotPrefab, slotParent);
            var slot = go.GetComponent<InventorySlot>();
            slot.Set(itemData);
            slot.Bind();

            // 선택 시 디테일 패널 연동
            slot.onClick = (s) =>
            {
                if (overrideDetailPanel != null)
                    overrideDetailPanel.Show(s.item);
                else
                    ShowItemDetail(s.item);

                SetSelectedSlot(s);
            };

            spawnedSlots.Add(slot);
        }
    }

    private void ShowItemDetail(ItemData data)
    {
        detailPanel.Show(data);
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
}
