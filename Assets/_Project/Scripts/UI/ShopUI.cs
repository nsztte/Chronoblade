using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private InventoryUI inventoryPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private Transform itemListContainer;
    [SerializeField] private GameObject shopSlotPrefab;
    [SerializeField] private ItemDetailPanel detailPanel;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button sellButton;

    private ShopData currentShopData;
    private InventorySlot currentSelected;
    private ItemData selectedItem;

    private void OnEnable()
    {
        InputManager.Instance.OnPause += ShopManager.Instance.CloseShop;
        GameManager.Instance.EnterPaused();
        SelectedItemContext.OnSelectedItemChanged += OnSelectedChanged;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnPause -= ShopManager.Instance.CloseShop;
        SelectedItemContext.OnSelectedItemChanged -= OnSelectedChanged;
    }

    public void OpenShopUI(ShopData data)
    {
        currentShopData = data;
        inventoryPanel.Open(InventoryOpenContext.Shop, detailPanel);
        shopPanel.SetActive(true);
        ClearList();

        foreach (var item in data.items)
        {
            var go = Instantiate(shopSlotPrefab, itemListContainer);
            var slot = go.GetComponent<InventorySlot>();
            slot.Set(item, true);
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

        SelectedItemContext.Set(selectedItem); // ← 공용 선택 설정
        UpdateActionButtons();
    }

    private void OnBuyClicked()
    {
        if (selectedItem == null) return;
        ShopManager.Instance.BuyItem(selectedItem);
        inventoryPanel.UpdateOrAddSlot(selectedItem, detailPanel);
        UpdateActionButtons();
    }

    private void OnSellClicked()
    {
        if (selectedItem == null) return;
        ShopManager.Instance.SellItem(selectedItem);
        inventoryPanel.UpdateOrAddSlot(selectedItem);
        UpdateActionButtons();
    }

    private void UpdateActionButtons()
    {
        if (selectedItem == null)
        {
            buyButton.interactable = false;
            sellButton.interactable = false;
            return;
        }

        // 상점에 등록된 아이템인지 확인
        bool isInShop = currentShopData != null && currentShopData.items.Contains(selectedItem);

        buyButton.interactable = isInShop;
        sellButton.interactable = isInShop && InventoryManager.Instance.GetItemCount(selectedItem) > 0;
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
        currentShopData = null;
        currentSelected = null;
        selectedItem = null;
        inventoryPanel.gameObject.SetActive(false);
        shopPanel.SetActive(false);
    }

    private void OnSelectedChanged(ItemData item)
    {
        // 상점 슬롯 하이라이트 처리
        currentSelected = null;
        foreach (Transform child in itemListContainer)
        {
            var slot = child.GetComponent<InventorySlot>();
            bool sel = slot.item == item;
            slot.SetSelected(sel);
            if (sel) currentSelected = slot;
        }

        // 디테일 패널 동기화
        if (item == null && detailPanel.gameObject.activeSelf)
            detailPanel.Clear();
        else if (item != null)
            detailPanel.Show(item);

        // 상점에 등록된 아이템만 버튼 활성화
        selectedItem = item;
        UpdateActionButtons();
    }
}
