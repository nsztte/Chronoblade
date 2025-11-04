using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;

    [SerializeField] private bool isUnlocked = false;
    [SerializeField] private DoorController nextDoor;

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
            isOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isUnlocked) return;
        
        if (other.CompareTag("Player") && isOpen)
        {
            CloseDoor();
            isOpen = false;
        }
    }

    public void OpenDoor() => animator.SetTrigger("Open");
    public void CloseDoor() => animator.SetTrigger("Close");

    public void OnConditionMet()
    {
        if (nextDoor == null) return;
        nextDoor.SetUnlocked(true);
        nextDoor.OpenDoor();
    }
    public void SetUnlocked(bool unlocked) => isUnlocked = unlocked;
}
