using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public class HoldableBehaviour : MonoBehaviour
{
    [SerializeField] Transform startParent;
    [SerializeField] Vector3 heldOffset = Vector3.zero;

    public bool IsHeld { get; private set; }
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void AttachToHand(Transform hand)
    {
        IsHeld = true;
        rb.isKinematic = true;
        transform.SetParent(hand, true);
        transform.localPosition = heldOffset;
        transform.localRotation = Quaternion.identity;
    }

    public void DetachToWorld(Transform worldParent)
    {
        IsHeld = false;
        transform.SetParent(worldParent ? worldParent : startParent, true);
        rb.isKinematic = false;
    }

    public bool TryGetWorldPose(out Vector3 pos, out Quaternion rot)
    {
        pos = transform.position; rot = transform.rotation;
        return !IsHeld; // 손에 들린 상태면 월드포즈 저장 안 함
    }

    public void ApplyWorldPose(Vector3 pos, Quaternion rot)
    {
        if (IsHeld) return;
        transform.SetPositionAndRotation(pos, rot);
    }

    public Transform DefaultWorldParent => startParent;
}
