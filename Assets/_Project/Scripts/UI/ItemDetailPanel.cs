using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDetailPanel : MonoBehaviour
{
    [SerializeField] private bool isInventoryMode;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI typeTag;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI effectText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button firstButton;
    [SerializeField] private Button secondButton;

    private TextMeshProUGUI firstButtonText;
    private TextMeshProUGUI secondButtonText;

    private void Awake()
    {
        firstButtonText = firstButton.GetComponentInChildren<TextMeshProUGUI>();
        secondButtonText = secondButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Show(ItemData item)
    {
        gameObject.SetActive(true);
        icon.sprite = item.Icon;
        var displayName = item.itemName.Replace("|", "\n");
        nameText.text = displayName;
        typeTag.text = $"[{TypeTagText(item)}]";
        descriptionText.text = item.description;
        effectText.text = ItemEffectText(item);
        priceText.text = $"가격: {item.price:N0} G";

        SetButtonText(item);

        firstButton.gameObject.SetActive(!string.IsNullOrEmpty(firstButtonText.text));
        secondButton.gameObject.SetActive(!string.IsNullOrEmpty(secondButtonText.text));

        BindButtons(item);
    }

    private void BindButtons(ItemData item)
    {
        if(!isInventoryMode) return;

        firstButton.onClick.RemoveAllListeners();
        secondButton.onClick.RemoveAllListeners();

        firstButton.onClick.AddListener(() =>
        {
            ItemManager.Instance.HandleItemAction(item);
            RefreshUI(item);
        });

        secondButton.onClick.AddListener(() =>
        {
            bool dropped = ItemManager.Instance.DropItem(item);
            if (dropped)
                RefreshUI(item);
        });
    }

    private void RefreshUI(ItemData item)
    {
        int count = InventoryManager.Instance.GetItemCount(item);
        if (count <= 0)
            Clear(); // 패널 닫기
        else
            SetButtonText(item); // 버튼 텍스트 업데이트

        if (UIManager.Instance?.InventoryUI != null)
        {
            UIManager.Instance.InventoryUI.UpdateOrAddSlot(item);       // 슬롯 제거/수량 업데이트
            UIManager.Instance.InventoryUI.RefreshEquippedMarkers();    //장착/해제/버리기 후 전체 슬롯의 equippedMarker 갱신
        }
    }

    public void Clear()
    {
        icon.sprite = null;
        nameText.text = "";
        typeTag.text = "";
        descriptionText.text = "";
        effectText.text = "";
        priceText.text = "";
        firstButton.gameObject.SetActive(false);
        secondButton.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void SetButtonText(ItemData item)
    {
        if(!isInventoryMode) return;

        switch (item.itemType)
        {
            case ItemType.Consumable:
                firstButtonText.text = "사용";
                secondButtonText.text = "버리기";
                break;
            case ItemType.Equipment:
                firstButtonText.text = InventoryManager.Instance.IsEquipped(item) ? "해제" : "장착";
                secondButtonText.text = "버리기";
                break;

            case ItemType.Material:
                firstButtonText.text = "사용";
                secondButtonText.text = "버리기";
                break;

            case ItemType.Quest:
                firstButtonText.text = "";
                secondButtonText.text = "";
                break;
        }
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
                    ConsumableItemEffectType.Heal => $"체력을 {item.value}만큼 회복합니다.",
                    ConsumableItemEffectType.ManaRestore => $"마나를 {item.value}만큼 회복합니다.",
                    ConsumableItemEffectType.AmmoSupply => $"탄약을 {item.value}발 보충합니다.",
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
