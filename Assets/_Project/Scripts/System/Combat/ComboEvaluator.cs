using System;
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
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnLightAttackPressed += () => RegisterInput(AttackType.Light);
            InputManager.Instance.OnHeavyAttackPressed += () => RegisterInput(AttackType.Heavy);
        }

        // TimingComboManager의 비트 이벤트 구독
        if (TimingComboManager.Instance != null)
        {
            TimingComboManager.Instance.OnBeat += OnBeat;
        }
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnLightAttackPressed -= () => RegisterInput(AttackType.Light);
            InputManager.Instance.OnHeavyAttackPressed -= () => RegisterInput(AttackType.Heavy);
        }

        if (TimingComboManager.Instance != null)
        {
            TimingComboManager.Instance.OnBeat -= OnBeat;
        }
    }

    /// <summary>
    /// 공격 입력 등록 및 콤보 평가
    /// </summary>
    public void RegisterInput(AttackType input)
    {
        // 타이밍 판정
        var (result, damageMultiplier, absOffset) = TimingComboManager.Instance.JudgeTiming(Time.time);
        
        // Miss 판정이면 콤보 실패 또는 일반 공격으로 처리
        if (result == TimingComboManager.TimingResult.Miss)
        {
            if (isComboInProgress)
            {
                Debug.Log($"[ComboEvaluator] 콤보 실패 - 타이밍 Miss (offset: {absOffset:F3})");
                OnComboFailed?.Invoke(currentCombo);
                ResetCombo();
            }
            else
            {
                // 콤보 진행 중이 아니면 일반 공격으로 처리
                OnNormalAttackExecuted?.Invoke(input);
            }
            return;
        }
        
        // Perfect/Good 판정일 때만 콤보 진행
        Debug.Log($"[ComboEvaluator] 타이밍 판정: {result} (offset: {absOffset:F3})");
        
        lastInputTime = Time.time;

        // 콤보 진행 중이 아니면 콤보 시작 시도
        if (!isComboInProgress)
        {
            TryStartCombo(input);
        }
        else
        {
            // 콤보 진행 중이면 다음 단계 확인
            TryContinueCombo(input);
        }
    }

    /// <summary>
    /// 콤보 시작 시도
    /// </summary>
    private void TryStartCombo(AttackType input)
    {
        var weapon = WeaponManager.Instance?.CurrentWeapon;
        if (weapon == null || weapon.weaponData.weaponType != WeaponType.Sword)
            return;
            
        foreach (var combo in weapon.weaponData.swordCombos)
        {
            if (combo.attackSequence[0].attackType == input)
            {
                // 콤보 시작!
                isComboInProgress = true;
                currentCombo = combo;
                currentComboStep = 0;
                
                Debug.Log($"[ComboEvaluator] 콤보 시작: {combo.comboName}");
                
                // 즉시 첫 번째 공격 실행
                ExecuteComboAttack(combo, 0);
                return;
            }
        }
        
        // 콤보가 아니면 일반 공격으로 처리
        OnNormalAttackExecuted?.Invoke(input);
    }

    /// <summary>
    /// 콤보 계속 진행
    /// </summary>
    private void TryContinueCombo(AttackType input)
    {
        if (currentCombo == null) return;
        
        currentComboStep++;
        if (currentComboStep < currentCombo.attackSequence.Count)
        {
            if (currentCombo.attackSequence[currentComboStep].attackType == input)
            {
                // 콤보 계속 진행 - 즉시 공격 실행
                Debug.Log($"[ComboEvaluator] 콤보 단계 진행: {currentComboStep + 1}/{currentCombo.attackSequence.Count}");
                ExecuteComboAttack(currentCombo, currentComboStep);
            }
            else
            {
                // 콤보 실패 - 잘못된 입력
                Debug.Log("[ComboEvaluator] 콤보 실패 - 잘못된 입력");
                OnComboFailed?.Invoke(currentCombo);
                ResetCombo();
            }
        }
        else
        {
            // 콤보 완성!
            Debug.Log($"[ComboEvaluator] 콤보 완성: {currentCombo.comboName}");
            OnComboCompleted?.Invoke(currentCombo);
            ResetCombo();
        }
    }

    /// <summary>
    /// 콤보 공격 실행
    /// </summary>
    private void ExecuteComboAttack(ComboSequence combo, int step)
    {
        var attackData = combo.attackSequence[step];
        // 즉시 공격 실행 (애니메이션, 데미지 등)
        OnComboAttackExecuted?.Invoke(combo, step, attackData);
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
    /// 첫 번째 공격 입력이 콤보의 첫 번째 입력과 일치하는지 확인
    /// </summary>
    public bool CanStartCombo(AttackType input)
    {
        var weapon = WeaponManager.Instance?.CurrentWeapon;
        if (weapon == null || weapon.weaponData.weaponType != WeaponType.Sword)
            return false;
            
        foreach (var combo in weapon.weaponData.swordCombos)
        {
            if (combo.attackSequence[0].attackType == input)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 첫 번째 공격 입력으로 시작할 수 있는 콤보 찾기
    /// </summary>
    public ComboSequence GetStartableCombo(AttackType input)
    {
        var weapon = WeaponManager.Instance?.CurrentWeapon;
        if (weapon == null || weapon.weaponData.weaponType != WeaponType.Sword)
            return null;
            
        foreach (var combo in weapon.weaponData.swordCombos)
        {
            if (combo.attackSequence[0].attackType == input)
            {
                return combo;
            }
        }
        return null;
    }

    /// <summary>
    /// 비트마다 호출되는 타이밍 체크
    /// </summary>
    private void OnBeat()
    {
        CheckComboTiming();
    }
}
