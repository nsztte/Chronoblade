using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("디폴트")]
    [SerializeField] private EnemyBehaviorData behaviorData;
    private EnemyStateMachine fsm;
    private EnemyTimeController timeController;
    private int currentHP;


    [Header("공격 판정")]
    [SerializeField] private Transform attackStartPosition;
    [SerializeField] private Transform attackEndPosition;
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private LayerMask playerLayer;

    [Header("크로노몽크 발사체")]
    [SerializeField] private Transform projectileSpawnPoint;


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
    public GameObject ChronoProjectilePrefab => behaviorData.chronoProjectilePrefab;
    public float ProjectileSpeed => behaviorData.projectileSpeed;
    public float ProjectileLifetime => behaviorData.projectileLifetime;

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
        timeController = GetComponent<EnemyTimeController>();
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
        
        // 시간 조절을 반영한 파괴 지연
        StartCoroutine(DestroyWithTimeScale(5f));
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
        switch(behaviorData.enemyType)
        {
            case EnemyType.Watcher:
            case EnemyType.MirrorDuelist:
                DealDamagedWithCapsule(attackStartPosition, attackEndPosition, attackRadius);
                break;
            case EnemyType.ChronoMonk:
                FireChronoProjectile();
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

    // 크로노몽크 구체 발사
    private void FireChronoProjectile()
    {
        if (ChronoProjectilePrefab == null || projectileSpawnPoint == null)
        {
            Debug.LogWarning("크로노몽크 발사체 프리팹 또는 스폰 포인트 없음");
            return;
        }

        // 플레이어 방향 계산
        Transform target = fsm?.Target;
        if (target == null)
        {
            Debug.LogWarning("크로노몽크 발사체 대상 없음");
            return;
        }

        // 플레이어의 실제 높이를 고려한 타겟 위치 계산
        Vector3 targetPos = target.position;
        Vector3 spawnPos = projectileSpawnPoint.position;
        
        // 플레이어의 Collider를 확인하여 적절한 높이 계산
        Collider playerCollider = target.GetComponent<Collider>();
        float targetHeight = targetPos.y;
        
        if (playerCollider != null)
        {
            targetHeight = playerCollider.bounds.center.y;  // 플레이어 중앙 높이
        }
        
        // 발사체가 날아갈 타겟 위치 (수평은 플레이어 위치, 높이는 계산된 높이)
        Vector3 adjustedTargetPos = new Vector3(targetPos.x, targetHeight, targetPos.z);
        Vector3 direction = (adjustedTargetPos - spawnPos).normalized;
        
        // 발사체 생성 및 초기화
        GameObject projectileObj = Instantiate(ChronoProjectilePrefab, projectileSpawnPoint.position, Quaternion.LookRotation(direction));
        ChronoProjectile projectile = projectileObj.GetComponent<ChronoProjectile>();
        
        if (projectile != null)
        {
            projectile.Initialize(direction, Damage, SlowDuration);
            Debug.Log($"크로노몽크 발사체 발사: {target.name} (높이: {targetHeight:F2})");
        }
        else
        {
            Debug.LogError("크로노몽크 발사체 컴포넌트 없음");
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

        // 발사체 스폰 포인트 표시
        if (projectileSpawnPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(projectileSpawnPoint.position, 0.2f);
        }
    }
#endif
}
