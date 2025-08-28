using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDetailPanel : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI typeTag;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI effectText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button sellButton;

    public void Show(ItemData item)
    {
        gameObject.SetActive(true);
        icon.sprite = item.icon;
        nameText.text = item.itemName;
        typeTag.text = $"[{TypeTagText(item)}]";
        descriptionText.text = item.description;
        effectText.text = ItemEffectText(item);
        priceText.text = $"가격: {item.price:N0} G";
        buyButton.gameObject.SetActive(true);
        sellButton.gameObject.SetActive(true);
    }

    public void Clear()
    {
        icon.sprite = null;
        nameText.text = "";
        typeTag.text = "";
        descriptionText.text = "";
        effectText.text = "";
        priceText.text = "";
        buyButton.gameObject.SetActive(false);
        sellButton.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private string TypeTagText(ItemData item)
    {
        string typeText = "";
        switch (item.itemType)
        {
            case ItemType.Consumable:
                typeText = "소모형";
                break;
            case ItemType.Equipment:
                typeText = "장비형";
                break;

            case ItemType.Material:
                typeText = "제작 재료";
                break;

            case ItemType.Quest:
                typeText = "퀘스트";
                break;
        }

        return typeText;
    }

    private string ItemEffectText(ItemData item)
    {
        string effectText = "";
        switch (item.itemType)
        {
            case ItemType.Consumable:
                effectText = item.consumableItemEffectType switch
                {
                    ConsumableItemEffectType.Heal => $"체력을 {item.value} 회복합니다.",
                    ConsumableItemEffectType.ManaRestore => $"마나를 {item.value} 회복합니다.",
                    ConsumableItemEffectType.AmmoSupply => $"탄약을 {item.value} 보충합니다.",
                    _ => ""
                };
                break;
            case ItemType.Equipment:
                effectText = "장비 아이템입니다.";
                break;

            case ItemType.Material:
                effectText = "제작 재료로 사용됩니다.";
                break;

            case ItemType.Quest:
                effectText = "퀘스트용 아이템입니다.";
                break;
        }

        return effectText;
    }
}
