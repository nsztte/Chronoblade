using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class InventoryManager : MonoBehaviour
{
    #region Singleton
    public static InventoryManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private Dictionary<string, int> itemCounts = new Dictionary<string, int>();
    private Dictionary<AmmoType, int> ammoCounts = new Dictionary<AmmoType, int>();

    // 슬롯 기반 인벤토리 구조(확장용, 실제 슬롯 로직은 추후 구현)
    // public List<InventorySlot> slots = new List<InventorySlot>();

    #region 일반아이템 관리
    public int AddItem(ItemData item, int amount)
    {
        // 유효성 검사
        if (item == null || item.maxStack <= 0) return amount;
        string key = item.itemName;
        if(!itemCounts.TryGetValue(key, out int currentCount))
            currentCount = 0;

        int spaceLeft = Mathf.Max(0, item.maxStack - currentCount);
        int toAdd = Mathf.Min(spaceLeft, amount);

        if(toAdd > 0)
            itemCounts[key] = currentCount + toAdd;

        return amount - toAdd;
    }

    public bool RemoveItem(ItemData item, int amount)
    {
        if (item == null) return false;
        string key = item.itemName;
        if(!itemCounts.ContainsKey(key)) return false;
        if(itemCounts[key] < amount) return false;

        itemCounts[key] -= amount;

        if(itemCounts[key] <= 0)
            itemCounts.Remove(key);

        return true;
    }

    public int GetItemCount(ItemData item)
    {
        if (item == null) return 0;
        string key = item.itemName;
        if(itemCounts.TryGetValue(key, out int count)) return count;
        return 0;
    }
    #endregion

    #region 탄약아이템 관리
    public int AddAmmo(AmmoType type, int amount)
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

        return amount - toAdd; // 추가되지 못한 탄약 수량 반환
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

        // 기본값 반환 (해당 타입의 무기가 없거나 WeaponManager가 없는 경우)
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
}