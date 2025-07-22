using UnityEngine;
using System;

public class PuzzleClockManager : MonoBehaviour
{
    [Header("퍼즐 바늘 참조")]
    [SerializeField] private PuzzleHand hourHand;
    [SerializeField] private PuzzleHand minuteHand;
    [SerializeField] private PuzzleHand secondHand;

    [Header("퍼즐 시간 설정")]
    [SerializeField] private float puzzleTimeLimit = 25f;

    [Tooltip("퍼즐 남은 시간")]
    [SerializeField] private float remainingTime;
    [SerializeField] private bool isPuzzleActive = false;
    [SerializeField] private bool isPuzzleCleared = false;

    private Animator animator;

    public event Action OnPuzzleSuccess;
    public event Action OnPuzzleFail;

    public bool IsPuzzleActive => isPuzzleActive;
    public bool IsPuzzleCleared => isPuzzleCleared;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(!isPuzzleActive || isPuzzleCleared) return;

        // 애니메이션 속도 조절
        UpdateAnimationSpeed();

        remainingTime -= Time.deltaTime;

        // TODO: 잔여 시간에 따른 화면 연출 (화면 흔들림 트리거)

        if(remainingTime <= 0f)
        {
            PuzzleFail();
            return;
        }

        if(IsPuzzleSuccess())
        {
            PuzzleSuccess();
        }
    }

    public void StartPuzzle()
    {
        isPuzzleActive = true;
        isPuzzleCleared = false;
        remainingTime = puzzleTimeLimit;

        Debug.Log("[PuzzleClockManager] 퍼즐 시작");
    }

    private bool IsPuzzleSuccess()
    {
        return hourHand.IsAligned() && minuteHand.IsAligned() && secondHand.IsAligned()
            && TimeManager.Instance.CurrentTimeState == TimeState.Stop;
    }

    private void PuzzleSuccess()
    {
        isPuzzleCleared = true;
        isPuzzleActive = false;

        Debug.Log("[PuzzleClockManager] 퍼즐 성공");
        TimeManager.Instance.InitializeTimeState();

        OnPuzzleSuccess?.Invoke();
    }

    private void PuzzleFail()
    {
        isPuzzleCleared = true;
        isPuzzleActive = false;

        Debug.Log("[PuzzleClockManager] 퍼즐 실패");
        TimeManager.Instance.InitializeTimeState();

        OnPuzzleFail?.Invoke();
    }

    private void UpdateAnimationSpeed()
    {
        float speed = TimeManager.Instance.CurrentTimeState switch
        {
            TimeState.Normal => 1f,
            TimeState.Slow => TimeManager.Instance.SlowFactor,
            TimeState.Stop => 0f,
            TimeState.Rewind => -1f,
            TimeState.FastForward => TimeManager.Instance.FastForwardFactor,
            _ => 1f
        };

        animator.SetFloat("Speed", speed);
    }
}
