using UnityEngine;
using UnityEngine.EventSystems;

public class InfoTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ItemData item;

    private RectTransform slotRect;

    private void Awake()
    {
        slotRect = GetComponentInParent<InventorySlot>()?.GetComponent<RectTransform>();
    }

    public void SetItem(ItemData itemData)
    {
        item = itemData;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null || item.itemType != ItemType.Equipment) return;
        if (slotRect != null)
            UIManager.Instance.TooltipUI.Show(item, slotRect);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.TooltipUI.Hide();
    }
}
