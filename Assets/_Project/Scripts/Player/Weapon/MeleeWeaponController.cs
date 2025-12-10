using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MeleeWeaponController : WeaponController
{
    private enum AttackType { None, Light, Heavy }
    private AttackType currentAttackType = AttackType.None;

    [Header("근접 공격 설정")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private LayerMask hitLayer;

    [Header("쉐이킹 설정")]
    [SerializeField] private float shakeIntensity = 0.06f;
    [SerializeField] private float shakeDuration = 0.1f;
    [SerializeField] private float comboShakeIntensity = 1f;
    [SerializeField] private float comboShakeDuration = 0.1f;

    [Header("Vfx 설정")]
    [SerializeField] private Transform vfxSpawnPoint;
    [SerializeField] private Vector3 hitVfxRotationOffsetEuler;
    public Transform VfxSpawnPoint => vfxSpawnPoint;

    private float pendingComboDamage;
    private ComboAttackData pendingComboData;

    private Animator animator;
    private HashSet<IDamageable> hitTargets = new HashSet<IDamageable>();

    private static readonly int Light = Animator.StringToHash("Light");
    private static readonly int Heavy = Animator.StringToHash("Heavy");
    private static readonly int Blocking  = Animator.StringToHash("Blocking");
    private static readonly int Parrying  = Animator.StringToHash("Parrying");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void ExecuteLightAttack()
    {
        if (!gameObject.activeInHierarchy) return;
        if (!PlayerManager.Instance.UseStaminaIfAvailable(PlayerManager.Instance.StaminaCost))
        {
            Debug.Log("스태미너 부족! 약공격 불가");
            return;
        }
        currentAttackType = AttackType.Light;
        if(animator) animator.SetTrigger(Light);
        ClearHitTargets();
        // 공격 실행 후에 isAttacking 설정
        isAttacking = true;
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
        if(animator) animator.SetTrigger(Heavy);
        ClearHitTargets();
        // 공격 실행 후에 isAttacking 설정
        isAttacking = true;
    }

    public override void SetBlocking(bool isBlocking)
    {
        if (!animator) return;
        animator.SetBool(Blocking, isBlocking);
    }

    public override void ExecuteParrying()
    {
        if (!animator) return;
        animator.SetTrigger(Parrying);
    }

    // 콤보 공격 데이터 셋팅
    public void SetPendingCombo(float damage, ComboAttackData data)
    {
        pendingComboDamage = damage;
        pendingComboData   = data;
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

                if (vfxSpawnPoint)
                {
                    var baseRot = vfxSpawnPoint.rotation;
                    var offsetRot = Quaternion.Euler(hitVfxRotationOffsetEuler);
                    var finalRot = baseRot * offsetRot;
                    VFXManager.Instance?.Spawn("HitSpark", vfxSpawnPoint.position, finalRot);
                }

                if (target is Enemy enemy && enemy.Type == EnemyType.Watcher_Tutorial)
                {
                    if (currentAttackType == AttackType.Light)
                    {
                        CombatTutorialManager.Instance?.OnLightAttackHitEnemy();
                    }
                    else if (currentAttackType == AttackType.Heavy)
                    {
                        CombatTutorialManager.Instance?.OnHeavyAttackHitEnemy();
                    }
                }

                CameraController.Instance?.PlayImpactShake(shakeIntensity, shakeDuration);
                
                Debug.Log($"[타격 성공] 대상: {hit.name}, 데미지: {damage} (타입: {currentAttackType})");
            }
        }

        if (vfxSpawnPoint)
            VFXManager.Instance?.Spawn("Swing", vfxSpawnPoint.position, vfxSpawnPoint.rotation);
    }

    public override void OnMeleeAttackEnd()
    {
        isAttacking = false;
        currentAttackType = AttackType.None;
    }

    public void OnComboAttackHit()
    {
        OnComboAttackHit(pendingComboDamage, pendingComboData);
    }

    public override void OnComboAttackHit(float damage, ComboAttackData comboAttackData)
    {
        Vector3 startPos = startPoint.position;
        Vector3 endPos = endPoint.position;
        float radius = weaponData.range;
        int aoeCount = 0;

        Collider[] hits = Physics.OverlapCapsule(startPos, endPos, radius, hitLayer);
        foreach(var hit in hits)
        {
            if(hit.TryGetComponent(out IDamageable target))
            {
                if (vfxSpawnPoint)
                {
                    var baseRot = vfxSpawnPoint.rotation;
                    var offsetRot = Quaternion.Euler(hitVfxRotationOffsetEuler);
                    var finalRot = baseRot * offsetRot;
                    VFXManager.Instance?.Spawn("HitSpark", vfxSpawnPoint.position, finalRot);
                }

                if (target is Enemy enemy && enemy.Type == EnemyType.Watcher_Tutorial)
                {
                    if (comboAttackData.attackType == global::AttackType.Light)
                    {
                        CombatTutorialManager.Instance?.OnLightAttackHitEnemy();
                    }
                    else if (comboAttackData.attackType == global::AttackType.Heavy)
                    {
                        CombatTutorialManager.Instance?.OnHeavyAttackHitEnemy();
                    }
                }

                if (comboAttackData.isAOE)
                {
                    ApplyAOE(hit.transform.position, comboAttackData, damage, ref aoeCount);
                }
                else
                {
                    ProcessComboAttack(hit.gameObject, comboAttackData, damage);
                }

                if (comboAttackData.isFinalHit)
                {
                    CameraController.Instance?.PlayImpactShake(comboShakeIntensity, comboShakeDuration);

                    // 튜토리얼용 콜백
                    if (target is Enemy e && e.Type == EnemyType.Watcher_Tutorial)
                    {
                        CombatTutorialManager.Instance?.OnTimingComboSuccess();
                    }
                }
            }
        }

        if (vfxSpawnPoint)
            VFXManager.Instance?.Spawn("Swing", vfxSpawnPoint.position, vfxSpawnPoint.rotation);
    }

    private void ProcessComboAttack(GameObject hitObject, ComboAttackData data, float damage)
    {
        if(!hitObject.TryGetComponent(out IDamageable target)) return;
        if(hitTargets.Contains(target)) return;
        hitTargets.Add(target);

        if(data.isMultiHit)
        {
            StartCoroutine(ApplyMultiHit(target, data, damage));
        }
        else
        {
            target.TakeDamage(Mathf.RoundToInt(damage));
        }

        if(data.knockbackPower > 0)
        {
            target.ApplyKnockback(data.knockbackPower);
        }

        if(data.isFinalHit && data.statusEffectType != StatusEffectType.None && hitObject.TryGetComponent(out IStatusEffectable effectable))
        {
            effectable.ApplyStatus(data);
        }
    }

    private IEnumerator ApplyMultiHit(IDamageable target, ComboAttackData data, float damage)
    {
        int count = data.multiHitCount;
        float interval = data.multiHitInterval;

        for(int i = 0; i < count; i++)
        {
            Debug.Log($"ApplyMultiHit {i}: {damage}");
            target.TakeDamage(Mathf.RoundToInt(damage));
            yield return new WaitForSeconds(interval);
            damage = Mathf.Max(damage * 0.8f, 10f);
        }
    }

    private void ApplyAOE(Vector3 center, ComboAttackData data, float damage, ref int hitCount)
    {
        Collider[] aoeHits = Physics.OverlapSphere(center, data.aoeRadius, hitLayer);
        foreach(var aoeHit in aoeHits)
        {
            if(hitCount >= data.aoeHitCount) break;
            ProcessComboAttack(aoeHit.gameObject, data, damage);
            hitCount++;
        }
    }

    // hitTargets 클리어 메서드 (콤보 타격 시작 시 호출)
    public void ClearHitTargets()
    {
        hitTargets.Clear();
    }

    #region 애니메이션 관련
    public void SetSpeed(float speed)
    {
        if (animator) animator.speed = Mathf.Max(0.01f, speed);
    }

    // 스테이트 이름으로 페이드 재생
    public void PlayClip(string stateName, float fade = 0.05f)
    {
        if (!animator || string.IsNullOrEmpty(stateName)) return;

        int hash = Animator.StringToHash(stateName);
        animator.CrossFadeInFixedTime(hash, fade, 0);
    }

    // 클립 이름으로 페이드 재생
    public void PlayClip(AnimationClip clip, float fade = 0.05f)
    {
        if (!animator || clip == null) return;
        animator.CrossFadeInFixedTime(clip.name, fade, 0);
    }

    // 현재 상태 완료 여부 확인
    public bool IsCurrentStateFinished(float normalizedThreshold = 1.0f)
    {
        if (!animator) return true;
        var info = animator.GetCurrentAnimatorStateInfo(0);
        return info.normalizedTime >= normalizedThreshold;
    }
    #endregion
    

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(startPoint.position, weaponData.range);
        Gizmos.DrawWireSphere(endPoint.position, weaponData.range);
    }
#endif
}
