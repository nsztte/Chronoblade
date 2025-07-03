using System;
using UnityEngine;
using System.Collections.Generic;

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
    [SerializeField] private float comboWindow = 1.0f; // 콤보 입력 유효 시간

    // 이벤트
    public event Action<ComboSequence, int, ComboAttackData> OnComboAttackExecuted; // 콤보 공격 실행
    public event Action<ComboSequence> OnComboCompleted; // 콤보 완성
    public event Action<ComboSequence> OnComboFailed; // 콤보 실패
    public event Action<AttackType> OnNormalAttackExecuted; // 일반 공격 실행

    // 진행형 콤보 관련 변수
    private bool isComboInProgress = false; // 콤보 진행 중 플래그
    private ComboSequence currentCombo = null; // 현재 진행 중인 콤보
    private int currentComboStep = 0; // 현재 콤보 단계
    private float lastInputTime = 0f; // 마지막 입력 시간

    private void Start()
    {
        // InputManager에서 입력 발생 시 콤보 평가
        /*
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnLightAttackPressed += () => RegisterInput(AttackType.Light);
            InputManager.Instance.OnHeavyAttackPressed += () => RegisterInput(AttackType.Heavy);
        }
        */
        // TimingComboManager의 비트 이벤트 구독
        if (TimingComboManager.Instance != null)
        {
            TimingComboManager.Instance.OnBeat += OnBeat;
        }
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제
        /*
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnLightAttackPressed -= () => RegisterInput(AttackType.Light);
            InputManager.Instance.OnHeavyAttackPressed -= () => RegisterInput(AttackType.Heavy);
        }
        */
        if (TimingComboManager.Instance != null)
        {
            TimingComboManager.Instance.OnBeat -= OnBeat;
        }
    }

    /// <summary>
    /// 콤보 타이밍 체크 (외부에서 호출)
    /// </summary>
    public void CheckComboTiming()
    {
        if (!isComboInProgress) return;
        
        // 콤보 윈도우 초과 시 콤보 실패
        if (Time.time - lastInputTime > comboWindow)
        {
            Debug.Log("[ComboEvaluator] 콤보 실패 - 타이밍 초과");
            OnComboFailed?.Invoke(currentCombo);
            ResetCombo();
        }
    }

    /// <summary>
    /// 콤보 리셋
    /// </summary>
    private void ResetCombo()
    {
        isComboInProgress = false;
        currentCombo = null;
        currentComboStep = 0;
    }

    /// <summary>
    /// 콤보 진행 중인지 확인
    /// </summary>
    public bool IsComboInProgress => isComboInProgress;

    /// <summary>
    /// 현재 진행 중인 콤보 정보
    /// </summary>
    public ComboSequence CurrentCombo => currentCombo;

    /// <summary>
    /// 현재 콤보 단계
    /// </summary>
    public int CurrentComboStep => currentComboStep;

    /// <summary>
    /// 비트마다 호출되는 타이밍 체크
    /// </summary>
    private void OnBeat()
    {
        CheckComboTiming();
    }

    // 입력 시퀀스와 일치하는 콤보 후보군 반환
    public List<ComboSequence> GetMatchingCombos(List<AttackType> inputSequence)
    {
        var weapon = WeaponManager.Instance?.CurrentWeapon;
        var result = new List<ComboSequence>();
        if (weapon == null || weapon.weaponData.weaponType != WeaponType.Sword)
            return result;
        foreach (var combo in weapon.weaponData.swordCombos)
        {
            if (combo.attackSequence.Count < inputSequence.Count)
                continue;
            bool match = true;
            for (int i = 0; i < inputSequence.Count; i++)
            {
                if (combo.attackSequence[i].attackType != inputSequence[i])
                {
                    match = false;
                    break;
                }
            }
            if (match)
                result.Add(combo);
        }
        return result;
    }
}
