using UnityEngine;

public class BossKeyPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform startParent;
    [SerializeField] private Vector3 heldOffset = new Vector3(0, 0, 0);

    private bool isHeld = false;
    private Rigidbody rb;
    private Collider col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        rb.isKinematic = false;

        if (startParent == null) startParent = transform.parent;
    }

    private void PickUp(bool value)
    {
        if(isHeld == value) return;

        Transform socket = value ? PlayerManager.Instance.HeldPosition : startParent;

        isHeld = value;
        rb.isKinematic = value;
        col.enabled = !value;
        transform.SetParent(socket, true);

        if(value)
        {
            PlayerManager.Instance.SetHeldObject(gameObject);
            transform.localPosition = heldOffset;
            transform.localRotation = Quaternion.identity;  // 향후 회전 오프셋도 적용 가능성 있음
        }
        else
        {
            PlayerManager.Instance.ClearHeldObject();
        }
    }

    public void Interact()
    {
        PickUp(!isHeld);
    }
}
