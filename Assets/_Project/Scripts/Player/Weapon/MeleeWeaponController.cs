using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeleeWeaponController : WeaponController
{
    [Header("근접 공격 설정")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    // [SerializeField] private float attackDuration = 0.3f;
    public LayerMask hitLayer;
    private HashSet<IDamageable> hitTargets = new HashSet<IDamageable>();

    [Header("스태미너 소모")]
    [SerializeField] private int staminaCost = 25;

    protected override void OnAttackInput()
    {
        // 스태미너가 충분할 때만 공격
        if (!gameObject.activeInHierarchy || isAttacking) return;
        if (PlayerManager.Instance.CurrentStamina < staminaCost)
        {
            Debug.Log("스태미너 부족! 공격 불가");
            return;
        }
        PlayerManager.Instance.UseStamina(staminaCost);

        isAttacking = true;
        Attack();
    }

    protected override void Attack()
    {
        PlayerManager.Instance.SetAnimatorTrigger("IsAttacking");
        // CameraController.Instance?.SetCameraMeleeAttackOffset(0.3f, 15f);
        hitTargets.Clear();
        Debug.Log($"[공격 시작] {weaponData.weaponName}");
    }

    // 애니메이션 이벤트로 호출될 메서드들
    public override void OnMeleeAttackHit()
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
    }

    public override void OnMeleeAttackEnd()
    {
        Debug.Log($"[공격 종료] 총 타격 대상 수: {hitTargets.Count}");
        // CameraController.Instance?.ResetCameraPosition(10f);
        isAttacking = false;
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
