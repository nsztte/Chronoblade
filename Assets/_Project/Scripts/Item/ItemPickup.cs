using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class ItemPickup : MonoBehaviour, IInteractable, IInteractableSavable
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private int amount = 1;
    [SerializeField] private UnityEvent onPickupSuccess;

    private bool isPlayerInRange = false;

    private Collider col;

    private void Awake()
    {
        col = GetComponent<Collider>();
        col.isTrigger = true;

        if (itemData == null)
        {
            Debug.LogWarning($"[ItemPickup] {name} 오브젝트의 itemData가 비어 있습니다.", this);
        }
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
                InventoryManager.Instance.RegisterWeapon(itemData);
            }

            onPickupSuccess.Invoke();

            if(col != null) col.enabled = false;
            gameObject.SetActive(false);
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
        if (col == null || !col.enabled) return "";

        return itemData.isAutoPickup ? "" : $"줍기: {itemData.itemName}";
    }


    #region IInteractableSavable 구현부
    public bool IsActivated()
    {
        return gameObject.activeSelf;
    }

    public bool IsHeld()
    {
        return false;
    }

    public bool TryGetWorldPose(out Vector3 pos, out Quaternion rot)
    {
        pos = transform.position;
        rot = transform.rotation;
        return true;
    }

    public void ApplyActivated(bool activated)
    {
        if (col == null) col = GetComponent<Collider>();

        if (activated)
        {
            gameObject.SetActive(true);
            col.enabled = true;
            isPlayerInRange = false; // 로드 시에는 항상 새로 트리거 들어오게
        }
        else
        {
            // 이미 주운 상태: 안 보이고, 콜리전도 꺼둠
            col.enabled = false;
            gameObject.SetActive(false);
            isPlayerInRange = false;
        }
    }

    public void ApplyHeld(bool held){}

    public void ApplyWorldPose(Vector3 pos, Quaternion rot)
    {
        transform.SetPositionAndRotation(pos, rot);
    }
    #endregion

    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (itemData == null)
        {
            Debug.LogWarning($"[ItemPickup] {name} 오브젝트의 itemData가 비어 있습니다.", this);
        }
    }
    #endif
}
