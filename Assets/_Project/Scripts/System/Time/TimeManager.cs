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

    [Range(0, 1)] [SerializeField] private float slowFactor = 0.01f;
    [Range(0, 1)] [SerializeField] private float fastForwardFactor = 1.8f;

    #region Singleton
    public static TimeManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

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

    private void OnDisable()
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

    #region Time Control Event Handlers
    private void OnTimeSlowToggle()
    {
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
    }

    private void OnTimeStop()
    {
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
    }

    private void OnTimeRewindStart()
    {
        if(currentTimeState == TimeState.Rewind) return;

        currentTimeState = TimeState.Rewind;
        Debug.Log($"[TimeManager] 시간 되감기 실행");

        foreach(var rewindable in rewindables)
        {
            rewindable.StartRewind();
        }
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
    }

    private void OnTimeFastForwardStart()
    {
        if(currentTimeState == TimeState.FastForward) return;
        
        currentTimeState = TimeState.FastForward;
        ApplyTimeScale(fastForwardFactor);
        Debug.Log($"[TimeManager] 시간 빨리감기 실행");
    }

    private void OnTimeFastForwardEnd()
    {
        if(currentTimeState != TimeState.FastForward) return;
        
        currentTimeState = TimeState.Normal;
        ApplyTimeScale(1f);
        Debug.Log($"[TimeManager] 시간 빨리감기 해제");
    }

    private void ApplyTimeScale(float timeScale)
    {
        foreach (var controllable in controllables)
        {
            controllable.SetTimeScale(timeScale);
        }
    }
    #endregion
}
