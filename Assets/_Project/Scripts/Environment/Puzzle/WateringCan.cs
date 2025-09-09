using System;
using System.Collections;
using UnityEngine;

public class WateringCan : MonoBehaviour, IInteractable, IInteractableSavable
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
    private bool isHeld = false;
    [SerializeField] private float currentFill = 0f; // 디버그용

    public Action OnPlaced;
    public Action OnWatered;
    // public bool IsHeld { get; private set; } = false;
    public bool IsPlaced { get; private set; } = false;
    public bool IsNearFaucet { get; set; } = false;
    public bool IsNearPlant { get; set; } = false;
    public bool IsEmpty => currentFill <= 0f;
    public bool IsFull => currentFill >= 1f;

    public bool IsActivated() => true; // 의미 없으니 상수
    public bool IsHeld() => isHeld;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
    }

    public void PlaceTo(Transform socket, bool IsHeld)
    {
        isHeld = IsHeld;
        IsPlaced = !IsHeld;
        rb.isKinematic = true;
        transform.SetParent(socket);

        if(isHeld)
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
        if(isHeld) return;

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
        isHeld = false;
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
        if(!isHeld)
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

    public string GetPrompt()
    {
        if (!isHeld) return "들기";
        if (IsNearFaucet) return "채우기";
        if (IsNearPlant) return "물 주기";
        return "내려놓기";
    }

    public bool TryGetWorldPose(out Vector3 pos, out Quaternion rot)
    {
        pos = transform.position; rot = transform.rotation;
        return !isHeld;
    }

    public void ApplyActivated(bool activated) {}

    public void ApplyHeld(bool held)
    {
        if (held) PlaceTo(PlayerManager.Instance.HeldPosition, true);
        else DropToStart();
    }

    public void ApplyWorldPose(Vector3 p, Quaternion r)
    {
        if (!isHeld) transform.SetPositionAndRotation(p, r);
    }
}
