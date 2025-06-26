using UnityEngine;

public class ItemManager : MonoBehaviour
{
    #region Singleton
    public static ItemManager Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    // 아이템 사용
    public void UseItem(ItemData itemData)
    {
        if(itemData.itemType == ItemType.Consumable)
        {
            ApplyConsumableItemEffect(itemData);
            // 탄약 아이템이 아니라면 인벤토리에서 차감
            if(itemData.consumableItemEffectType != ConsumableItemEffectType.AmmoSupply)
                RemoveItem(itemData, 1);
        }
    }

    // 소비형 아이템 효과 적용
    private void ApplyConsumableItemEffect(ItemData itemData)
    {
        switch(itemData.consumableItemEffectType)
        {
            case ConsumableItemEffectType.Heal:
                PlayerManager.Instance.HealHP(itemData.value);
                break;
            case ConsumableItemEffectType.ManaRestore:
                PlayerManager.Instance.RestoreMP(itemData.value);
                break;
            case ConsumableItemEffectType.AmmoSupply:
                InventoryManager.Instance.AddAmmo(itemData.ammoType, itemData.value);
                break;
            default:
                Debug.LogError($"아이템 효과 미지정: {itemData.itemName}");
                break;
        }
        Debug.Log($"{itemData.itemName} 사용");
    }

    // 인벤토리 연동 메서드
    public int AddItem(ItemData item, int amount)
    {
        return InventoryManager.Instance.AddItem(item, amount);
    }

    public bool RemoveItem(ItemData item, int amount)
    {
        return InventoryManager.Instance.RemoveItem(item, amount);
    }

    public int GetItemCount(ItemData item)
    {
        return InventoryManager.Instance.GetItemCount(item);
    }
}
