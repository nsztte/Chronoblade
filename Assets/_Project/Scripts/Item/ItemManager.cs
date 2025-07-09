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
            InventoryManager.Instance.RemoveItem(itemData, 1);
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
            // case ConsumableItemEffectType.AmmoSupply:
            //     InventoryManager.Instance.AddAmmo(itemData.ammoType, itemData.value);
            //     break;
            default:
                Debug.LogError($"아이템 효과 미지정: {itemData.itemName}");
                break;
        }
        Debug.Log($"{itemData.itemName} 사용");
    }

    // 인벤토리 연동 메서드
    // public bool AddItem(ItemData item, int amount)
    // {
    //     string failReason;
    //     if(InventoryManager.Instance.TryAddItem(item, amount, out failReason))
    //     {
    //         return true;
    //     }
    //     else
    //     {
    //         Debug.LogError($"[ItemManager] 아이템 추가 실패: {failReason}");
    //         return false;
    //     }
    // }

    // public bool RemoveItem(ItemData item, int amount)
    // {
    //     return InventoryManager.Instance.RemoveItem(item, amount);
    // }

    // public int GetItemCount(ItemData item)
    // {
    //     return InventoryManager.Instance.GetItemCount(item);
    // }
}
