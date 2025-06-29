using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    [Header("디폴트")]
    [SerializeField] protected EnemyBehaviorData behaviorData;
    protected EnemyStateMachine fsm;
    protected EnemyTimeController timeController;
    [SerializeField] protected int currentHP;
    [SerializeField] protected float destroyTime = 5f;

    [Header("공격 판정")]
    [SerializeField] protected LayerMask playerLayer;

    #region Getter
    public EnemyBehaviorData BehaviorData => behaviorData;
    public EnemyType Type => behaviorData.enemyType;
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
    }

    public virtual void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log($"Enemy HP: {currentHP}");

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

    public virtual void Die()
    {
        Debug.Log("Enemy Die");
        
        Collider collider = GetComponent<Collider>();
        if(collider != null) collider.enabled = false;
        
        this.enabled = false;
        
        // 시간 조절을 반영한 파괴 지연
        StartCoroutine(DestroyWithTimeScale(destroyTime));
    }

    private System.Collections.IEnumerator DestroyWithTimeScale(float delay)
    {
        float elapsed = 0f;
        while (elapsed < delay)
        {
            elapsed += GetAdjustedDeltaTime();
            yield return null;
        }
        Destroy(this.gameObject);
    }

    public float GetAdjustedDeltaTime()
    {
        return timeController != null ? timeController.GetAdjustedDeltaTime() : Time.deltaTime;
    }

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
        Debug.Log("근접 공격 판정");
        Collider[] hits = Physics.OverlapCapsule(startPosition.position, endPosition.position, radius, playerLayer);

        foreach(Collider hit in hits)
        {
            if(hit.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(Damage);
                Debug.Log($"에너미 {transform.name} 공격: {damageable.GetType().Name}이 {Damage} 입음");
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
                damageable.TakeDamage(Damage);
                Debug.Log($"에너미 {transform.name} 공격: {damageable.GetType().Name}이 {Damage} 입음");
            }
        }
    }
}
