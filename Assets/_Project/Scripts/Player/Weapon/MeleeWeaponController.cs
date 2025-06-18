using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeleeWeaponController : WeaponController
{
    [Header("근접 공격 설정")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private float attackDuration = 0.3f;
    public LayerMask hitLayer;
    private HashSet<IDamageable> hitTargets = new HashSet<IDamageable>();

    protected override void Attack()
    {
        StartCoroutine(SwingWeapon());
    }

    /// <summary>
    /// ToDO: 플레이어 애니메이션 반영, 애니메이션 클립 길이만큼 쿨타임 반영
    /// </summary>
    private IEnumerator SwingWeapon()
    {
        Debug.Log($"[공격 시작] {weaponData.weaponName}, 지속시간: {attackDuration}");
        hitTargets.Clear();
        float t = 0f;

        while(t < attackDuration)
        {
            Vector3 startPos = startPoint.position;
            Vector3 endPos = endPoint.position;
            float radius = weaponData.range;

            Collider[] hits = Physics.OverlapCapsule(startPos, endPos, radius, hitLayer);

            foreach(var hit in hits)
            {
                if(hit.TryGetComponent(out IDamageable target) && !hitTargets.Contains(target))
                {
                    target.TakeDamage(weaponData.damage);
                    hitTargets.Add(target);
                    Debug.Log($"[타격 성공] 대상: {hit.name}, 데미지: {weaponData.damage}");
                }
            }

            t += Time.deltaTime;
            yield return null;
            Debug.Log($"[공격 종료] 총 타격 대상 수: {hitTargets.Count}");
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(startPoint.position, weaponData.range);
        Gizmos.DrawWireSphere(endPoint.position, weaponData.range);
    }
#endif
}
