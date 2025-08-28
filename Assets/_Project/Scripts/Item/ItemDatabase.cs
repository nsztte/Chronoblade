using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Database/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    [SerializeField] private List<ItemData> items;

    private Dictionary<string, ItemData> itemDict;

    public void Initialize()
    {
        itemDict = new Dictionary<string, ItemData>();

        foreach (var item in items)
        {
            if (itemDict.ContainsKey(item.itemID))
            {
                Debug.LogWarning($"[ItemDatabase] 중복된 아이템 ID 발견: {item.itemID}");
                continue;
            }

            itemDict[item.itemID] = item;
        }
    }

    public ItemData Get(string id)
    {
        if (itemDict == null) Initialize();
        return itemDict.TryGetValue(id, out var item) ? item : null;
    }

    public List<ItemData> GetAllItems()
    {
        if (itemDict == null) Initialize();
        return itemDict.Values.ToList();
    }
}
