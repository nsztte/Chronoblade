using System.Collections.Generic;
using UnityEngine;

public enum TimeState
{
    Normal,
    Slow,
    Stop,
    Rewind,
    FastForward
}

public class TimeManager : MonoBehaviour
{
    private readonly List<ITimeControllable> controllables = new();
    private readonly List<IRewindable> rewindables = new();
    [SerializeField] private TimeState currentTimeState = TimeState.Normal;

    [Header("배속")]
    [Range(0, 1)] [SerializeField] private float slowFactor = 0.01f;
    [Range(0, 5)] [SerializeField] private float fastForwardFactor = 1.8f;


    [Header("시간 스킬 MP 소모량(초당)")]
    [SerializeField] private float rewindMpDrain = 30f;
    [SerializeField] private float stopMpDrain = 25f;
    [SerializeField] private float slowMpDrain = 10f;
    [SerializeField] private float fastForwardMpDrain = 8f;

    // 기본 잠금 상태
    [Header("시간 스킬 해금 상태")]
    [SerializeField] private bool unlockedSlow = false;
    [SerializeField] private bool unlockedStop = false;
    [SerializeField] private bool unlockedRewind = false;
    [SerializeField] private bool unlockedFastForward = false;

    public float SlowFactor => slowFactor;
    public float FastForwardFactor => fastForwardFactor;

    #region Singleton
    public static TimeManager Instance { get; private set; }

