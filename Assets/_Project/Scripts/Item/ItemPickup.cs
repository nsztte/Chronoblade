using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ItemPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private int amount = 1;

    private bool isPlayerInRange = false;

    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player")) return;

        isPlayerInRange = true;

        if(itemData.isAutoPickup)
        {
            TryPickup();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag("Player")) return;

        isPlayerInRange = false;
    }

    public void Interact()
    {
        if (isPlayerInRange)
        {
            TryPickup();
        }
    }

    private void TryPickup()
    {
        int leftOver = InventoryManager.Instance.TryAddItem(itemData, amount, out string failReason);

        if(leftOver <= 0)
        {
            if(itemData.itemType == ItemType.Equipment && itemData.weaponData != null)
            {
                InventoryManager.Instance.RegisterWeapon(itemData.weaponData);
            }
            Destroy(gameObject);
        }
        else
        {
            // 총알 아이템이라면: 남은 총알 수 → 박스 개수로 환산
            if (itemData.itemType == ItemType.Consumable &&
                itemData.consumableItemEffectType == ConsumableItemEffectType.AmmoSupply)
            {
                int originalValue = itemData.value;
                int remaining = leftOver;

                itemData = Instantiate(itemData);
                itemData.value = remaining;
                amount = Mathf.CeilToInt((float)remaining / originalValue);
            }
            else
            {
                amount = leftOver; // 일반 아이템은 남은 개수 그대로 사용
            }

            Debug.Log($"[ItemPickup] 일부만 획득됨. 남은 수량: {leftOver} | 사유: {failReason}");
        }
    }

    public string GetPrompt()
    {
        return $"줍기: {itemData.itemName}";
    }
}
