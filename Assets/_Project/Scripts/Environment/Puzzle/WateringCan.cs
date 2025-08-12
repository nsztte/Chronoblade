using System;
using System.Collections;
using UnityEngine;

public class WateringCan : MonoBehaviour, IInteractable
{
    [Header("워터 메쉬 설정")]
    [SerializeField] Transform waterMesh;
    [SerializeField] float minHeight = 0.0f;
    [SerializeField] float maxHeight = 1.0f;
    [SerializeField] float fillPerDrop = 0.05f;
    [SerializeField] private Transform startParent;
    [SerializeField] private Vector3 heldOffset = new Vector3(0, 0, 0);
    [SerializeField] private ParticleSystem pourEffect;
    [SerializeField] private float pouringDuration = 5f;

    private bool isPouring = false;
    [SerializeField] private float currentFill = 0f; // 디버그용

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

        if(IsHeld)
        {
            PlayerManager.Instance.SetHeldObject(gameObject);
            transform.localPosition = heldOffset;
            transform.localRotation = Quaternion.identity;  // 향후 회전 오프셋도 적용 가능성 있음
        }
        else
        {
            PlayerManager.Instance.ClearHeldObject();
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
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

    private void DropToStart()
    {
        IsHeld = false;
        IsPlaced = false;
        IsNearFaucet = false;

        PlayerManager.Instance.ClearHeldObject();

        transform.SetParent(startParent);            
        rb.isKinematic = false;
    }

    private void PlayPourAnimation()
    {
        if(isPouring) return;
        isPouring = true;
        PlayerManager.Instance.SetAnimatorBool("IsPour", true);

        if(pourEffect != null)
            pourEffect.Play();

        StartCoroutine(StopPourAnimation(pouringDuration));
        StartCoroutine(DrainWaterOverTime(pouringDuration));
    }

    private IEnumerator StopPourAnimation(float duration)
    {
        yield return new WaitForSeconds(duration);

        PlayerManager.Instance.SetAnimatorBool("IsPour", false);

        if(pourEffect != null)
            pourEffect.Stop();
        
        isPouring = false;
        DropToStart();
    }

    private IEnumerator DrainWaterOverTime(float duration)
    {
        float startFill = currentFill;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            currentFill = Mathf.Lerp(startFill, 0f, t);
            ApplyFillVisual();
            yield return null;
        }

        currentFill = 0f;
        ApplyFillVisual();
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
            PlayPourAnimation();
        }
        else
        {
            DropToStart();
        }
    }
}
