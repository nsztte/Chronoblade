using System;
using UnityEngine;

public class WateringCan : MonoBehaviour, IInteractable
{
    [Header("워터 메쉬 설정")]
    [SerializeField] Transform waterMesh;
    [SerializeField] float minHeight = 0.0f;
    [SerializeField] float maxHeight = 1.0f;
    [SerializeField] float fillPerDrop = 0.05f;
    [SerializeField] private Transform startParent;

    [SerializeField] private float currentFill = 0f;
    public Action OnPlaced;
    public Action OnWatered;

    public bool IsHeld { get; private set; } = false;
    public bool IsPlaced { get; private set; } = false;
    public bool IsNearFaucet { get; set; } = false;
    public bool IsNearPlant { get; set; } = false;
    public bool IsEmpty => currentFill <= 0f;
    public bool IsFull => currentFill >= 1f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
    }

    public void PlaceTo(Transform socket, bool IsHeld)
    {
        this.IsHeld = IsHeld;
        IsPlaced = !IsHeld;
        rb.isKinematic = true;
        transform.SetParent(socket);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void AddDrop(bool isPlus)
    {
        if (isPlus && IsFull) return;
        if (!isPlus && IsEmpty) return;

        currentFill = Mathf.Clamp01(currentFill + (isPlus ? fillPerDrop : - fillPerDrop));
        ApplyFillVisual();
    }

    private void TryPickUp()
    {
        if(IsHeld) return;

        PlaceTo(PlayerManager.Instance.HeldPosition, true);
    }

    private void ApplyFillVisual()
    {
        if(!waterMesh) return;
        var p = waterMesh.localPosition;
        p.y = Mathf.Lerp(minHeight, maxHeight, currentFill);
        waterMesh.localPosition = p;
    }

    public void Interact()
    {
        if(!IsHeld)
            TryPickUp();
        else if(IsNearFaucet)
            OnPlaced?.Invoke();
        else if(IsNearPlant)
        {
            OnWatered?.Invoke();
            // 물주는 모션 추가
        }
        else
        {
            IsHeld = false;
            IsPlaced = false;
            IsNearFaucet = false;
            transform.SetParent(startParent);
            rb.isKinematic = false;
        }
    }
}
