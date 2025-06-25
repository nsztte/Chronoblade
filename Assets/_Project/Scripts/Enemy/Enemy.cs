using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    // [SerializeField] private int maxHP = 100;
    // [SerializeField] private int damage = 10;
    // [SerializeField] private float detectionRange = 10f;

    // [SerializeField] private float attackRange = 2f;
    // [SerializeField] private float attackCooldown = 1.5f;
    [Header("디폴트")]
    [SerializeField] private EnemyBehaviorData behaviorData;
    private EnemyStateMachine fsm;
    private int currentHP;


    [Header("공격 판정")]
    [SerializeField] private Transform attackStartPosition;
    [SerializeField] private Transform attackEndPosition;
    [SerializeField] private Transform attackCenter;
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private LayerMask playerLayer;


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

    // ChronoMonk 전용 프로퍼티
    public float TeleportDistance => behaviorData.teleportDistance;
    public float SlowDuration => behaviorData.slowDuration;

    // Mirror Duelist 전용 프로퍼티
    public GameObject FakeClonePrefab => behaviorData.fakeClonePrefab;
    public int NumberOfClones => behaviorData.numberOfClones;
    public float CloneLifetime => behaviorData.cloneLifeTime;
    public float CloneSpread => behaviorData.cloneSpread;
    #endregion

    private void Awake()
    {
        currentHP = behaviorData.maxHP;
        fsm = GetComponent<EnemyStateMachine>();
    }

    public void TakeDamage(int damage)
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

    public void Die()
    {
        Debug.Log("Enemy Die");
        
        Collider collider = GetComponent<Collider>();
        if(collider != null) collider.enabled = false;
        
        this.enabled = false;
        Destroy(this.gameObject, 5f);
    }

    public void PerformAttack()
    {
        switch(behaviorData.enemyType)
        {
            case EnemyType.Watcher:
            case EnemyType.MirrorDuelist:
                DealDamagedWithCapsule(attackStartPosition, attackEndPosition, attackRadius);
                break;
            case EnemyType.ChronoMonk:
                DealDamageWithSphere(attackCenter.position, attackRadius);
                break;
        }
    }

    // 근접 공격 판정 (Watcher, MirrorDuelist)
    private void DealDamagedWithCapsule(Transform startPosition, Transform endPosition, float radius)
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

    // 원형 공격 판정 (ChronoMonk)
    private void DealDamageWithSphere(Vector3 center, float radius)
    {
        Collider[] hits = Physics.OverlapSphere(center, radius, playerLayer);

        foreach(Collider hit in hits)
        {
            if(hit.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(Damage);
                //TODO: 플레이어 디버프 적용
                Debug.Log($"Enemy {Type} attacked {damageable.GetType().Name} for {Damage} damage");
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // 근접 공격(캡슐) 범위
        if (attackStartPosition != null && attackEndPosition != null)
        {
            Gizmos.color = Color.red;
            Vector3 start = attackStartPosition.position;
            Vector3 end = attackEndPosition.position;
            float length = Vector3.Distance(start, end);
            int steps = Mathf.Max(2, Mathf.CeilToInt(length / (attackRadius * 0.5f)));
            for (int i = 0; i <= steps; i++)
            {
                float t = (float)i / steps;
                Vector3 pos = Vector3.Lerp(start, end, t);
                Gizmos.DrawWireSphere(pos, attackRadius);
            }
        }

        // 원형 공격(스피어) 범위
        if (attackCenter != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(attackCenter.position, attackRadius);
        }
    }
#endif
}
