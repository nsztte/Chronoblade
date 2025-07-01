using System;
using System.Collections.Generic;
using UnityEngine;

public class ComboEvaluator : MonoBehaviour
{
    #region Singleton
    public static ComboEvaluator Instance { get; private set; }
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

    [Header("콤보 설정")]
    [SerializeField] private int maxComboBeats = 8; // 콤보 최대 비트 수

    public event Action<ComboSequence> OnComboMatched;
    public event Action<string> OnComboProgress;

    // 비트별 입력 버퍼
    private Queue<AttackType> beatInputBuffer = new Queue<AttackType>();
    private int currentBeatIndex = 0;
    private float lastComboTime = 0f;
    private bool inputRegisteredThisBeat = false;

    private void Start()
    {
        // InputManager에서 입력 발생 시 현재 비트에 기록
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnLightAttackPressed += () => RegisterInput(AttackType.Light);
            InputManager.Instance.OnHeavyAttackPressed += () => RegisterInput(AttackType.Heavy);
        }

        // TimingComboManager의 비트 루프에 맞춰 평가
        if (TimingComboManager.Instance != null)
        {
            TimingComboManager.Instance.OnBeat += OnBeat; // OnBeat는 각 비트마다 호출되는 이벤트라고 가정
        }
    }

    /// <summary>
    /// 비트마다 호출: 입력이 없으면 Rest로 기록, 입력이 있으면 해당 입력 기록
    /// </summary>
    private void OnBeat()
    {
        Debug.Log("지금!!!!");
        // 1. 먼저 현재 상태로 콤보 매칭 시도 (입력 타이밍 오차 없는 정밀한 평가)
        TryMatchCombo();

        // 2. 입력이 기록되지 않았다면 Rest로 기록
        if (!inputRegisteredThisBeat)
        {
            beatInputBuffer.Enqueue(AttackType.Rest);
        }
        inputRegisteredThisBeat = false;

        // 3. 콤보 윈도우 초과 시 초기화
        if (Time.time - lastComboTime > TimingComboManager.Instance.GetComboWindow())
        {
            beatInputBuffer.Clear();
            currentBeatIndex = 0;
            NotifyComboProgress();
            return;
        }

        // 4. 비트 인덱스 증가 및 진행 상황 갱신
        currentBeatIndex++;
        lastComboTime = Time.time;
        NotifyComboProgress();

        // 5. 최대 비트 수 초과 시 자동 슬라이딩
        if (beatInputBuffer.Count > maxComboBeats)
        {
            beatInputBuffer.Dequeue();
        }
    }

    /// <summary>
    /// 입력 발생 시 현재 비트에 기록
    /// </summary>
    public void RegisterInput(AttackType input)
    {
        // 이미 입력이 기록된 비트라면 무시
        if (inputRegisteredThisBeat) return;
        beatInputBuffer.Enqueue(input);
        inputRegisteredThisBeat = true;
        lastComboTime = Time.time;
    }

    /// <summary>
    /// 콤보 매칭 시도
    /// </summary>
    private void TryMatchCombo()
    {
        var weapon = WeaponManager.Instance?.CurrentWeapon;
        if (weapon == null || weapon.weaponData.weaponType != WeaponType.Sword)
            return;

        var availableCombos = weapon.weaponData.swordCombos;
        if (availableCombos == null || availableCombos.Count == 0)
            return;

        foreach (var combo in availableCombos)
        {
            if (IsComboMatch(combo))
            {
                Debug.Log($"[ComboEvaluator] 콤보 매칭 성공: {combo.comboName}");
                OnComboMatched?.Invoke(combo);
                beatInputBuffer.Clear();
                currentBeatIndex = 0;
                return;
            }
        }
        Debug.Log("[ComboEvaluator] 매칭되는 콤보 없음");
    }

    /// <summary>
    /// 입력 버퍼와 콤보 패턴이 일치하는지 확인
    /// </summary>
    private bool IsComboMatch(ComboSequence combo)
    {
        if (beatInputBuffer.Count < combo.attackSequence.Count)
            return false;

        var bufferArray = beatInputBuffer.ToArray();
        int startIdx = bufferArray.Length - combo.attackSequence.Count;

        for (int i = 0; i < combo.attackSequence.Count; i++)
        {
            if (combo.attackSequence[i].attackType != bufferArray[startIdx + i])
                return false;
        }
        return true;
    }

    /// <summary>
    /// 콤보 진행 상황 알림
    /// </summary>
    private void NotifyComboProgress()
    {
        if (beatInputBuffer.Count > 0)
        {
            var bufferArray = beatInputBuffer.ToArray();
            string progress = string.Join(" → ", Array.ConvertAll(bufferArray, GetAttackTypeString));
            OnComboProgress?.Invoke(progress);
        }
    }

    private string GetAttackTypeString(AttackType type)
    {
        switch (type)
        {
            case AttackType.Light: return "약";
            case AttackType.Heavy: return "강";
            case AttackType.Rest: return "쉬기";
            default: return "?";
        }
    }

    public void ClearInputBuffer()
    {
        beatInputBuffer.Clear();
        currentBeatIndex = 0;
        inputRegisteredThisBeat = false;
    }
} 