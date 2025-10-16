using System;
using UnityEngine;
using System.Collections;

public class TimingComboManager : MonoBehaviour
{
    #region Singleton
    public static TimingComboManager Instance { get; private set; }

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

    [Header("리듬 설정")]
    [SerializeField] private float beatInterval = 0.5f;   // 리듬 템포 (예: 120 BPM = 0.5초 간격)
    [SerializeField] private float perfectWindow = 0.06f; // Perfect 판정 윈도우 (40ms)
    [SerializeField] private float goodWindow = 0.12f;    // Good 판정 윈도우 (80ms)
    public float BeatInterval => beatInterval;
    public float PerfectWindow => perfectWindow;
    public float GoodWindow => goodWindow;

    [Header("보너스 배율")]
    [SerializeField] private float perfectBonusMultiplier = 1.5f;
    [SerializeField] private float goodBonusMultiplier = 1.2f;
    [SerializeField] public float missPenaltyMultiplier = 1.0f;   // Miss 배율 (콤보 끊김 시)
    public float PerfectBonusMultiplier => perfectBonusMultiplier;
    public float GoodBonusMultiplier => goodBonusMultiplier;

    [Header("입력 유효 시간")]
    [SerializeField] private float inputValidTime = 1.0f; // 입력 유효 시간(초)

    public enum TimingResult { Perfect, Good, Miss, Unavailable, None }
    
    // 심장박동 UI, 사운드에 사용
    public event Action OnBeat; // 비트마다 콤보 시스템에 알림
    private float startTime;    // 비트 루프 시작 시점 (코루틴 시작 시점)
    private Coroutine beatRoutineCoroutine;
    private bool beatStarted = false;
    public float StartTime => startTime; // 외부에서 접근 가능한 프로퍼티
    public bool IsBeatStarted => beatStarted; // 비트 루틴 시작 상태 확인

    // UI 등 피드백용 프로퍼티, 이벤트
    public bool IsMissed { get; private set; }
    public bool IsPerfect { get; private set; }
    public bool IsGood { get; private set; }

    public event Action OnMissed;
    public event Action OnPerfect;
    public event Action OnGood;

    private Action onPerfectHandler;
    private Action onGoodHandler;
    private Action onMissedHandler;

    private void Start()
    {
        onPerfectHandler = () => UIManager.Instance.ComboResultUIController.ShowResult("PERFECT", Color.yellow);
        onGoodHandler    = () => UIManager.Instance.ComboResultUIController.ShowResult("Good", Color.green);
        onMissedHandler  = () => UIManager.Instance.ComboResultUIController.ShowResult("Miss", Color.red);

        OnPerfect += onPerfectHandler;
        OnGood    += onGoodHandler;
        OnMissed  += onMissedHandler;
    }

    private void OnDestroy()
    {
        OnPerfect -= onPerfectHandler;
        OnGood    -= onGoodHandler;
        OnMissed  -= onMissedHandler;
    }

    // 외부에서 Miss 피드백만 명시적으로 요청해야 하는 경우 사용
    public void MissFeedback()
    {
        OnMissed?.Invoke();
    }

    public void StartBeatRoutine()
    {
        if (!beatStarted)
        {
            beatStarted = true;
            startTime = Time.time; // 비트 루프 시작 시점 초기화
            beatRoutineCoroutine = StartCoroutine(BeatRoutine());
            UIManager.Instance?.HeartbeatScroller.Show();
        }
    }

    public void StopBeatRoutine()
    {
        if (beatRoutineCoroutine != null)
        {
            StopCoroutine(beatRoutineCoroutine);
            beatRoutineCoroutine = null;
            UIManager.Instance?.HeartbeatScroller.Hide();
        }
        beatStarted = false;
    }

    private IEnumerator BeatRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(beatInterval);
            OnBeat?.Invoke(); // 비트마다 알림 (UI, 사운드 호출 용도)
        }
    }

    // public float GetCurrentOffset()
    // {
    //     float currentTime = Time.time;
    //     float beatsPassed = Mathf.Round((currentTime - startTime) / beatInterval);
    //     float nearestBeatTime = startTime + beatsPassed * beatInterval;
    //     return currentTime - nearestBeatTime;
    // }

    public float GetComboWindow()
    {
        return inputValidTime; // 콤보 입력 유효 시간 반환
    }

    public (TimingResult result, float damageMultiplier, float absOffset) JudgeTiming(float inputTime, bool emitEvents = true)
    {
        // 비트 루틴이 시작되지 않았으면 Unavailable 판정
        if (!beatStarted)
        {
            IsPerfect = false; IsGood = false; IsMissed = false;
            return (TimingResult.Unavailable, 1.0f, 0f);
        }

        float beatsPassed = Mathf.Round((inputTime - StartTime) / BeatInterval);
        float nearestBeatTime = StartTime + beatsPassed * BeatInterval;
        float offset = inputTime - nearestBeatTime;
        float absOffset = Mathf.Abs(offset);

        TimingResult result;
        float damageMultiplier;
        if (absOffset <= PerfectWindow)
        {
            result = TimingResult.Perfect;
            damageMultiplier = PerfectBonusMultiplier;
            IsPerfect = true; IsGood = false; IsMissed = false;
            if (emitEvents) OnPerfect?.Invoke();
        }
        else if (absOffset <= GoodWindow)
        {
            result = TimingResult.Good;
            damageMultiplier = GoodBonusMultiplier;
            IsPerfect = false; IsGood = true; IsMissed = false;
            if (emitEvents) OnGood?.Invoke();
        }
        else
        {
            result = TimingResult.Miss;
            damageMultiplier = missPenaltyMultiplier;
            IsPerfect = false; IsGood = false; IsMissed = true;
            if (emitEvents) OnMissed?.Invoke();
        }
        return (result, damageMultiplier, absOffset);
    }
}
