using System;
using UnityEngine;

public class BossKeyPickup : MonoBehaviour, IInteractable, IInteractableSavable 
{
    [SerializeField] private Vector3 heldOffset = new Vector3(0, 0, 0);
    private Transform startParent;
    public int SlotIndex;
    public Action insert;
    [SerializeField] private bool isHeld = false;
    [SerializeField] private bool isActivated = false;
    private Rigidbody rb;
    // private Collider col;

    public bool IsHeld() => isHeld;
    public bool IsActivated() => isActivated;
    public bool CanActive { get; set; } = false;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // col = GetComponent<Collider>();
        // rb.isKinematic = false;

        startParent = transform.parent;
    }

    private void OnEnable()
    {
        isActivated = true;
    }

    private void OnDisable()
    {
        if (isHeld) PlayerManager.Instance?.ClearHeldObject();
        isActivated = false;
    }

    public void ActivateSocket(GameObject socket)
    {
        // isActivated = false;
        isHeld = false;

        PlayerManager.Instance?.ClearHeldObject();
        socket.SetActive(true);
        gameObject.SetActive(false);
    }


    private void PickUp(bool value)
    {
        if(isHeld == value) return;

        Transform socket = value ? PlayerManager.Instance.HeldPosition : startParent;

        isHeld = value;
        rb.isKinematic = value;
        // col.enabled = !value;
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
        // if (isActivated) return;
        if (!isHeld && PlayerManager.Instance != null && PlayerManager.Instance?.CurrentHeldObject) return;

        if (CanActive)
        {
            insert?.Invoke();
            CanActive = false;
            // isActivated = false;
        }
        else
        {
            PickUp(!isHeld);
        }
    }

    public string GetPrompt()
    {
        // if (isActivated) return "";
        if (CanActive) return "삽입하기";
        if (!isHeld && PlayerManager.Instance?.CurrentHeldObject != null) return "";
        return isHeld ? "놓기" : "들기";
    }

    public bool TryGetWorldPose(out Vector3 pos, out Quaternion rot)
    {
        pos = transform.position; rot = transform.rotation;
        return !isHeld;
    }

    public void ApplyActivated(bool activated)
    {
        Debug.Log($"{gameObject.name} 활성화 여부 : {activated}");
        // isActivated = activated;
        if (activated)
        {
            Debug.Log("ApplyActivated: SetActive(true)");
            if (isHeld) PlayerManager.Instance?.ClearHeldObject();
            gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("ApplyActivated: SetActive(false)");
            gameObject.SetActive(false);
        }
    }

    public void ApplyHeld(bool held) => PickUp(held);
    public void ApplyWorldPose(Vector3 p, Quaternion r)
    {
        if (!isHeld) transform.SetPositionAndRotation(p, r);
    }
}
