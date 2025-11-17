using System;
using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    #region Singleton
    public static InventoryManager Instance { get; private set; }

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

    [SerializeField] private ItemData defaultPistolData;
    private Dictionary<string, int> itemCounts = new Dictionary<string, int>();
    private Dictionary<AmmoType, int> ammoCounts = new Dictionary<AmmoType, int>();
    [SerializeField] private WeaponData equippedWeapon;

    public Dictionary<ItemData, int> GetAllItems()
    {
        Dictionary<ItemData, int> result = new();

        foreach (var kvp in itemCounts)
        {
            var item = ItemManager.Instance.GetItemByID(kvp.Key);
            if (item != null)
                result[item] = kvp.Value;
        }

        return result;
    }
    
    public int TryAddItem(ItemData item, int amount, out string faliReason)
    {
        faliReason = string.Empty;

        if(item.itemType == ItemType.Consumable && item.consumableItemEffectType == ConsumableItemEffectType.AmmoSupply)
        {
            int totalAmmo = item.value * amount;
            int leftOverAmmo = AddAmmo(item.ammoType, totalAmmo);
            if(leftOverAmmo > 0)
            {
                faliReason = $"최대 탄약 보유량을 초과했습니다. 최대 탄약 보유량: {GetMaxAmmoForType(item.ammoType)}";
            }

            return leftOverAmmo;
        }
        else
        {
            int leftOver = AddItem(item, amount);
            if(leftOver > 0)
            {
                faliReason = $"최대 아이템 보유량을 초과했습니다. 최대 아이템 보유량: {item.maxStack}";
            }
            return leftOver;
        }
    }

    public bool TryRemoveItem(ItemData item, int amount, out string failReason)
    {
        failReason = string.Empty;
        if(item.itemType == ItemType.Consumable && item.consumableItemEffectType == ConsumableItemEffectType.AmmoSupply)
        {
            bool success = DropAmmo(item.ammoType, item.value * amount);
            if(!success)
            {
                failReason = $"탄약 보유량이 부족합니다. 탄약 보유량: {ammoCounts[item.ammoType]}";
            }
            return success;
        }
        else
        {
            bool success = RemoveItem(item, amount);
            if(!success)
            {
                failReason = $"아이템 보유량이 부족합니다. 아이템 보유량: {itemCounts[item.itemID]}";
            }
            return success;
        }
    }

    // 새 게임용 초기 세팅
    public void GiveDefaultWeaponAndQuickSlot()
    {
        if (defaultPistolData == null) return;

        // 인벤토리에 등록
        RegisterWeapon(defaultPistolData);

        // 퀵슬롯 0번에 배치
        QuickSlotManager.Instance?.AssignItemToSlot(0, defaultPistolData);
    }

    #region 세이브/로드 관리
    [Serializable]
    public struct ItemEntry { public string id; public int count; }

    [Serializable]
    public struct AmmoEntry { public AmmoType type; public int count; }

    public (List<ItemEntry> items, List<AmmoEntry> ammos) DumpItemsAndAmmo()
    {
        // 아이템 덤프
        var items = new List<ItemEntry>();
        var all = GetAllItems();
        foreach (var kv in all)
        {
            if (kv.Key != null && kv.Value > 0)
                items.Add(new ItemEntry { id = kv.Key.itemID, count = kv.Value });
        }

        // 탄약 덤프
        var ammos = new List<AmmoEntry>();
        foreach (AmmoType t in Enum.GetValues(typeof(AmmoType)))
        {
            if (t == AmmoType.None) continue;
            int c = GetAmmoCount(t);
            if (c > 0) ammos.Add(new AmmoEntry { type = t, count = c });
        }
        return (items, ammos);
    }

    public void RestoreItemsAndAmmo(List<ItemEntry> items, List<AmmoEntry> ammos)
    {
        // 내부 상태 초기화
        itemCounts.Clear();
        ammoCounts.Clear();

        // 아이템 복원
        foreach (var e in items)
        {
            var item = ItemManager.Instance.GetItemByID(e.id);
            if (item == null) continue;

            int leftover = TryAddItem(item, e.count, out _);   // 내부에서 퀵슬롯 Refresh 호출
            if (leftover > 0)
                Debug.LogWarning($"[InventoryManager] '{e.id}' 일부 미반영: {leftover}");
        }

        // 탄약 복원
        foreach (var a in ammos)
        {
            if (a.type == AmmoType.None || a.count <= 0) continue;

            int leftover = AddAmmo(a.type, a.count);    // 내부에서 Ammo UI 업데이트 포함
            if (leftover > 0)
                Debug.LogWarning($"[InventoryManager] '{a.type}' 탄약 일부 미반영: {leftover}");
        }
    }
    #endregion

    #region 일반아이템 관리
    private int AddItem(ItemData item, int amount)
    {
        // 유효성 검사
        if (item == null || item.maxStack <= 0) return amount;
        string key = item.itemID;
        if(!itemCounts.TryGetValue(key, out int currentCount))
            currentCount = 0;

        int spaceLeft = Mathf.Max(0, item.maxStack - currentCount);
        int toAdd = Mathf.Min(spaceLeft, amount);

        if(toAdd > 0)
            itemCounts[key] = currentCount + toAdd;

        // 퀵슬롯 업데이트
        QuickSlotManager.Instance?.RefreshAllSlotVisuals();

        return amount - toAdd;
    }

    public bool RemoveItem(ItemData item, int amount)
    {
        if (item == null) return false;
        string key = item.itemID;
        if(!itemCounts.ContainsKey(key)) return false;
        if(itemCounts[key] < amount) return false;

        itemCounts[key] -= amount;

        if(itemCounts[key] <= 0)
            itemCounts.Remove(key);

        // 퀵슬롯 업데이트
        QuickSlotManager.Instance?.RefreshAllSlotVisuals();

        return true;
    }

    public int GetItemCount(ItemData item)
    {
        if (item == null) return 0;
        string key = item.itemID;
        if(itemCounts.TryGetValue(key, out int count)) return count;
        return 0;
    }
    #endregion

    #region 탄약아이템 관리
    private int AddAmmo(AmmoType type, int amount)
    {
        // 현재 탄약 수량 확인
        if (!ammoCounts.TryGetValue(type, out int currentCount))
            currentCount = 0;

        // 해당 탄약 타입의 최대 보유량 찾기
        int maxAmmo = GetMaxAmmoForType(type);
        
        // 추가 가능한 탄약 수량 계산
        int spaceLeft = Mathf.Max(0, maxAmmo - currentCount);
        int toAdd = Mathf.Min(spaceLeft, amount);

        if (toAdd > 0)
            ammoCounts[type] = currentCount + toAdd;

        var curWeapon = WeaponManager.Instance.CurrentWeapon;
        if(curWeapon != null && curWeapon is GunWeaponController gun && curWeapon.weaponData.ammoType == type)
            gun.UpdateAmmoCount();

        Debug.Log($"[InventoryManager] 탄약 추가 결과: {toAdd} | 남은 수량: {ammoCounts[type]}");

        return amount - toAdd; // 추가되지 못한 탄약 수량 반환
    }

    private bool DropAmmo(AmmoType type, int amount)
    {
        if(!ammoCounts.ContainsKey(type)) return false;
        if(ammoCounts[type] < amount) return false;

        ammoCounts[type] -= amount;

        if(ammoCounts[type] <= 0) ammoCounts.Remove(type);

        return true;
    }

    private int GetMaxAmmoForType(AmmoType type)
    {
        // WeaponManager의 모든 무기 슬롯에서 해당 타입의 maxAmmo 찾기
        var weaponSlots = WeaponManager.Instance?.GetWeaponSlots();
        if (weaponSlots != null)
        {
            foreach (var weapon in weaponSlots)
            {
                if (weapon != null && weapon.weaponData != null && weapon.weaponData.ammoType == type)
                {
                    return weapon.weaponData.maxAmmo;
                }
            }
        }

        // 기본값 반환
        switch (type)
        {
            case AmmoType.PistolAmmo: return 60;
            case AmmoType.RifleAmmo: return 120;
            case AmmoType.ShotgunAmmo: return 30;
            default: return 100;
        }
    }

    public bool UseAmmo(AmmoType type, int amount)
    {
        if (!ammoCounts.ContainsKey(type)) return false;
        if (ammoCounts[type] < amount) return false;

        ammoCounts[type] -= amount;

        if (ammoCounts[type] <= 0)
            ammoCounts.Remove(type);

        return true;
    }

    public int GetAmmoCount(AmmoType type)
    {
        if (ammoCounts.TryGetValue(type, out int count)) return count;
        return 0;
    }
    #endregion

    #region 무기 획득 관리
    public void RegisterWeapon(ItemData item)
    {
        // 유효성 검사: 무기 아이템인지 확인
        if (item == null || item.itemType != ItemType.Equipment || item.weaponData == null)
            return;

        // TryAddItem을 통해 인벤토리에 등록 (중복 방지 포함)
        TryAddItem(item, 1, out string _);

        Debug.Log($"[InventoryManager] 무기 획득 등록됨: {item.itemName} (itemID: {item.itemID})");
    }

    public bool IsWeaponObtained(ItemData item)
    {
        return item != null &&
            item.itemType == ItemType.Equipment &&
            item.weaponData != null &&
            GetItemCount(item) > 0;
    }
    #endregion
    
    #region 무기 장착 관리
    public bool IsEquipped(ItemData item)
    {
        return item.itemType == ItemType.Equipment && item.weaponData != null && item.weaponData == equippedWeapon;
    }

    public void Equip(WeaponData weapon)
    {
        equippedWeapon = weapon;
    }

    public void Unequip(WeaponData weapon)
    {
        equippedWeapon = null;
    }
    #endregion
}