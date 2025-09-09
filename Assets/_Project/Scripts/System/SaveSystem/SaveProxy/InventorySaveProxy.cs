using System;
using System.Collections.Generic;
using UnityEngine;

public class InventorySaveProxy : SaveableBehaviour
{
    [Serializable]
    private class Data
    {
        public List<InventoryManager.ItemEntry> items = new();
        public List<InventoryManager.AmmoEntry> ammos = new();
    }

    private InventoryManager Inventory => InventoryManager.Instance;

    public override string CaptureStateJson()
    {
        var (items, ammos) = Inventory.DumpItemsAndAmmo();
        var d = new Data { items = items, ammos = ammos };
        return JsonUtility.ToJson(d);
    }

    public override void RestoreStateJson(string json)
    {
        if (string.IsNullOrEmpty(json)) return;
        var d = JsonUtility.FromJson<Data>(json);

        // 딕셔너리 초기화 후 복원 (UI 자동 동기화)
        Inventory.RestoreItemsAndAmmo(d.items, d.ammos);
    }
}
