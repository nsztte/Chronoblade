using UnityEngine;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    [SerializeField] private ItemDatabase itemDatabase;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        itemDatabase.Initialize(); // 데이터베이스 초기화
    }

    public ItemData GetItemByID(string id)
    {
        return itemDatabase.Get(id);
    }

    public List<ItemData> GetAllItems()
    {
        return itemDatabase.GetAllItems();
    }

    // 아이템 사용
    public void HandleItemAction(ItemData itemData)
    {
        if(itemData == null) return;
        
        switch(itemData.itemType)
        {
            case ItemType.Equipment:
                var current = WeaponManager.Instance.CurrentWeapon;
                if (current != null && current.ItemData == itemData)
                {
                    WeaponManager.Instance.UnEquipWeapon();
                }
                else
                {
                    WeaponManager.Instance.EquipWeaponByItem(itemData);
                }
                break;
            case ItemType.Consumable:
                ApplyConsumableItemEffect(itemData);
                InventoryManager.Instance.RemoveItem(itemData, 1);
                break;
            default:
                Debug.LogError($"아이템 효과 미지정: {itemData.itemName}");
                break;
        }
    }

    public bool DropItem(ItemData item)
    {
        if(item == null) return false;

        if(item.itemType == ItemType.Equipment && InventoryManager.Instance.IsEquipped(item))
            WeaponManager.Instance.UnEquipWeapon();

        bool result = InventoryManager.Instance.TryRemoveItem(item, 1, out string fail);

        if (!result)
            Debug.LogWarning($"[ItemManager] {item.itemName} 버리기 실패: {fail}");

        return result;
    }

    // 소비형 아이템 효과 적용
    private void ApplyConsumableItemEffect(ItemData itemData)
    {
        if(InventoryManager.Instance.GetItemCount(itemData) <= 0)
        {
            Debug.Log($"{itemData.itemName} 보유량 부족");
            return;
        }

        switch(itemData.consumableItemEffectType)
        {
            case ConsumableItemEffectType.Heal:
                PlayerManager.Instance.HealHP(itemData.value);
                break;
            case ConsumableItemEffectType.ManaRestore:
                PlayerManager.Instance.RestoreMP(itemData.value);
                break;
            default:
                Debug.LogError($"아이템 효과 미지정: {itemData.itemName}");
                break;
        }
        Debug.Log($"{itemData.itemName} 사용");
    }
}
