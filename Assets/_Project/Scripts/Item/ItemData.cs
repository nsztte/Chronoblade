using UnityEngine;


[CreateAssetMenu(fileName = "ItemData", menuName = "Item/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("기본 정보")]
    public string itemName;
    public Sprite icon;
    public ItemType itemType;
    public ConsumableItemEffectType consumableItemEffectType;
    public AmmoType ammoType;   // AmmoSupply 타입일 경우 사용

    [Header("효과 수치")]
    public int value;
    public float duration;  // 현재 사용 안함, 향후 확장성 용도

    [Header("소지 및 상점 정보")]
    public int maxStack = 1;
    public int price = 0;

    [Header("설명")]
    [TextArea(3, 10)] public string description;
}

/// <summary>
/// 아이템 종류
/// 추후 확장성 고려
/// </summary>
public enum ItemType
{
    Consumable,   // 소비형: 포션, 탄약 등
    Equipment,    // 장비형: 무기, 방어구 등
    Material,     // 재료형: 제작 재료, 상점 구매품 등
    Quest         // 퀘스트 아이템
}

public enum ConsumableItemEffectType
{
    Heal,           // 체력 회복
    ManaRestore,    // 마나 회복
    AmmoSupply      // 총알 보충
}

public enum AmmoType
{
    None,
    PistolAmmo,
    RifleAmmo,
    ShotgunAmmo
}