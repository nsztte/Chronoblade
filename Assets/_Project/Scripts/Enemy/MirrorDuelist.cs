using UnityEngine;

public class MirrorDuelist : Enemy
{
    [Header("공격 판정")]
    [SerializeField] private Transform attackCenter;
    [SerializeField] private float attackRadius = 1f;

    // Mirror Duelist 전용 프로퍼티
    public GameObject FakeClonePrefab => behaviorData.fakeClonePrefab;
    public int NumberOfClones => behaviorData.numberOfClones;
    public float CloneLifetime => behaviorData.cloneLifeTime;
    public float CloneSpawnRadius => behaviorData.cloneSpawnRadius;

    public override void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log($"Mirror Duelist HP: {currentHP}");

        // 공격 중이거나 클론 스폰 중일 때는 HitState로 전환하지 않음
        if (fsm?.CurrentState is MirrorAttackState mirrorAttackState)
        {
            if ((mirrorAttackState.isAttacking || mirrorAttackState.isSpawned) && currentHP > 0)
            {
                Debug.Log("Mirror Duelist 공격 중 피격 - 애니메이션 없이 데미지만 적용");
                return;
            }
        }

        // 일반적인 피격 처리
        if (fsm != null)
        {
            fsm.TransitionToState(currentHP <= 0 ? fsm.DeadState : fsm.HitState);
        }
    }

    protected override void OnPerformAttack()
    {
        // Mirror Duelist는 근접 공격만 수행
        DealDamagedWithSphere(attackCenter, attackRadius);
        Debug.Log("Mirror Duelist 근접 공격 실행");
    }

    // 애니메이션 이벤트로 호출될 메서드 (공격 완료)
    public void OnMirrorAttackEnd()
    {
        if (fsm?.CurrentState is MirrorAttackState mirrorAttackState)
        {
            mirrorAttackState.OnMirrorAttackEnd();
        }
    }

    // 애니메이션 이벤트로 호출될 메서드 (클론 스폰 완료)
    public void OnMirrorSpawnEnd()
    {
        if (fsm?.CurrentState is MirrorAttackState mirrorAttackState)
        {
            mirrorAttackState.OnMirrorSpawnEnd();
        }
    }

    // 애니메이션 이벤트로 호출될 메서드 (실제 클론 생성)
    public void OnMirrorCreateClones()
    {
        CreateClones();
    }

    // 클론 생성 로직 (애니메이션 이벤트 함수에서 호출)
    private void CreateClones()
    {
        Debug.Log($"Mirror Duelist 클론 생성 시작 - {NumberOfClones}개");
        
        for(int i = 0; i < NumberOfClones; i++)
        {
            // 반경 내 랜덤 위치 계산
            Vector2 randomCircle = Random.insideUnitCircle * CloneSpawnRadius;
            Vector3 spawnPosition = transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);

            // NavMesh 위의 유효한 위치 찾기
            if(UnityEngine.AI.NavMesh.SamplePosition(spawnPosition, out UnityEngine.AI.NavMeshHit hit, 2f, UnityEngine.AI.NavMesh.AllAreas))
            {
                GameObject clone = Instantiate(FakeClonePrefab, hit.position, Quaternion.identity);
                
                if(clone.TryGetComponent(out FakeClone fakeClone))
                {
                    fakeClone.Initialize(this);
                    Debug.Log($"클론 {i+1} 생성 완료: {hit.position}");
                }
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // 구체 공격 범위 표시
        if (attackCenter != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackCenter.position, attackRadius);
        }
        
        // 공격 범위 표시
        if (fsm?.Target != null)
        {
            Gizmos.color = new Color(1f, 0.5f, 0f); // 주황색
            Gizmos.DrawWireSphere(transform.position, AttackRange);
        }
        
        // 클론 스폰 범위 표시
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, CloneSpawnRadius);
    }
#endif
}
