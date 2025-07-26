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

    [Header("퍼즐 클럭 파트")]
    [SerializeField] private ClockPart[] clockParts;

    // 파이널 퍼즐 전용
    public enum HandProgress { Hour, Minute, Second, Done }
    private HandProgress currentProgress = HandProgress.Hour;
    public HandProgress CurrentProgress => currentProgress;
    public BossPhase CurrentPhase => bossPhaseManager.CurrentPhase;

    private BossPhaseManager bossPhaseManager;

    private Animator animator;

    public event Action OnPuzzleSuccess;
    public event Action OnPuzzleFail;

    public bool IsPuzzleActive => isPuzzleActive;
    public bool IsPuzzleCleared => isPuzzleCleared;

    private void Awake()
    {
        bossPhaseManager = GetComponentInParent<BossPhaseManager>();

        animator = GetComponent<Animator>();

        clockParts = GetComponentsInChildren<ClockPart>();
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

        if(CurrentPhase == BossPhase.Puzzle1)
        {
            if(IsPuzzleSuccess())
            {
                PuzzleSuccess();
            }
        }
        else if(CurrentPhase == BossPhase.FinalPuzzle)
        {
            switch(currentProgress)
            {
                case HandProgress.Hour:
                    if(hourHand.IsAligned() && TimeManager.Instance.CurrentTimeState == TimeState.Stop)
                    {
                        currentProgress = HandProgress.Minute;
                        TimeManager.Instance.InitializeTimeState();
                    }
                    break;
                case HandProgress.Minute:
                    if(minuteHand.IsAligned() && TimeManager.Instance.CurrentTimeState == TimeState.Stop)
                    {
                        currentProgress = HandProgress.Second;
                        TimeManager.Instance.InitializeTimeState();
                    }
                    break;
                case HandProgress.Second:
                    if(secondHand.IsAligned() && TimeManager.Instance.CurrentTimeState == TimeState.Stop)
                    {
                        currentProgress = HandProgress.Done;
                        PuzzleSuccess();
                    }
                    break;
            }
        }
    }

    public void StartPuzzle()
    {
        if(CurrentPhase == BossPhase.FinalPuzzle)
        {
            currentProgress = HandProgress.Hour;
        }

        isPuzzleActive = true;
        isPuzzleCleared = false;
        remainingTime = puzzleTimeLimit;

        Debug.Log("[PuzzleClockManager] 퍼즐 시작");
    }

    public void SetClockPartsTarget(bool isPlayer)
    {
        foreach(var clockPart in clockParts)
        {
            clockPart.SetTarget(isPlayer);
        }
    }

    public bool AreAllPartsArrived()
    {
        foreach (ClockPart clockPart in clockParts)
        {
            if (!clockPart.HasArrived) return false;
        }

        return true;
    }

    public void LaunchAllClockParts()
    {
        foreach(ClockPart clockPart in clockParts)
        {
            clockPart.Launch();
        }
    }

    public void ResetAllClockParts()
    {
        foreach(ClockPart clockPart in clockParts)
        {
            clockPart.ForceReset();
        }
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
