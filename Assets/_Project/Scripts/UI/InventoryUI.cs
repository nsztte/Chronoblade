using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public enum InventoryOpenContext { Standalone, Shop }
public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform slotParent;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private ItemDetailPanel detailPanel;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private int minLeftPadding = 30;
    [SerializeField] private int maxLeftPadding = 90;

    private List<InventorySlot> spawnedSlots = new();
    private InventoryOpenContext context;

    private void OnEnable()
    {
        // InputManager.Instance.OnPause += Close;
        // GameManager.Instance.EnterPaused();
        // UIManager.Instance.SetCursorLockState(CursorLockMode.None);

        UIManager.Instance.ShowOverlayBackground();

        SelectedItemContext.OnSelectedItemChanged += OnSelectedChanged;

        slotParent.GetComponent<GridLayoutGroup>().padding.left = context == InventoryOpenContext.Standalone ? maxLeftPadding : minLeftPadding;
    }

    private void OnDisable()
    {
        // InputManager.Instance.OnPause -= Close;
        // UIManager.Instance.SetCursorLockState(CursorLockMode.Locked);

        UIManager.Instance.HideOverlayBackground();

        SelectedItemContext.OnSelectedItemChanged -= OnSelectedChanged;

        if(detailPanel.gameObject.activeSelf)
            detailPanel.gameObject.SetActive(false);
    }

    public void SetGold(int amount)
    {
        if (goldText == null) return;

        goldText.text = $"{amount} G";
    }

    public void Open(InventoryOpenContext context = InventoryOpenContext.Standalone, ItemDetailPanel overrideDetailPanel = null)
    {
        this.context = context;
        gameObject.SetActive(true);
        Refresh(overrideDetailPanel);

        // 현재 플레이어 골드 표시
        if (PlayerManager.Instance != null)
            SetGold(PlayerManager.Instance.Gold);
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

    public void Close()
    {
        gameObject.SetActive(false);
        SelectedItemContext.Clear();
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
            SelectedItemContext.Set(s.item);
        };

        spawnedSlots.Add(slot);
    }

    private void OnSelectedChanged(ItemData item)
    {
        InventorySlot target = null;
        foreach (var slot in spawnedSlots)
        {
            bool sel = slot.item == item;
            slot.SetSelected(sel);
            if(sel) target = slot;
        }

        if(item == null && detailPanel.gameObject.activeSelf)
            detailPanel.Clear();
    }
}
