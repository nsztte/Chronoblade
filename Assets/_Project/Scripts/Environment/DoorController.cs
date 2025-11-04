using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;

    [SerializeField] private bool isUnlocked = false;
    [SerializeField] private DoorController[] nextDoor;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
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
}
