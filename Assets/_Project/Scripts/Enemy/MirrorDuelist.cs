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
    public float CloneSpread => behaviorData.cloneSpread;

    protected override void OnPerformAttack()
    {
        // Mirror Duelist는 근접 공격만 수행
        DealDamagedWithSphere(attackCenter, attackRadius);
        Debug.Log("Mirror Duelist 근접 공격 실행");
    }

    // 클론 생성 로직 (애니메이션 이벤트 함수에서 호출)
    public void CreateClones()
    {
        // TODO: 클론 생성 로직 구현
        Debug.Log("Mirror Duelist 클론 생성");
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
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, AttackRange);
        }
        
        // 클론 스프레드 범위 표시
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, CloneSpread);
    }
#endif
}
