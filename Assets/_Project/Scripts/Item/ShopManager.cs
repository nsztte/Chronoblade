using UnityEngine;

public class ShopManager : MonoBehaviour
{
    #region Singleton
    public static ShopManager Instance { get; private set; }

    public void Initialize()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning($"{GetType().Name} 인스턴스 감지됨, 초기화 스킵");
            return;
        }
        Instance = this;
    }
    #endregion

    private Shop currentShop;

    public void OpenShop(Shop shop)
    {
        currentShop = shop;

        UIManager.Instance.ShopUI.OpenShopUI(shop.shopData);

        // UIManager.Instance.SetCursorLockState(CursorLockMode.None);

        UIManager.Instance.ShowOverlayBackground();
    }

    public void SellItem(ItemData item)
    {
        if(item == null)
        {
            Debug.LogError("[ShopManager] 판매할 아이템이 없습니다.");
            return;
        }

        int ownedCount = InventoryManager.Instance.GetItemCount(item);

        if(ownedCount <= 0)
        {
            Debug.LogError($"[ShopManager] 아이템이 없습니다. 판매 실패: {item.name}");
            return;
        }

        bool removed = InventoryManager.Instance.RemoveItem(item, 1);
        if(!removed)
        {
            Debug.LogError($"[ShopManager] 아이템 판매 실패: {item.name}");
            return;
        }

        int sellPrice = currentShop.shopData.GetSellPrice(item);

        PlayerManager.Instance.AddGold(sellPrice);
        Debug.Log($"[ShopManager] 아이템 판매 성공: {item.name} / 판매 가격: {sellPrice}G");
    }

    public void BuyItem(ItemData item)
    {
        int price = item.price;

        // 1단계: 골드 차감
        if(!PlayerManager.Instance.SpendGold(price))
        {
            Debug.LogError($"[ShopManager] 골드가 부족합니다. 아이템 구매 실패: {item.name}");
            return;
        }

        // 2단계: 아이템 추가 시도
        int leftOver = InventoryManager.Instance.TryAddItem(item, 1, out string failReason);
        if(leftOver > 0)
        {
            // 아이템 추가 실패 시 골드 환불
            PlayerManager.Instance.AddGold(price);
            Debug.LogError($"[ShopManager] 아이템 구매 실패: {failReason} - 골드 환불됨: {price}G");
            return;
        }

        // 3단계: 구매 성공
        Debug.Log($"[ShopManager] 아이템 구매 성공: {item.name} / 수량: {InventoryManager.Instance.GetItemCount(item)}");
    }

    public void CloseShop()
    {
        currentShop = null;

        UIManager.Instance.ShopUI.CloseShopUI();

        // UIManager.Instance.SetCursorLockState(CursorLockMode.Locked);

        UIManager.Instance.HideOverlayBackground();
    }
}
