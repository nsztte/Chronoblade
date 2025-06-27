using UnityEngine;
using System;

public class TimeInputHandler : MonoBehaviour
{
    #region Singleton
    public static TimeInputHandler Instance { get; private set; }

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

    #region Events
    public event Action OnTimeSlowToggle;      // Q 탭
    public event Action OnTimeRewind;          // Q 홀드
    public event Action OnTimeStop;            // E 탭
    public event Action OnTimeFastForward;     // E 홀드
    #endregion

    // 시간 조절 입력 상태
    private bool isQKeyPressed = false;
    private bool isEKeyPressed = false;
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
        }
        
        if (Input.GetKey(KeyCode.Q) && isQKeyPressed)
        {
            qKeyHoldTime += Time.deltaTime;
            
            // Q 키 홀드 시 시간 되감기
            if (qKeyHoldTime >= holdThreshold)
            {
                OnTimeRewind?.Invoke();
            }
        }
        
        if (Input.GetKeyUp(KeyCode.Q) && isQKeyPressed)
        {
            // Q 키 탭 시 시간 슬로우 토글
            if (qKeyHoldTime < holdThreshold)
            {
                OnTimeSlowToggle?.Invoke();
            }
            
            isQKeyPressed = false;
            qKeyHoldTime = 0f;
        }

        // E 키 입력 처리
        if (Input.GetKeyDown(KeyCode.E))
        {
            isEKeyPressed = true;
            eKeyHoldTime = 0f;
        }
        
        if (Input.GetKey(KeyCode.E) && isEKeyPressed)
        {
            eKeyHoldTime += Time.deltaTime;
            
            // E 키 홀드 시 시간 빨리감기
            if (eKeyHoldTime >= holdThreshold)
            {
                OnTimeFastForward?.Invoke();
            }
        }
        
        if (Input.GetKeyUp(KeyCode.E) && isEKeyPressed)
        {
            // E 키 탭 시 시간 정지
            if (eKeyHoldTime < holdThreshold)
            {
                OnTimeStop?.Invoke();
            }
            
            isEKeyPressed = false;
            eKeyHoldTime = 0f;
        }
    }
} 