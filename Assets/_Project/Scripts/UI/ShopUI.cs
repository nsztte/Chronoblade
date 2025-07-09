using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopUI : MonoBehaviour
{
    public GameObject shopPanel;
    public Transform itemListContainer;
    public GameObject itemButtonPrefab;
    public Button buyButton;
    public Button sellButton;
    public TextMeshProUGUI selectedInfoText;

    private ItemData selectedItem;
    private Dictionary<Button, ItemData> buttonItemMap = new ();
    private Button currentSelectedButton;

    private void Start()
    {
        buyButton.onClick.AddListener(OnBuyClicked);
        sellButton.onClick.AddListener(OnSellClicked);

        selectedInfoText.text = "";
    }
    
    public void OpenShopUI(ShopData shopData)
    {
        shopPanel.SetActive(true);
        selectedItem = null;
        selectedInfoText.text = "";
        currentSelectedButton = null;
        buttonItemMap.Clear();

        // 기존 아이템 제거
        foreach(Transform child in itemListContainer)
        {
            Destroy(child.gameObject);
        }

        // 아이템 버튼 생성
        foreach(var item in shopData.items)
        {
            GameObject itemButton = Instantiate(itemButtonPrefab, itemListContainer);
            TextMeshProUGUI itemNameText = itemButton.GetComponentInChildren<TextMeshProUGUI>();
            itemNameText.text = $"{item.name} - {item.price}G";

            Button button = itemButton.GetComponent<Button>();

            buttonItemMap[button] = item;

            button.onClick.AddListener(() => OnItemButtonClicked(button));
        }

        UpdateActionButtons();
    }

    private void OnItemButtonClicked(Button clickedButton)
    {
        if(currentSelectedButton == clickedButton)
        {
            selectedItem = null;
            selectedInfoText.text = "";
            currentSelectedButton = null;
        }
        else
        {
            selectedItem = buttonItemMap[clickedButton];
            currentSelectedButton = clickedButton;
            selectedInfoText.text = $"{selectedItem.name}";
        }

        UpdateButtonHighlight();
        UpdateActionButtons();
    }

    private void UpdateButtonHighlight()
    {
        foreach (var kvp in buttonItemMap)
        {
            var colors = kvp.Key.colors;
            colors.normalColor = (kvp.Key == currentSelectedButton) ? Color.yellow : Color.white;
            kvp.Key.colors = colors;
        }
    }

    private void UpdateActionButtons()
    {
        buyButton.interactable = selectedItem != null;
        sellButton.interactable = selectedItem != null && InventoryManager.Instance.GetItemCount(selectedItem) > 0;
    }

     public void OnBuyClicked()
    {
        if (selectedItem == null) return;
        ShopManager.Instance.BuyItem(selectedItem);
        UpdateActionButtons();
    }

    public void OnSellClicked()
    {
        if (selectedItem == null) return;
        ShopManager.Instance.SellItem(selectedItem);
        UpdateActionButtons();
    }

    public void CloseShopUI()
    {
        shopPanel.SetActive(false);
    }
}
