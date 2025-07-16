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

    [Header("공격 판정")]
    [SerializeField] protected LayerMask playerLayer;

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
    }
    
    // 향후 플레이어 거리기반 활성화 도입시 OnEnable으로 변경, OnDisable 추가
    private void Start()
    {
        EnemyManager.Instance?.RegisterEnemy(this);
    }

    // private void OnEnable()
    // {
    //     EnemyManager.Instance?.RegisterEnemy(this);
    // }

    // private void OnDisable()
    // {
    //     EnemyManager.Instance?.UnregisterEnemy(this);
    // }

    // 데미지 처리
    public virtual void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log($"Enemy HP: {currentHP}");

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
        StartCoroutine(DestroyWithTimeScale(destroyTime));
    }

    // 파괴 지연 처리
    private IEnumerator DestroyWithTimeScale(float delay)
    {
        float elapsed = 0f;
        while (elapsed < delay)
        {
            elapsed += GetAdjustedDeltaTime();
            yield return null;
        }
        Destroy(this.gameObject);
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
                        continue;
                    }
                }
                
                damageable.TakeDamage(Damage);
                Debug.Log($"에너미 {transform.name} 공격: {damageable.GetType().Name}이 {Damage} 입음");
            }
        }
    }
}
