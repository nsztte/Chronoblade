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
        int remaining = InventoryManager.Instance.AddItem(itemData, amount);
        if(remaining <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            amount = remaining;
        }
    }
}
