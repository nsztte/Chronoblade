using UnityEngine;

[RequireComponent(typeof(Animator)), RequireComponent(typeof(GenericInteractionSaveProxy)), RequireComponent(typeof(Collider))]
public class DoorController : MonoBehaviour, IInteractableSavable
{
    private Animator animator;
    [SerializeField] private bool isOpen = false;
    [SerializeField] private bool isUnlocked = false;
    [SerializeField] private DoorController[] nextDoor;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isUnlocked) return;

        if (other.CompareTag("Player") && !isOpen)
        {
            OpenDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isUnlocked) return;
        
        if (other.CompareTag("Player") && isOpen)
        {
            CloseDoor();
        }
    }

    public void OpenDoor()
    {
        isOpen = true;
        animator.SetTrigger("Open");
    }

    public void CloseDoor()
    {
        isOpen = false;
        animator.SetTrigger("Close");
    }

    public void OnConditionMet()
    {
        if (nextDoor == null) return;
        foreach (var door in nextDoor)
        {
            door.SetUnlocked(true);
            door.OpenDoor();
        }
    }
    public void SetUnlocked(bool unlocked) => isUnlocked = unlocked;

    #region IInteractableSavable 구현부
    public bool IsActivated()
    {
        return isOpen;
    }

    public bool IsHeld()
    {
        return isUnlocked;
    }

    public bool TryGetWorldPose(out Vector3 pos, out Quaternion rot)
    {
        pos = Vector3.zero;
        rot = Quaternion.identity;
        return false;
    }

    public void ApplyActivated(bool activated)
    {
        // 세이브된 "열림 상태"를 복원
        if (activated)
        {
            OpenDoor();
        }
        else
        {
            isOpen = false;
        }
    }

    public void ApplyHeld(bool held)
    {
        // 세이브된 "잠금 상태" 복원
        SetUnlocked(held);
    }

    public void ApplyWorldPose(Vector3 pos, Quaternion rot){}
    #endregion
}
