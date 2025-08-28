using UnityEngine;

public class KeycardDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData requiredKeycard;
    private bool isOpen = false;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        if(isOpen) return;

        if(InventoryManager.Instance.GetItemCount(requiredKeycard) > 0)
        {
            animator.SetTrigger("Open");
            isOpen = true;
        }
        else
        {
            Debug.Log("필요한 키카드가 없습니다: " + requiredKeycard.itemID);
        }
    }

    public string GetPrompt()
    {
        if (isOpen) return "";

        bool hasKey = InventoryManager.Instance.GetItemCount(requiredKeycard) > 0;
        return hasKey ? "문 열기" : $"[잠김] {requiredKeycard.itemName} 필요";
    }
}
