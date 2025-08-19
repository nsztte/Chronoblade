using System;
using UnityEngine;

public class BossKeyPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform startParent;
    [SerializeField] private Vector3 heldOffset = new Vector3(0, 0, 0);
    public int SlotIndex;
    public Action insert;
    private bool isHeld = false;
    private bool isInserted = false;
    private Rigidbody rb;
    private Collider col;

    public bool IsHeld => isHeld;
    public bool IsInserted => isInserted;
    public bool CanInsert { get; set; } = false;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        rb.isKinematic = false;

        if (startParent == null) startParent = transform.parent;
    }
    
    public void InsertToSocket(Transform socket)
    {
        isInserted = true;
        isHeld = false;

        rb.isKinematic = true;
        col.enabled = false;

        PlayerManager.Instance?.ClearHeldObject();

        transform.SetParent(socket, false);
        transform.localPosition =Vector3.zero;
        transform.localRotation = Quaternion.identity;
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
        if(isInserted) return;

        if(CanInsert)
        {
            insert?.Invoke();
            CanInsert = false;
        }
        else
        {
            PickUp(!isHeld);
        }
    }
}
