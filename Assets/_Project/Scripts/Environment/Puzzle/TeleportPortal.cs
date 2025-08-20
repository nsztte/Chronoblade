using UnityEngine;

public class TeleportPortal : MonoBehaviour
{
    [SerializeField] private Transform teleportPosition;
    private Collider col;

    private void Awake()
    {
        col = GetComponent<Collider>();

        if(col != null)
            col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
           var controller = other.GetComponent<CharacterController>();
            if (controller != null)
                controller.enabled = false;

            other.transform.position = teleportPosition.position;
            other.transform.rotation = teleportPosition.rotation;

            if (controller != null)
                controller.enabled = true;
        }
    }
}
