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
            Vector3 position = teleportPosition.position;
            Quaternion rotation = teleportPosition.rotation;

            PlayerManager.Instance.PlayerController.SetPositionAndRotation(position, rotation);
        }
    }
}
