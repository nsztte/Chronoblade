using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeleeWeaponController : WeaponController
{
    private enum AttackType { None, Light, Heavy }
    private AttackType currentAttackType = AttackType.None;

    [Header("근접 공격 설정")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    // [SerializeField] private float attackDuration = 0.3f;
    public LayerMask hitLayer;
    private HashSet<IDamageable> hitTargets = new HashSet<IDamageable>();

    [Header("스태미너 소모")]
    [SerializeField] private int staminaCost = 25;

    public override void ExecuteLightAttack()
    {
        if (!gameObject.activeInHierarchy) return;
        if (PlayerManager.Instance.CurrentStamina < staminaCost)
        {
            Debug.Log("스태미너 부족! 약공격 불가");
            return;
        }
        PlayerManager.Instance.UseStamina(staminaCost);
        Debug.Log("스테미너 25 소비");
        currentAttackType = AttackType.Light;
        // PlayerManager.Instance.SetAnimatorTrigger("IsLightAttacking");
        PlayerManager.Instance.SetAnimatorTrigger("IsAttacking"); // 나중에 수정해야됨 지금은 테스트용
        hitTargets.Clear();
        // 공격 실행 후에 isAttacking 설정
        isAttacking = true;
        // Debug.Log($"[약공격 시작] {weaponData.weaponName}");
    }

    public override void ExecuteHeavyAttack()
    {
        if (!gameObject.activeInHierarchy) return;
        if (PlayerManager.Instance.CurrentStamina < staminaCost * 2)
        {
            Debug.Log("스태미너 부족! 강공격 불가");
            return;
        }
        PlayerManager.Instance.UseStamina(staminaCost * 2);
        currentAttackType = AttackType.Heavy;
        // PlayerManager.Instance.SetAnimatorTrigger("IsHeavyAttacking");
        PlayerManager.Instance.SetAnimatorTrigger("IsAttacking"); // 나중에 수정해야됨 지금은 테스트용
        hitTargets.Clear();
        // 공격 실행 후에 isAttacking 설정
        isAttacking = true;
        // Debug.Log($"[강공격 시작] {weaponData.weaponName}");
    }

    public override void OnMeleeAttackHit()
    {
        Vector3 startPos = startPoint.position;
        Vector3 endPos = endPoint.position;
        float radius = weaponData.range;
        int damage = weaponData.damage;
        if (currentAttackType == AttackType.Heavy)
        {
            damage = Mathf.RoundToInt(weaponData.damage * 1.8f);
        }
        Collider[] hits = Physics.OverlapCapsule(startPos, endPos, radius, hitLayer);
        foreach(var hit in hits)
        {
            if(hit.TryGetComponent(out IDamageable target) && !hitTargets.Contains(target))
            {
                target.TakeDamage(damage);
                hitTargets.Add(target);
                Debug.Log($"[타격 성공] 대상: {hit.name}, 데미지: {damage} (타입: {currentAttackType})");
            }
        }
    }

    public override void OnMeleeAttackEnd()
    {
        // Debug.Log($"[공격 종료] 총 타격 대상 수: {hitTargets.Count}");
        // CameraController.Instance?.ResetCameraPosition(10f);
        isAttacking = false;
        currentAttackType = AttackType.None;
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
