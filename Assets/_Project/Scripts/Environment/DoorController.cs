using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isOpen)
            {
                OpenDoor();
                isOpen = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isOpen)
            {
                CloseDoor();
                isOpen = false;
            }
        }
    }

    private void OpenDoor()
    {
        animator.SetTrigger("Open");
    }

    private void CloseDoor()
    {
        animator.SetTrigger("Close");
    }
}
