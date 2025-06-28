using UnityEngine;

public class MirrorDuelist : Enemy
{
    // Mirror Duelist 전용 프로퍼티
    public GameObject FakeClonePrefab => behaviorData.fakeClonePrefab;
    public int NumberOfClones => behaviorData.numberOfClones;
    public float CloneLifetime => behaviorData.cloneLifeTime;
    public float CloneSpread => behaviorData.cloneSpread;

    protected override void OnPerformAttack()
    {
        // Mirror Duelist는 근접 공격만 수행
        DealDamagedWithCapsule(attackStartPosition, attackEndPosition, attackRadius);
        Debug.Log("Mirror Duelist 근접 공격 실행");
    }

    // 클론 생성 로직 (필요시 구현)
    private void CreateClones()
    {
        // TODO: 클론 생성 로직 구현
        Debug.Log("Mirror Duelist 클론 생성");
    }
}
