using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuickSlotSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private GameObject highlight;

    private int index;
    private ItemData item;

    public void SetItem(ItemData itemData)
    {
        item = itemData;

        if(itemData != null)
        {
            iconImage.sprite = itemData.Icon;
            iconImage.enabled = true;

            iconImage.color = Color.white; 
        }
        else
        {
            iconImage.sprite = null;
            iconImage.enabled = false;

            iconImage.color = new Color(1f, 1f, 1f, 0f); // 투명 처리
        }
    }

    public void SetHighlight(bool isHighlight)
    {
        if(highlight != null)
            highlight.SetActive(isHighlight);
    }

    public ItemData GetItem()
    {
        return item;
    }

    public void SetIndex(int i)
    {
        index = i;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
        var selectedItem = SelectedItemContext.SelectedItem;
        if (selectedItem != null)
        {
            QuickSlotManager.Instance.AssignItemToSlot(index, selectedItem);
        }
    }

    public void UpdateVisual()
    {
        if (item == null) return;

        // 소비형 아이템이고 수량이 0 이하일 경우 회색 처리
        if (item.itemType == ItemType.Consumable && InventoryManager.Instance.GetItemCount(item) <= 0)
        {
            iconImage.color = new Color(0.5f, 0.5f, 0.5f, 0.5f); // 회색+반투명
        }
        else
        {
            iconImage.color = Color.white;
        }
    }
}
