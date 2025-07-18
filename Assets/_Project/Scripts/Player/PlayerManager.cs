using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IDamageable
{
    [Header("플레이어 참조")]
    [SerializeField] private Transform playerTransform;

    [Header("HP")]
    [SerializeField] private int maxHP = 100;
    [SerializeField] private float currentHP;

    [Header("MP")]
    [SerializeField] private int maxMP = 100;
    [SerializeField] private float currentMP;
    [SerializeField] private float mpRecoveryDelay = 2.5f;    // 회복 시작전 초기 딜레이
    [SerializeField] private float mpRecoveryRate = 0.03f; // 최대 MP의 3%/초
    [SerializeField] private float mpRecoveryFlat = 1.5f;    // 초당 고정 1.5 회복

    [Header("스태미너")]
    [SerializeField] private int maxStamina = 100;
    [SerializeField] private float currentStamina;
    [SerializeField] private float staminaRecoveryDelay = 0.5f;    // 회복 시작전 초기 딜레이
    [SerializeField] private float staminaRecoveryRate = 25f;    // 초당 25 회복

    [Header("골드")]
    [SerializeField] private int gold = 0;

    [Header("상호작용")]
    [SerializeField] private float interactRadius = 2f;

    [Header("스태미너 소모")]
    [SerializeField] private int staminaCost = 12;  // 약공격 기준 (강공격 2배 소모)
    [SerializeField] private int dashStaminaCost = 15;

    [Header("방어 관련")]
    [SerializeField] private int blockHitCost = 15;
    [SerializeField] private float blockDamageReduction = 0.4f;

    public bool IsBlocking { get; set; } = false;
    public float LastBlockEndTime { get; set; } = -999f;
    public float BlockHitCost => blockHitCost;
    public float BlockDamageReduction => blockDamageReduction;

    [Header("패링")]
    [SerializeField] private float parryWindow = 0.25f;
    public float ParryWindow => parryWindow;
    

    [Header("무적 상태")]
    [SerializeField] private bool isInvincible = false;
    [SerializeField] private float invincibilityTimer = 0f;
    [SerializeField] private float hitInvincibilityDuration = 0.5f;

    private float currentComboDamage;
    private ComboAttackData currentCombo;

    private Animator animator;
    private PlayerStateMachine playerStateMachine;
    private PlayerController playerController;

    // 회복 타이머
    private float mpRecoveryTimer = 0f;
    private float staminaRecoveryTimer = 0f;

    #region Properties
    public Transform PlayerTransform => playerTransform;
    public int MaxHP => maxHP;
    public float CurrentHP => currentHP;
    public int MaxMP => maxMP;
    public float CurrentMP => currentMP;
    public int MaxStamina => maxStamina;
    public float CurrentStamina => currentStamina;
    public int Gold => gold;
    public PlayerStateMachine PlayerStateMachine => playerStateMachine;
    public PlayerController PlayerController => playerController;
    public int StaminaCost => staminaCost;
    public int DashStaminaCost => dashStaminaCost;
    public bool IsInvincibleProperty => isInvincible;
    public ComboAttackData CurrentCombo => currentCombo;
    public bool IsFrozen => playerController.IsFrozen;
    #endregion

    #region Singleton
    public static PlayerManager Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 플레이어 애니메이터 참조
            animator = GetComponent<Animator>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private void Start()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
        
        currentHP = maxHP;
        currentMP = maxMP;
        currentStamina = maxStamina;

        // UI 업데이트
        UIManager.Instance?.UpdateHP(Mathf.RoundToInt(currentHP), maxHP);
        UIManager.Instance?.UpdateMP(Mathf.RoundToInt(currentMP), maxMP);
        UIManager.Instance?.UpdateStamina(Mathf.RoundToInt(currentStamina), maxStamina);
        UIManager.Instance?.UpdateGold(gold);

        // 상호작용 이벤트 등록
        InputManager.Instance.OnInteract += OnHandleInteract;
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnInteract -= OnHandleInteract;
    }

    private void Update()
    {
        RecoverMpOverTime();
        RecoverStaminaOverTime();
        UpdateInvincibility();
    }

    #region 플레이어 상태 관리
    public void TakeDamage(int damage)
    {
        // 무적 상태 체크
        if (IsInvincible()) return;

        // 방어 상태 체크
        if(IsBlocking)
        {
            if(UseStaminaIfAvailable(BlockHitCost))
            {
                damage = Mathf.RoundToInt(damage * (1 - BlockDamageReduction));
                Debug.Log($"방어 성공! 데미지 감소: {damage}");
            }

            currentHP -= damage;
            currentHP = Mathf.Clamp(currentHP, 0, maxHP);
            UIManager.Instance?.UpdateHP(Mathf.RoundToInt(currentHP), maxHP);

            if(currentHP <= 0)
            {
                Die();
            }

            return;
        }
        
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        // UI 업데이트
        UIManager.Instance?.UpdateHP(Mathf.RoundToInt(currentHP), maxHP);

        if(currentHP <= 0)
        {
            Die();
        }
        else
        {
            // 일반 피격 시 일정 시간 무적
            SetInvincible(true, hitInvincibilityDuration);
            
            // 피격 상태로 전환
            playerStateMachine?.ChangeState(new PlayerHitState(playerStateMachine));
        }
    }


    private void Die()
    {
        // 게임 오버 처리
        Debug.Log("플레이어 죽음");

        // 사망 상태로 전환
        playerStateMachine?.ChangeState(new PlayerDeathState(playerStateMachine));
    }


    #endregion

    #region 플레이어 자원 관리
    public void HealHP(float amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        // UI 업데이트
        UIManager.Instance?.UpdateHP(Mathf.RoundToInt(currentHP), maxHP);
    }

    public void UseMP(float amount)
    {
        currentMP -= amount;
        currentMP = Mathf.Clamp(currentMP, 0, maxMP);
        mpRecoveryTimer = 0f;

        // UI 업데이트
        UIManager.Instance?.UpdateMP(Mathf.RoundToInt(currentMP), maxMP);
    }

    public void RestoreMP(float amount)
    {
        currentMP += amount;
        currentMP = Mathf.Clamp(currentMP, 0, maxMP);

        // UI 업데이트
        UIManager.Instance?.UpdateMP(Mathf.RoundToInt(currentMP), maxMP);
    }

    public bool UseStaminaIfAvailable(float amount)
    {
        if(currentStamina >= amount)
        {
            UseStamina(amount);
            return true;
        }
        return false;
    }

    private void UseStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        staminaRecoveryTimer = 0f;

        // UI 업데이트
        UIManager.Instance?.UpdateStamina(Mathf.RoundToInt(currentStamina), maxStamina);
    }

    public void RecoverStamina(float amount)
    {
        currentStamina += amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        // UI 업데이트
        UIManager.Instance?.UpdateStamina(Mathf.RoundToInt(currentStamina), maxStamina);
    }

    public void AddGold(int amount)
    {
        gold += amount;

        // UI 업데이트
        UIManager.Instance?.UpdateGold(gold);
    }

    public bool SpendGold(int amount)
    {
        if(gold >= amount)
        {
            gold -= amount;

            // UI 업데이트
            UIManager.Instance?.UpdateGold(gold);

            return true;
        }
        return false;
    }

    private void RecoverMpOverTime()
    {
        if(currentMP < maxMP)
        {
            mpRecoveryTimer += Time.deltaTime;

            if(mpRecoveryTimer >= mpRecoveryDelay)
            {
                float recoveryAmount = (mpRecoveryRate * maxMP + mpRecoveryFlat) * Time.deltaTime;
                currentMP += recoveryAmount;
                currentMP = Mathf.Clamp(currentMP, 0, maxMP);

                UIManager.Instance?.UpdateMP(Mathf.RoundToInt(currentMP), maxMP);
            }
        }
        else
        {
            mpRecoveryTimer = 0f;
        }
    }

    private void RecoverStaminaOverTime()
    {
        if(IsBlocking) return;
        
        if(currentStamina < maxStamina)
        {
            staminaRecoveryTimer += Time.deltaTime;

            if(staminaRecoveryTimer >= staminaRecoveryDelay)
            {
                currentStamina += staminaRecoveryRate * Time.deltaTime;
                currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

                UIManager.Instance?.UpdateStamina(Mathf.RoundToInt(currentStamina), maxStamina);
            }
        }
        else
        {
            staminaRecoveryTimer = 0f;
        }
    }
    #endregion

    #region 애니메이터 제어
    // 애니메이터 제어 메서드
    public void SetAnimatorBool(string param, bool value)
    {
        animator.SetBool(param, value);
    }
    public void SetAnimatorTrigger(string param)
    {
        animator.SetTrigger(param);
    }
    public void SetAnimatorFloat(string param, float value)
    {
        animator.SetFloat(param, value);
    }
    public void SetAnimatorFloat(string param, float value, float dampTime, float deltaTime)
    {
        animator.SetFloat(param, value, dampTime, deltaTime);
    }
    #endregion

    #region 애니메이션 이벤트 메서드
    // 애니메이션 이벤트 메서드들 (무기 컨트롤러에 전달)
    public void OnMeleeAttackHit()
    {
        WeaponManager.Instance?.CurrentWeapon?.OnMeleeAttackHit();
    }

    public void OnMeleeAttackEnd()
    {
        WeaponManager.Instance?.CurrentWeapon?.OnMeleeAttackEnd();
    }

    // 애니메이션 이벤트에서 호출할 메서드
    public void OnComboAttackHit()
    {
        WeaponManager.Instance?.CurrentWeapon?.OnComboAttackHit(currentComboDamage, currentCombo);
    }
    #endregion

    // 콤보 공격 정보 설정
    public void SetCurrentCombo(float damage, ComboAttackData comboAttackData)
    {
        currentComboDamage = damage;
        currentCombo = comboAttackData;
    }

    private void OnHandleInteract()
    {
        // 플레이어 주변의 IInteractable을 탐색하여 가장 가까운 것과 상호작용
        Collider[] hits = Physics.OverlapSphere(transform.position, interactRadius);
        IInteractable closest = null;
        float minDistance = float.MaxValue;
        foreach (var hit in hits)
        {
            var interactable = hit.GetComponent<IInteractable>();
            if (interactable != null)
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = interactable;
                }
            }
        }
        if (closest != null)
        {
            closest.Interact();
        }
    }

    public void ApplyKnockback(float force) {}

    public bool TryParry(float attackTime)
    {
        float deltaTime = Mathf.Abs(Time.time - LastBlockEndTime);
        if(deltaTime <= ParryWindow)
        {
            Debug.Log("패링 성공!");

            SetInvincible(true, 0.5f);

            // TODO: 이펙트, 슬로우 연출, 패링 카운터 처리
            return true;
        }
        return false;
    }

    #region 무적 상태 관리
    public bool IsInvincible()
    {
        return isInvincible;
    }

    public void SetInvincible(bool invincible, float duration = 0f)
    {
        isInvincible = invincible;
        if (invincible && duration > 0)
        {
            invincibilityTimer = duration;
        }
    }

    private void UpdateInvincibility()
    {
        if (isInvincible && invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
            }
        }
    }

    // 콤보 타이밍에 따른 무적 시간 설정
    public void OnComboAttackSuccess(TimingComboManager.TimingResult timing)
    {
        float invincibilityDuration = GetInvincibilityTimeByTiming(timing);
        SetInvincible(true, invincibilityDuration);
    }

    private float GetInvincibilityTimeByTiming(TimingComboManager.TimingResult timing)
    {
        switch (timing)
        {
            case TimingComboManager.TimingResult.Perfect:
                return 0.7f;
            case TimingComboManager.TimingResult.Good:
                return 0.5f;
            case TimingComboManager.TimingResult.Miss:
                return 0f;
            default:
                return 0f;
        }
    }
    #endregion
}          
