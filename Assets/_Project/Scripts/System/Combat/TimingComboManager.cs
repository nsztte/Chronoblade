using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class TimingComboManager : MonoBehaviour
{
    [Header("리듬 설정")]
    [SerializeField] private float beatInterval = 0.5f;   // 리듬 템포 (예: 120 BPM = 0.5초 간격)
    [SerializeField] private float perfectWindow = 0.1f;
    [SerializeField] private float goodWindow = 0.25f;

    [Header("보너스 배율")]
    [SerializeField] private float perfectBonusMultiplier = 1.2f;
    [SerializeField] private float goodBonusMultiplier = 1.0f;

    [Header("입력 유효 시간")]
    [SerializeField] private float inputValidTime = 1.0f; // 입력 유효 시간(초)

    public enum TimingResult { Perfect, Good, Miss, None }
    public event Action<TimingResult> OnTimingJudged;

    private float startTime;
    private List<float> inputTimes = new(); // 입력 시각들 저장 (연타 대응)

    private float lastBeatTime;

    private void Start()
    {
        startTime = Time.time; // 비트 기준 시점 설정
        lastBeatTime = Time.time;
        StartCoroutine(BeatRoutine());
        // InputManager 이벤트 구독
        InputManager.Instance.OnAttackPressed += OnAttackInput;
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnAttackPressed -= OnAttackInput;
        }
    }

    private void OnAttackInput()
    {
        inputTimes.Add(Time.time);
    }

    private IEnumerator BeatRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(beatInterval);
            EvaluateInputs();
        }
    }

    private void EvaluateInputs()
    {
        if (inputTimes.Count == 0) return;

        float currentTime = Time.time;
        float beatsPassed = Mathf.Round((currentTime - startTime) / beatInterval);
        float nearestBeatTime = startTime + beatsPassed * beatInterval;

        List<float> toRemove = new();

        foreach (float inputTime in inputTimes)
        {
            // 유효 시간 초과 입력은 바로 제거
            if (currentTime - inputTime > inputValidTime)
            {
                toRemove.Add(inputTime);
                continue;
            }

            float offset = inputTime - nearestBeatTime;
            float absOffset = Mathf.Abs(offset);

            TimingResult result;
            if (absOffset <= perfectWindow)
                result = TimingResult.Perfect;
            else if (absOffset <= goodWindow)
                result = TimingResult.Good;
            else
                result = TimingResult.Miss;

            Debug.Log($"[TimingComboManager] inputTime: {inputTime:F3}, beatTime: {nearestBeatTime:F3}, offset: {offset:F3}, result: {result}");

            OnTimingJudged?.Invoke(result);
            toRemove.Add(inputTime);
        }

        // 사용한 입력 제거
        foreach (float inputTime in toRemove)
        {
            inputTimes.Remove(inputTime);
        }
    }

    public float GetCurrentOffset()
    {
        float currentTime = Time.time;
        float beatsPassed = Mathf.Round((currentTime - startTime) / beatInterval);
        float nearestBeatTime = startTime + beatsPassed * beatInterval;
        return currentTime - nearestBeatTime;
    }
}
