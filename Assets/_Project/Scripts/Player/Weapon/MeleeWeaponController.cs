using UnityEngine;
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



    public override void ExecuteLightAttack()
    {
        if (!gameObject.activeInHierarchy) return;
        if (!PlayerManager.Instance.UseStaminaIfAvailable(PlayerManager.Instance.StaminaCost))
        {
            Debug.Log("스태미너 부족! 약공격 불가");
            return;
        }
        currentAttackType = AttackType.Light;
        // PlayerManager.Instance.SetAnimatorTrigger("IsLightAttacking");
        PlayerManager.Instance.SetAnimatorTrigger("IsAttacking"); // 나중에 수정해야됨 지금은 테스트용
        ClearHitTargets();
        // 공격 실행 후에 isAttacking 설정
        isAttacking = true;
        // Debug.Log($"[약공격 시작] {weaponData.weaponName}");
    }

    public override void ExecuteHeavyAttack()
    {
        if (!gameObject.activeInHierarchy) return;
        if (!PlayerManager.Instance.UseStaminaIfAvailable(PlayerManager.Instance.StaminaCost * 2))
        {
            Debug.Log("스태미너 부족! 강공격 불가");
            return;
        }
        currentAttackType = AttackType.Heavy;
        // PlayerManager.Instance.SetAnimatorTrigger("IsHeavyAttacking");
        PlayerManager.Instance.SetAnimatorTrigger("IsAttacking"); // 나중에 수정해야됨 지금은 테스트용
        ClearHitTargets();
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

    public override void OnComboAttackHit(ComboAttackInfo comboAttackInfo)
    {
        Vector3 startPos = startPoint.position;
        Vector3 endPos = endPoint.position;
        float radius = weaponData.range;

        var comboInfo = comboAttackInfo;
        
        Collider[] hits = Physics.OverlapCapsule(startPos, endPos, radius, hitLayer);
        foreach(var hit in hits)
        {
            if(hit.TryGetComponent(out IDamageable target) && !hitTargets.Contains(target))
            {
                Debug.Log($"OnComboAttackHit!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                target.TakeDamage(Mathf.RoundToInt(comboInfo.damage));
                hitTargets.Add(target);
                
                // 넉백 적용
                if (comboInfo.knockbackPower > 0)
                {
                    // 에너미에서 자체적으로 로컬 기준 뒤쪽 방향을 계산하므로 방향 전달 불필요
                    target.ApplyKnockback(comboInfo.knockbackPower);
                }

                if(comboInfo.isFinalHit && hit.TryGetComponent(out IStatusEffectable effectable))
                {
                    effectable.ApplyStatus(comboInfo.statusEffect, comboInfo.statusDuration);
                }
                
                Debug.Log($"[콤보 타격] 대상: {hit.name}, 데미지: {comboInfo.damage:F1}, 넉백: {comboInfo.knockbackPower}");
            }
        }
    }

    // hitTargets 클리어 메서드 (콤보 타격 시작 시 호출)
    public void ClearHitTargets()
    {
        hitTargets.Clear();
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
