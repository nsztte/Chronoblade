using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(EnemyStateMachine), typeof(EnemyTimeController), typeof(FinalComboController))]
public abstract class Enemy : MonoBehaviour, IDamageable
{
    [Header("디폴트")]
    [SerializeField] protected EnemyBehaviorData behaviorData;
    protected EnemyStateMachine fsm;
    protected EnemyTimeController timeController;
    protected FinalComboController finalComboController;
    [SerializeField] protected int currentHP;
    [SerializeField] protected float destroyTime = 5f;
    [SerializeField] private Vector3 hpUIOffset = new Vector3(0, 0.3f, 0);
    [SerializeField] private GameObject hpUIPrefab;

    [Header("공격 판정")]
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected float stunDuration = 1.5f;
    [SerializeField] protected float stunKnockbackForce = 1f;

    private Animator animator;
    private NavMeshAgent agent;
    private EnemyHPUI hpUI;

    #region 전투 감지 이벤트
    // 전투 감지 이벤트
    public static event System.Action OnCombatStarted;

    // 전투 감지 이벤트를 호출하는 정적 메서드
    public static void TriggerCombatStarted()
    {
        OnCombatStarted?.Invoke();
    }
    #endregion

    #region Getter
    public EnemyBehaviorData BehaviorData => behaviorData;
    public EnemyType Type => behaviorData.enemyType;
    public FinalComboController FinalComboController => finalComboController;
    public int MaxHP => behaviorData.maxHP;
    public int Damage => behaviorData.damage;
    public float MoveSpeed => behaviorData.moveSpeed;
    public float DetectionRange => behaviorData.detectionRange;
    public float AttackRange => behaviorData.attackRange;
    public float AttackCooldown => behaviorData.attackCooldown;
    public float AttackSpeed => behaviorData.attackSpeed;
    #endregion

    protected virtual void Awake()
    {
        currentHP = behaviorData.maxHP;
        fsm = GetComponent<EnemyStateMachine>();
        timeController = GetComponent<EnemyTimeController>();
        finalComboController = GetComponent<FinalComboController>();

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    
    private void Start()
    {
        // EnemyManager.Instance?.RegisterEnemy(this);
        SetupHPUI();
    }

    private void OnEnable()
    {
        EnemyManager.Instance?.RegisterEnemy(this);
        ResetState();
    }

    private void OnDisable()
    {
        EnemyManager.Instance?.UnregisterEnemy(this);
    }

    public virtual void ResetState()
    {
        currentHP = behaviorData.maxHP;
        enabled = true;

        fsm.ResetToIdle();

        if (hpUI != null)
        {
            hpUI.SetHP(currentHP, MaxHP);
            hpUI.gameObject.SetActive(true);
        }

        var collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = true;
    }

    // 데미지 처리
    public virtual void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log($"Enemy HP: {currentHP}");

        hpUI.SetHP(currentHP, MaxHP);

        // 플레이어가 적을 공격하면 전투 시작
        if (GameManager.Instance.CurrentGameState is ExplorationState || GameManager.Instance.CurrentGameState is PuzzleState)
        {
            TriggerCombatStarted();
        }

        if(fsm != null)
        {
            if(currentHP <= 0)
            {
                fsm.TransitionToState(fsm.DeadState);
            }
            else
            {
                fsm.TransitionToState(fsm.HitState);
            }
        }
    }

    // 넉백 처리
    public virtual void ApplyKnockback(float force)
    {
        // 에너미의 로컬 기준으로 뒤쪽 방향 계산 (transform.forward의 반대 방향)
        Vector3 localBackDirection = -transform.forward;
        
        // 약간 위로 올리는 효과 추가
        localBackDirection.y = 0.3f;
        localBackDirection = localBackDirection.normalized;
        
        Vector3 knockbackPosition = transform.position + localBackDirection * force;
        
        Debug.Log($"[넉백] 원래 위치: {transform.position}, 로컬 뒤쪽 방향: {localBackDirection}, 힘: {force}, 목표 위치: {knockbackPosition}");

        // NavMesh 검색 반경을 force의 절반으로 설정 (더 넓은 범위에서 검색)
        float searchRadius = Mathf.Max(force * 0.5f, 100f);
        
        if(NavMesh.SamplePosition(knockbackPosition, out NavMeshHit hit, searchRadius, NavMesh.AllAreas))
        {
            // 코루틴을 사용하여 부드러운 이동
            StartCoroutine(SmoothKnockback(hit.position, 0.3f));
            Debug.Log($"[넉백] 부드러운 이동 시작 - 목표 위치: {hit.position}");
        }
        else
        {
            Debug.LogWarning($"[넉백] 실패! NavMesh에서 유효한 위치를 찾을 수 없음. 검색 반경: {searchRadius}");
        }
    }

    // 부드러운 넉백 이동 코루틴
    private IEnumerator SmoothKnockback(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = transform.position;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += GetAdjustedDeltaTime();
            float t = elapsed / duration;
            
            // 선형 이동
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, t);
            
