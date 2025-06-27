using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private readonly List<ITimeControllable> controllables = new();

    [Range(0, 1)] public float slowFactor = 0.01f;
    public bool isSlowingTime = false;

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

    public void Register(ITimeControllable controllable)
    {
        if (!controllables.Contains(controllable))
        {
            controllables.Add(controllable);
        }
    }

    public void Unregister(ITimeControllable controllable)
    {
        if (controllables.Contains(controllable))
        {
            controllables.Remove(controllable);
        }
    }
    
    public void ApplySlowMotion(bool enable)
    {
        isSlowingTime = enable;

        float targetTimeScale = enable ? slowFactor : 1f;

        foreach (var controllable in controllables)
        {
            controllable.SetTimeScale(targetTimeScale);
        }
    }

    private void Start()
    {
        // TimeInputHandler 이벤트 구독
        TimeInputHandler.Instance.OnTimeSlowToggle += OnTimeSlowToggle;
        TimeInputHandler.Instance.OnTimeRewind += OnTimeRewind;
        TimeInputHandler.Instance.OnTimeStop += OnTimeStop;
        TimeInputHandler.Instance.OnTimeFastForward += OnTimeFastForward;
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제
        if (TimeInputHandler.Instance != null)
        {
            TimeInputHandler.Instance.OnTimeSlowToggle -= OnTimeSlowToggle;
            TimeInputHandler.Instance.OnTimeRewind -= OnTimeRewind;
            TimeInputHandler.Instance.OnTimeStop -= OnTimeStop;
            TimeInputHandler.Instance.OnTimeFastForward -= OnTimeFastForward;
        }
    }

    #region Time Control Event Handlers
    private void OnTimeSlowToggle()
    {
        Debug.Log($"[TimeManager] OnTimeSlowToggle 호출됨. 현재 상태: {isSlowingTime}");
        ApplySlowMotion(!isSlowingTime);
        Debug.Log($"[TimeManager] 시간 슬로우 토글 완료: {(isSlowingTime ? "활성화" : "비활성화")}");
        Debug.Log($"[TimeManager] 등록된 객체 수: {controllables.Count}");
    }

    private void OnTimeRewind()
    {
        Debug.Log("시간 되감기 실행");
        // TODO: 시간 되감기 로직 구현
    }

    private void OnTimeStop()
    {
        Debug.Log("시간 정지 실행");
        // TODO: 시간 정지 로직 구현
    }

    private void OnTimeFastForward()
    {
        Debug.Log("시간 빨리감기 실행");
        // TODO: 시간 빨리감기 로직 구현
    }
    #endregion
}
