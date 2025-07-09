using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ShopData", menuName = "Item/ShopData")]
public class ShopData : ScriptableObject
{
    [Header("상점 아이템 목록")]
    public List<ItemData> items;
    [Header("상점 아이템 감가율")]
    [Range(0, 1)] public float depreciationRate = 0.5f;

    // 아이템 판매 가격 계산 (감가율 적용)
    public int GetSellPrice(ItemData item)
    {
        return Mathf.RoundToInt(item.price * depreciationRate);
    }
}
