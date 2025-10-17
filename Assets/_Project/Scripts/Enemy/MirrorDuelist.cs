using UnityEngine;
using System.Collections.Generic;

public class MirrorDuelist : Enemy
{
    [Header("공격 판정")]
    [SerializeField] private Transform attackCenter;
    [SerializeField] private float attackRadius = 1f;

    [Header("클론 쿨다운")]
    [SerializeField] private float cloneCooldown = 10f;
    private float lastCloneSpawnTime;
    private bool isSpawning = false;

    private List<FakeClone> activeClones = new();

    // Mirror Duelist 전용 프로퍼티
    // public GameObject FakeClonePrefab => behaviorData.fakeClonePrefab;
    public int NumberOfClones => behaviorData.numberOfClones;
    public float CloneLifetime => behaviorData.cloneLifeTime;
    public float CloneSpawnRadius => behaviorData.cloneSpawnRadius;
    public bool IsSpawning => isSpawning;

    public void RegisterClone(FakeClone clone)
    {
        if (!activeClones.Contains(clone))
            activeClones.Add(clone);
    }

    public void UnregisterClone(FakeClone clone)
    {
        activeClones.Remove(clone);
    }

    public bool HasActiveClones()
    {
        // null 정리 포함
        activeClones.RemoveAll(c => c == null);
        return activeClones.Count > 0;
    }

    public bool CanSpawnClone()
    {
        return Time.time - lastCloneSpawnTime >= cloneCooldown;
    }

    public void MarkCloneSpawned()
    {
        isSpawning = true;
        lastCloneSpawnTime = Time.time;
    }

    public override void TakeDamage(int damage)
    {
        DetectPlayer();

        if (HasActiveClones())
        {
            Debug.Log("Mirror Duelist 클론 존재 중 - 무적 상태");
            return;
        }

        currentHP -= damage;
        Debug.Log($"Mirror Duelist HP: {currentHP}");

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
                // 공격 중이거나 클론 스폰 중일 때는 HitState로 전환하지 않음
                if (fsm?.CurrentState is MirrorAttackState mirrorAttackState)
                {
                    if ((mirrorAttackState.isAttacking || isSpawning) && currentHP > 0)
                    {
                        Debug.Log("Mirror Duelist 공격 중 피격 - 애니메이션 없이 데미지만 적용");
                        return;
                    }
                }
                
                fsm.TransitionToState(fsm.HitState);
            }
        }
    }

    protected override void OnPerformAttack()
    {
        // Mirror Duelist는 근접 공격만 수행
        DealDamagedWithSphere(attackCenter, attackRadius);
        Debug.Log("Mirror Duelist 근접 공격 실행");
    }

    // 애니메이션 이벤트로 호출될 메서드 (클론 스폰 완료)
    public void OnMirrorSpawnEnd()
    {
        isSpawning = false;
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
                FakeClone clone = FakeClonePool.Instance?.Get();
                if (clone != null)
                {
                    clone.transform.SetPositionAndRotation(hit.position, Quaternion.identity);
                    clone.Initialize(this);
                    RegisterClone(clone);
                    Debug.Log($"클론 {i + 1} 생성 완료: {hit.position}");
                }
                else
                {
                    Debug.LogWarning("FakeClonePool에서 클론 가져오기 실패");
                }
            }
        }
    }
    

#if UNITY_EDITOR
    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        
        // 구체 공격 범위 표시
        if (attackCenter != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackCenter.position, attackRadius);
        }
        
        // 공격 범위 표시
        if (fsm?.Target != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, AttackRange);
        }
        
        // 클론 스폰 범위 표시
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, CloneSpawnRadius);
    }
#endif
}
