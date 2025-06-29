using UnityEngine;

public class Watcher : Enemy
{
    [Header("공격 판정")]
    [SerializeField] private Transform attackStartPosition;
    [SerializeField] private Transform attackEndPosition;
    [SerializeField] private float attackRadius = 1f;

    protected override void OnPerformAttack()
    {
        // Watcher는 근접 공격만 수행
        DealDamagedWithCapsule(attackStartPosition, attackEndPosition, attackRadius);
        Debug.Log("Watcher 근접 공격 실행");
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // 근접 공격(캡슐) 범위 표시
        if (attackStartPosition != null && attackEndPosition != null)
        {
            Gizmos.color = Color.red;
            float length = Vector3.Distance(attackStartPosition.position, attackEndPosition.position);
            int steps = Mathf.Max(2, Mathf.CeilToInt(length / (attackRadius * 0.5f)));
            for (int i = 0; i <= steps; i++)
            {
                float t = (float)i / steps;
                Vector3 pos = Vector3.Lerp(attackStartPosition.position, attackEndPosition.position, t);
                Gizmos.DrawWireSphere(pos, attackRadius);
            }
        }

        // 공격 범위 표시
        if(fsm?.Target != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, AttackRange);
        }
    }
#endif
}
