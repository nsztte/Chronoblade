using UnityEngine;
using UnityEngine.UI;

public class QuickSlotSlot : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private GameObject highlight;

    private ItemData item;

    public void SetItem(ItemData itemData)
    {
        item = itemData;

        if(itemData != null)
        {
            iconImage.sprite = itemData.Icon;
            iconImage.enabled = true;
        }
        else
        {
            iconImage.sprite = null;
            iconImage.enabled = false;
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
}
