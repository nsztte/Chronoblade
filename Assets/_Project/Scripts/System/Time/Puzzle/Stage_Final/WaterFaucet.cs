using UnityEngine;

public class WaterFaucet : MonoBehaviour, ITimeControllable, IRewindable
{
    [Header("참조")]
    [SerializeField] Transform canPosition;
    [SerializeField] WateringCan wateringCan;

    // private bool isWatering = false;
    private Animator animator;
    private float timeScale = 1f;
    private bool IsRewinding = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        TimeManager.Instance.RegisterControllable(this);    // 테스트 이후에 OnEnable로 옮길것
        TimeManager.Instance.RegisterRewindable(this);

        if(animator != null)
        {
            animator.SetFloat("Speed", 1f);
            animator.SetBool("IsRunning", true);
        }
    }

    private void OnEnable()
    {
        wateringCan.OnPlaced += OnPlaced;
    }

    private void OnDisable()
    {
        wateringCan.OnPlaced -= OnPlaced;

        TimeManager.Instance.UnregisterControllable(this);
        TimeManager.Instance.UnregisterRewindable(this);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && wateringCan.IsHeld())
        {
            wateringCan.IsNearFaucet = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player") && wateringCan.IsHeld())
        {
            wateringCan.IsNearFaucet = false;
        }
    }

    // 애니메이션 떨어지는 시점에 이벤트 등록
    public void AnimDrip()
    {
        if(!wateringCan.IsPlaced) return;

        wateringCan.AddDrop(!IsRewinding);
    }
    
    private void OnPlaced()
    {
        wateringCan.PlaceTo(canPosition, false);
    }

    #region 인터페이스 구현부
    public void SetTimeScale(float timeScale)
    {
        this.timeScale = timeScale;
        if(animator != null)
            animator.SetFloat("Speed", timeScale);
    }

    public float GetTimeScale()
    {
        return timeScale;
    }

    public void StartRewind()
    {
        IsRewinding = true;
        animator.SetFloat("Speed", -1);
    }

    public void StopRewind()
    {
        IsRewinding = false;
        animator.SetFloat("Speed", timeScale);
    }
    #endregion
}
