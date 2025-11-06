using UnityEngine;

public class KeycardDoor : MonoBehaviour, IInteractable, IInteractableSavable
{
    [SerializeField] private ItemData requiredKeycard;
    private bool isOpen = false;
    private Animator animator;

    [SerializeField] private string openStateName = "OpenState";
    [SerializeField] private string closedStateName = "ClosedState";

    [SerializeField] private Collider colToActivate;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if(colToActivate) colToActivate.enabled = false;
    }

    public void Interact()
    {
        if(isOpen) return;

        if(InventoryManager.Instance.GetItemCount(requiredKeycard) > 0)
        {
            animator.SetTrigger("Open");
            isOpen = true;
            if(colToActivate) colToActivate.enabled = true;
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



    public bool IsActivated() => isOpen;
    public bool IsHeld() => false;

    public bool TryGetWorldPose(out Vector3 pos, out Quaternion rot)
    {
        // 문은 포즈 저장이 필요 없으므로 항상 false
        pos = default; rot = default;
        return false;
    }

    public void ApplyActivated(bool activated)
    {
        isOpen = activated;

        if (colToActivate) colToActivate.enabled = activated;

        if (!animator) return;

        if (!string.IsNullOrEmpty(openStateName) && !string.IsNullOrEmpty(closedStateName))
        {
            var state = activated ? openStateName : closedStateName;
            animator.Play(state, 0, 1f); // 끝 프레임로 스냅
            animator.Update(0f);         // 즉시 평가
            return;
        }
        
        if (activated)
        {
            animator.ResetTrigger("Open");
            animator.SetTrigger("Open");
            animator.Update(0f);
        }
    }

    public void ApplyHeld(bool held) {}
    public void ApplyWorldPose(Vector3 pos, Quaternion rot) {}
}
