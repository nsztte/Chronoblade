using UnityEngine;
using System;

public class TimeInputHandler : MonoBehaviour
{
    #region Singleton
    public static TimeInputHandler Instance { get; private set; }

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

    #region Events
    public event Action OnTimeSlowToggle;      // Q 탭
    public event Action OnTimeStop;            // E 탭
    public event Action OnTimeRewindStart;     // Q 키 누르기
    public event Action OnTimeRewindEnd;       // Q 키 떼기
    public event Action OnTimeFastForwardStart;     // E 키 누르기
    public event Action OnTimeFastForwardEnd;     // E 키 떼기
    #endregion

    // 시간 조절 입력 상태
    private bool isQKeyPressed = false;
    private bool isEKeyPressed = false;
    private bool isRewindInvoked = false;
    private bool isFastForwardInvoked = false;
    private float qKeyHoldTime = 0f;
    private float eKeyHoldTime = 0f;
    private const float holdThreshold = 0.3f; // 홀드 판정 시간 (초)

    private void Update()
    {
        HandleTimeControls();
    }
    
    private void HandleTimeControls()
    {
        // Q 키 입력 처리
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isQKeyPressed = true;
            qKeyHoldTime = 0f;
            isRewindInvoked = false;

            if (isFastForwardInvoked)
            {
                OnTimeFastForwardEnd?.Invoke();
                isEKeyPressed = false;
                eKeyHoldTime = 0f;
                isFastForwardInvoked = false;
            }
        }
        
        // Q 키 홀드 처리
        if (Input.GetKey(KeyCode.Q) && isQKeyPressed)
        {
            qKeyHoldTime += Time.deltaTime;
            
            // Q 키 홀드 시 시간 되감기
            if (qKeyHoldTime >= holdThreshold && !isRewindInvoked)
            {
                OnTimeRewindStart?.Invoke();
                isRewindInvoked = true;
            }
        }
        
        // Q 키 탭 처리
        if (Input.GetKeyUp(KeyCode.Q) && isQKeyPressed)
        {
            // Q 키 탭 시 시간 슬로우 토글
            if (qKeyHoldTime < holdThreshold)
            {
                OnTimeSlowToggle?.Invoke();
            }
            else if(isRewindInvoked)
            {
                OnTimeRewindEnd?.Invoke();
            }
            
            isQKeyPressed = false;
            qKeyHoldTime = 0f;
            isRewindInvoked = false;
        }

        // E 키 입력 처리
        if (Input.GetKeyDown(KeyCode.E))
        {
            isEKeyPressed = true;
            eKeyHoldTime = 0f;
            isFastForwardInvoked = false;

            if (isRewindInvoked)
            {
                OnTimeRewindEnd?.Invoke();
                isQKeyPressed = false;
                qKeyHoldTime = 0f;
                isRewindInvoked = false;
            }
        }
        
        if (Input.GetKey(KeyCode.E) && isEKeyPressed)
        {
            eKeyHoldTime += Time.deltaTime;
            
            // E 키 홀드 시 시간 빨리감기
            if (eKeyHoldTime >= holdThreshold && !isFastForwardInvoked)
            {
                OnTimeFastForwardStart?.Invoke();
                isFastForwardInvoked = true;
            }
        }
        
        if (Input.GetKeyUp(KeyCode.E) && isEKeyPressed)
        {
            // E 키 탭 시 시간 정지
            if (eKeyHoldTime < holdThreshold)
            {
                OnTimeStop?.Invoke();
            }
            else if(isFastForwardInvoked)
            {
                OnTimeFastForwardEnd?.Invoke();
            }
            
            isEKeyPressed = false;
            eKeyHoldTime = 0f;
            isFastForwardInvoked = false;
        }
    }
} 