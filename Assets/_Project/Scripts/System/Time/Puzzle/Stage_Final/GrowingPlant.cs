using UnityEngine;

public class GrowingPlant : MonoBehaviour, ITimeControllable, IRewindable
{
    [Header("참조")]
    [SerializeField] private PuzzleRoom4Manager puzzleManager;
    [SerializeField] private WateringCan wateringCan;
    [SerializeField] private float fastforwardMultiplier = 10;

    private bool isWaterd = false;
    
    private float timeScale = 1f;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        TimeManager.Instance.RegisterControllable(this);    // 테스트 이후에 OnEnable로 옮길것
        TimeManager.Instance.RegisterRewindable(this);
    }

    private void OnEnable()
    {
        // TimeManager.Instance.RegisterControllable(this);    // 테스트 이후에 OnEnable로 옮길것
        // TimeManager.Instance.RegisterRewindable(this);

        wateringCan.OnWatered += OnWatered;
    }

    private void OnDisable()
    {
        wateringCan.OnWatered -= OnWatered;

        TimeManager.Instance.UnregisterControllable(this);
        TimeManager.Instance.UnregisterRewindable(this);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && wateringCan.IsHeld)
        {
            wateringCan.IsNearPlant = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player") && wateringCan.IsHeld)
        {
            wateringCan.IsNearPlant = false;
        }
    }

    private void OnWatered()
    {
        if(!wateringCan.IsFull) return;
        if(isWaterd) return;

        isWaterd = true;
        animator.SetTrigger("Growing");
        animator.SetFloat("Speed", timeScale);
    }

    public void ChangeManagerState()
    {
        puzzleManager.ChangeIsCleared();
    }

    public void SetTimeScale(float timeScale)
    {
        this.timeScale = timeScale;
        float animSpeed = timeScale > 1.5f ? timeScale * fastforwardMultiplier : timeScale;
        animator.SetFloat("Speed", animSpeed);
    }

    public float GetTimeScale()
    {
        return timeScale;
    }

    public void StartRewind()
    {
        animator.SetFloat("Speed", -1);
    }

    public void StopRewind()
    {
        animator.SetFloat("Speed", timeScale);
    }
}
