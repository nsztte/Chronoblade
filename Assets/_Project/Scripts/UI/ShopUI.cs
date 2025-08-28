using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private Transform itemListContainer;
    [SerializeField] private GameObject shopSlotPrefab;
    [SerializeField] private ItemDetailPanel detailPanel;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button sellButton;

    private InventorySlot currentSelected;
    private ItemData selectedItem;

    public void OpenShopUI(ShopData data)
    {
        inventoryPanel.SetActive(true);
        shopPanel.SetActive(true);
        ClearList();

        foreach (var item in data.items)
        {
            var go = Instantiate(shopSlotPrefab, itemListContainer);
            var slot = go.GetComponent<InventorySlot>();
            slot.Set(item);
            slot.Bind();
            slot.onClick += OnSlotClicked;
        }

        buyButton.onClick.AddListener(OnBuyClicked);
        sellButton.onClick.AddListener(OnSellClicked);

        UpdateActionButtons();
    }

    private void OnSlotClicked(InventorySlot clicked)
    {
        if (currentSelected != null)
            currentSelected.SetSelected(false);

        currentSelected = clicked;
        currentSelected.SetSelected(true);
        selectedItem = clicked.item;
        detailPanel.Show(selectedItem);

        UpdateActionButtons();
    }

    private void OnBuyClicked()
    {
        if (selectedItem == null) return;
        ShopManager.Instance.BuyItem(selectedItem);
        UpdateActionButtons();
    }

    private void OnSellClicked()
    {
        if (selectedItem == null) return;
        ShopManager.Instance.SellItem(selectedItem);
        UpdateActionButtons();
    }

    private void UpdateActionButtons()
    {
        buyButton.interactable = selectedItem != null;
        sellButton.interactable = selectedItem != null &&
            InventoryManager.Instance.GetItemCount(selectedItem) > 0;
    }

    private void ClearList()
    {
        foreach (Transform child in itemListContainer)
            Destroy(child.gameObject);

        selectedItem = null;
        detailPanel.Clear();
    }

    public void CloseShopUI()
    {
        inventoryPanel.SetActive(false);
        shopPanel.SetActive(false);
    }
}
