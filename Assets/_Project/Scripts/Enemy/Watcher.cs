using UnityEngine;

public class Watcher : Enemy
{
    protected override void OnPerformAttack()
    {
        // Watcher는 근접 공격만 수행
        DealDamagedWithCapsule(attackStartPosition, attackEndPosition, attackRadius);
        Debug.Log("Watcher 근접 공격 실행");
    }
}