            // NavMesh 위에 있는지 확인하고 이동
            if (NavMesh.SamplePosition(newPosition, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            {
                transform.position = hit.position;
            }
            
            yield return null;
        }
        
        // 최종 위치 보정
        if (NavMesh.SamplePosition(targetPosition, out NavMeshHit finalHit, 1f, NavMesh.AllAreas))
        {
            transform.position = finalHit.position;
        }
        
        // Debug.Log($"[넉백] 부드러운 이동 완료 - 최종 위치: {transform.position}");
    }

    // 사망 처리
    public virtual void Die()
    {
        Debug.Log("Enemy Die");
        EnemyManager.Instance.UnregisterEnemy(this);
        
        Collider collider = GetComponent<Collider>();
        if(collider != null) collider.enabled = false;
        
        this.enabled = false;
        
        // 시간 조절을 반영한 파괴 지연
        StartCoroutine(ReleaseWithTimeScale(destroyTime));
    }

    // 파괴 지연 처리
    private IEnumerator ReleaseWithTimeScale(float delay)
    {
        float elapsed = 0f;
        while (elapsed < delay)
        {
            elapsed += GetAdjustedDeltaTime();
            yield return null;
        }
        // Destroy(this.gameObject);
        EnemyManager.Instance?.ReleaseEnemy(this);
    }

    // 시간 조절 처리
    public float GetAdjustedDeltaTime()
    {
        return timeController != null ? timeController.GetAdjustedDeltaTime() : Time.deltaTime;
    }

    // 공격 처리
    // 애니메이션 이벤트 등록해서 이용
    public void PerformAttack()
    {
        OnPerformAttack();
    }

    // 각 적 타입별로 구현할 추상 메서드
    protected abstract void OnPerformAttack();

    // 근접 공격 판정 (공통 기능)
    protected void DealDamagedWithCapsule(Transform startPosition, Transform endPosition, float radius)
    {
        // Debug.Log("근접 공격 판정");
        Collider[] hits = Physics.OverlapCapsule(startPosition.position, endPosition.position, radius, playerLayer);

        foreach(Collider hit in hits)
        {
            if(hit.TryGetComponent(out IDamageable damageable))
            {
                if(damageable is PlayerManager player)
                {
                    float attackTime = Time.time;
                    if(player.TryParry(attackTime))
                    {
                        OnParried();
                        continue;
                    }
                }

                damageable.TakeDamage(Damage);
                // Debug.Log($"에너미 {transform.name} 공격: {damageable.GetType().Name}이 {Damage} 입음");
            }
        }
    }

    // 근접 구체 공격 판정
    protected void DealDamagedWithSphere(Transform center, float radius)
    {
        Debug.Log("근접 구체 공격 판정");
        Collider[] hits = Physics.OverlapSphere(center.position, radius, playerLayer);

        foreach(Collider hit in hits)
        {
            if(hit.TryGetComponent(out IDamageable damageable))
            {
                if(damageable is PlayerManager player)
                {
                    float attackTime = Time.time;
                    if(player.TryParry(attackTime))
                    {
                        OnParried();
                        continue;
                    }
                }

                damageable.TakeDamage(Damage);
                Debug.Log($"에너미 {transform.name} 공격: {damageable.GetType().Name}이 {Damage} 입음");
            }
        }
    }

    private void OnParried()
    {
        fsm.enabled = false;
        agent.isStopped = true;
        animator.SetBool("IsStunned", true);

        ApplyKnockback(stunKnockbackForce);

        StartCoroutine(RecoverFromStun(stunDuration));
    }

    private IEnumerator RecoverFromStun(float duration)
    {
        yield return new WaitForSeconds(duration);

        fsm.enabled = true;
        agent.isStopped = false;
        animator.SetBool("IsStunned", false);
    }

    private void SetupHPUI()
    {
        if (hpUIPrefab == null)
        {
            hpUIPrefab = Resources.Load<GameObject>("UI/EnemyHPUI");
        }

        if (hpUIPrefab == null)
        {
            Debug.LogError("[Enemy] EnemyHPUI 프리팹 로드 실패: Resources/UI/EnemyHPUI");
            return;
        }

        Transform followTarget = transform;

        if (TryGetComponent(out Animator animator) && animator.isHuman && animator.avatar != null)
        {
            // 휴머노이드면 Head 본 사용
            var head = animator.GetBoneTransform(HumanBodyBones.Head);
            if (head != null)
                followTarget = head;
        }
        else
        {
            // 휴머노이드가 아닐 경우 이름에 "head"가 포함된 자식 찾기
            Transform found = TransformUtils.FindChildRecursive(transform, t => t.name.ToLower().Contains("head"));
            if (found != null)
            {
                followTarget = found;
            }
            else
            {
                Debug.LogWarning($"Enemy {name}: 'Head' 트랜스폼을 찾을 수 없어 기본 transform 사용");
            }
        }

        GameObject uiObj = Instantiate(hpUIPrefab, transform);
        hpUI = uiObj.GetComponent<EnemyHPUI>();
        hpUI.SetFollowTarget(followTarget);
        hpUI.SetOffset(hpUIOffset);
        hpUI.SetHP(currentHP, MaxHP);
    }
}