    public void Initialize()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning($"{GetType().Name} 인스턴스 감지됨, 초기화 스킵");
            return;
        }
        Instance = this;
    }
    #endregion

    private void Start()
    {
        // TimeInputHandler 이벤트 구독
        TimeInputHandler.Instance.OnTimeSlowToggle += OnTimeSlowToggle;
        TimeInputHandler.Instance.OnTimeRewindStart += OnTimeRewindStart;
        TimeInputHandler.Instance.OnTimeRewindEnd += OnTimeRewindEnd;
        TimeInputHandler.Instance.OnTimeStop += OnTimeStop;
        TimeInputHandler.Instance.OnTimeFastForwardStart += OnTimeFastForwardStart;
        TimeInputHandler.Instance.OnTimeFastForwardEnd += OnTimeFastForwardEnd;
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제
        if (TimeInputHandler.Instance != null)
        {
            TimeInputHandler.Instance.OnTimeSlowToggle -= OnTimeSlowToggle;
            TimeInputHandler.Instance.OnTimeRewindStart -= OnTimeRewindStart;
            TimeInputHandler.Instance.OnTimeRewindEnd -= OnTimeRewindEnd;
            TimeInputHandler.Instance.OnTimeStop -= OnTimeStop;
            TimeInputHandler.Instance.OnTimeFastForwardStart -= OnTimeFastForwardStart;
            TimeInputHandler.Instance.OnTimeFastForwardEnd -= OnTimeFastForwardEnd;
        }
    }

    private void Update()
    {
        DrainMpPerTime();
    }

    private void DrainMpPerTime()
    {
        if (PlayerManager.Instance == null) return;

        float drain = 0f;
        switch (currentTimeState)
        {
            case TimeState.Rewind:
                drain = rewindMpDrain;
                break;
            case TimeState.Stop:
                drain = stopMpDrain;
                break;
            case TimeState.Slow:
                drain = slowMpDrain;
                break;
            case TimeState.FastForward:
                drain = fastForwardMpDrain;
                break;
        }

        if (drain > 0f)
        {
            PlayerManager.Instance.UseMP(drain * Time.deltaTime);

            if (PlayerManager.Instance.CurrentMP <= 0)
            {
                // MP가 바닥나면 자동 해제
                SetTimeState(TimeState.Normal);
            }
        }
    }

    #region 시간 스킬 해금 상태 설정
    public void UnlockTimeSkill(TimeState skill)
    {
        switch (skill)
        {
            case TimeState.Slow:        unlockedSlow = true; break;
            case TimeState.Stop:        unlockedStop = true; break;
            case TimeState.Rewind:      unlockedRewind = true; break;
            case TimeState.FastForward: unlockedFastForward = true; break;
        }
    }

    private bool IsUnlocked(TimeState skill)
    {
        return skill switch {
            TimeState.Slow        => unlockedSlow,
            TimeState.Stop        => unlockedStop,
            TimeState.Rewind      => unlockedRewind,
            TimeState.FastForward => unlockedFastForward,
            _ => true
        };
    }
    #endregion

    #region 타임 컨트롤 등록 및 해제
    public void RegisterControllable(ITimeControllable controllable)
    {
        if (!controllables.Contains(controllable))
        {
            controllables.Add(controllable);
        }
    }

    public void UnregisterControllable(ITimeControllable controllable)
    {
        if (controllables.Contains(controllable))
        {
            controllables.Remove(controllable);
        }
    }

    public void RegisterRewindable(IRewindable rewindable)
    {
        if (!rewindables.Contains(rewindable))
        {
            rewindables.Add(rewindable);
        }
    }
    
    public void UnregisterRewindable(IRewindable rewindable)
    {
        if (rewindables.Contains(rewindable))
        {
            rewindables.Remove(rewindable);
        }
    }
    #endregion

    #region 타임 스케일 설정 및 초기화, 게임스테이트 연동
    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    // 보스 전용 시간 제어 함수
    public void SetTimeStop(bool isTimeStop)
    {
        if(isTimeStop)
        {
            foreach(var controllable in controllables)
            {
                controllable.SetTimeScale(0f);
            }
        }
        else
        {
            foreach(var controllable in controllables)
            {
                controllable.SetTimeScale(1f);
            }
        }
    }

    public void InitializeTimeState()
    {
        if (currentTimeState == TimeState.Normal) return;
        
        if (IsTimeSkillAllowed(currentTimeState))
        {
            currentTimeState = TimeState.Normal;
            ApplyTimeScale(1f);
            UpdateTimeUI();
        }
    }

    public bool IsTimeSkillAllowed(TimeState timeState)
    {
        if (!IsUnlocked(timeState))
        {
            UIManager.Instance.ShowToast($"{timeState} 스킬 해금이 필요합니다.");
            return false;
        }

        var state = GameManager.Instance.CurrentGameState;

        if (state is PuzzleState) return true;
        if (state is CombatState)
        {
            return timeState != TimeState.Rewind && timeState != TimeState.FastForward;
        }
        if (state is ExplorationState) return false;

        return false;
    }

    public void SetTimeState(TimeState timeState)
    {
        currentTimeState = timeState;
        
        switch(timeState)
        {
            case TimeState.Normal:
                ApplyTimeScale(1f);
                break;
            case TimeState.Slow:
                ApplyTimeScale(slowFactor);
                break;
            case TimeState.Stop:
                ApplyTimeScale(0f);
                break;
            case TimeState.Rewind:
                foreach(var rewindable in rewindables)
                {
                    rewindable.StartRewind();
                }
                break;
            case TimeState.FastForward:
                ApplyTimeScale(fastForwardFactor);
                break;
            default:
                break;
        }

        UpdateTimeUI();
    }

    public TimeState CurrentTimeState => currentTimeState;
    #endregion

    #region 타임 컨트롤 이벤트 핸들러
    private void OnTimeSlowToggle()
    {
        if(!IsTimeSkillAllowed(TimeState.Slow)) return;

        if(currentTimeState == TimeState.Slow)
        {
            currentTimeState = TimeState.Normal;
            ApplyTimeScale(1f);
            Debug.Log($"[TimeManager] 시간 슬로우 해제");
        }
        else
        {
            currentTimeState = TimeState.Slow;
            ApplyTimeScale(slowFactor);
            Debug.Log($"[TimeManager] 시간 슬로우 실행");
        }

        UpdateTimeUI();
    }

    private void OnTimeStop()
    {
        if(!IsTimeSkillAllowed(TimeState.Stop)) return;

        if(currentTimeState == TimeState.Stop)
        {
            currentTimeState = TimeState.Normal;
            ApplyTimeScale(1f);
            Debug.Log($"[TimeManager] 시간 정지 해제");
        }
        else
        {
            currentTimeState = TimeState.Stop;
            ApplyTimeScale(0f);
            Debug.Log($"[TimeManager] 시간 정지 실행");
        }

        UpdateTimeUI();
    }

    private void OnTimeRewindStart()
    {
        if(!IsTimeSkillAllowed(TimeState.Rewind)) return;
        if(currentTimeState == TimeState.Rewind) return;

        currentTimeState = TimeState.Rewind;
        Debug.Log($"[TimeManager] 시간 되감기 실행");

        foreach(var rewindable in rewindables)
        {
            rewindable.StartRewind();
        }

        UpdateTimeUI();
    }

    private void OnTimeRewindEnd()
    {
        if(currentTimeState != TimeState.Rewind) return;
        
        currentTimeState = TimeState.Normal;
        Debug.Log($"[TimeManager] 시간 되감기 해제");

        foreach(var rewindable in rewindables)
        {
            rewindable.StopRewind();
        }

        UpdateTimeUI();
    }

    private void OnTimeFastForwardStart()
    {
        if(!IsTimeSkillAllowed(TimeState.FastForward)) return;
        if(currentTimeState == TimeState.FastForward) return;
        
        currentTimeState = TimeState.FastForward;
        ApplyTimeScale(fastForwardFactor);
        Debug.Log($"[TimeManager] 시간 빨리감기 실행");

        UpdateTimeUI();
    }

    private void OnTimeFastForwardEnd()
    {
        if(currentTimeState != TimeState.FastForward) return;
        
        currentTimeState = TimeState.Normal;
        ApplyTimeScale(1f);
        Debug.Log($"[TimeManager] 시간 빨리감기 해제");

        UpdateTimeUI();
    }

    private void ApplyTimeScale(float timeScale)
    {
        foreach (var controllable in controllables)
        {
            controllable.SetTimeScale(timeScale);
        }
    }
    #endregion

    #region UI 동기화
    private void UpdateTimeUI()
    {
        switch (currentTimeState)
        {
            case TimeState.Normal:
                // UIManager.Instance?.ClearTimeState();
                UIManager.Instance?.ShowTimeState(TimeState.Normal);
                break;
            case TimeState.Slow:
                UIManager.Instance?.ShowTimeState(TimeState.Slow);
                break;
            case TimeState.Stop:
                UIManager.Instance?.ShowTimeState(TimeState.Stop);
                break;
            case TimeState.Rewind:
                UIManager.Instance?.ShowTimeState(TimeState.Rewind);
                break;
            case TimeState.FastForward:
                UIManager.Instance?.ShowTimeState(TimeState.FastForward);
                break;
        }
    }
    #endregion
}
