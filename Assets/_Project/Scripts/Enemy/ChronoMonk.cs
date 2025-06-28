using UnityEngine;
using UnityEngine.AI;

public class ChronoMonk : Enemy
{
    [Header("크로노몽크 발사체")]
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private ParticleSystem teleportEffect;
    [SerializeField] private ParticleSystem deathEffect;

    // ChronoMonk 전용 프로퍼티
    public float TeleportDistance => behaviorData.teleportDistance;
    public float SlowDuration => behaviorData.slowDuration;
    public GameObject ChronoProjectilePrefab => behaviorData.chronoProjectilePrefab;
    public float ProjectileSpeed => behaviorData.projectileSpeed;
    public float ProjectileLifetime => behaviorData.projectileLifetime;
    public float RetreatRange => behaviorData.retreatRange;

    public override void Die()
    {
        if(deathEffect != null)
        {
            deathEffect.Play();
        }

        if(teleportEffect != null && teleportEffect.isPlaying)
        {
            teleportEffect.Stop();
        }

        base.Die();
    }

    protected override void OnPerformAttack()
    {
        // 크로노몽크는 항상 발사체 공격만 수행
        FireChronoProjectile();
        Debug.Log("크로노몽크 발사체 공격 실행");
    }

    // 크로노몽크 구체 발사
    private void FireChronoProjectile()
    {
        if (ChronoProjectilePrefab == null || projectileSpawnPoint == null)
        {
            Debug.LogWarning("크로노몽크 발사체 프리팹 또는 스폰 포인트 없음");
            return;
        }

        // 플레이어 방향 계산
        Transform target = fsm?.Target;
        if (target == null)
        {
            Debug.LogWarning("크로노몽크 발사체 대상 없음");
            return;
        }

        // 플레이어의 실제 높이를 고려한 타겟 위치 계산
        Vector3 targetPos = target.position;
        Vector3 spawnPos = projectileSpawnPoint.position;
        
        // 플레이어의 Collider를 확인하여 적절한 높이 계산
        Collider playerCollider = target.GetComponent<Collider>();
        float targetHeight = targetPos.y;
        
        if (playerCollider != null)
        {
            targetHeight = playerCollider.bounds.center.y;  // 플레이어 중앙 높이
        }
        
        // 발사체가 날아갈 타겟 위치 (수평은 플레이어 위치, 높이는 계산된 높이)
        Vector3 adjustedTargetPos = new Vector3(targetPos.x, targetHeight, targetPos.z);
        Vector3 direction = (adjustedTargetPos - spawnPos).normalized;
        
        // 발사체 생성 및 초기화
        GameObject projectileObj = Instantiate(ChronoProjectilePrefab, projectileSpawnPoint.position, Quaternion.LookRotation(direction));
        ChronoProjectile projectile = projectileObj.GetComponent<ChronoProjectile>();
        
        if (projectile != null)
        {
            projectile.Initialize(direction, Damage, SlowDuration);
            Debug.Log($"크로노몽크 발사체 발사: {target.name} (높이: {targetHeight:F2})");
        }
        else
        {
            Debug.LogError("크로노몽크 발사체 컴포넌트 없음");
        }
    }

    // 크로노몽크 텔레포트 파티클 재생 (애니메이션 이벤트용)
    public void OnChronoTeleportParticle()
    {
        if(teleportEffect != null)
        {
            teleportEffect.Play();
        }
    }

    // 크로노몽크 텔레포트 (애니메이션 이벤트용)
    public void OnChronoTeleport()
    {
        if (fsm != null)
        {
            // 직접 텔레포트 로직 실행
            TryTeleport();
            ParticleSystem particle = GetComponentInChildren<ParticleSystem>();
            if(particle != null)
            {
                particle.Stop();
            }
            Debug.Log("크로노몽크 애니메이션 이벤트로 텔레포트 실행");
        }
    }

    // 텔레포트 로직
    private void TryTeleport()
    {
        if (fsm?.Target == null) return;
        
        Debug.Log("크로노몽크 후면 기습 텔레포트 시도");
        
        // 플레이어의 뒤쪽 방향 계산
        Vector3 direction = (fsm.Target.position - transform.position).normalized;
        
        // 플레이어 위치에서 뒤쪽으로 teleportDistance만큼 떨어진 위치 계산
        Vector3 behindPlayerPosition = fsm.Target.position - direction * TeleportDistance;
        
        // 플레이어가 바라보는 방향의 반대쪽으로 텔레포트
        Vector3 playerForward = fsm.Target.forward;
        Vector3 behindPosition = fsm.Target.position - playerForward * TeleportDistance;

        // NavMesh 위의 유효한 위치인지 확인 (후면 위치 우선)
        NavMeshHit hit;
        if(NavMesh.SamplePosition(behindPosition, out hit, 2f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
            Debug.Log($"크로노몽크 후면 기습 성공: {hit.position}");
        }
        else if(NavMesh.SamplePosition(behindPlayerPosition, out hit, 2f, NavMesh.AllAreas))
        {
            // 후면 위치가 실패하면 기존 로직 사용
            transform.position = hit.position;
            Debug.Log($"크로노몽크 대체 위치 텔레포트: {hit.position}");
        }
        else
        {
            // 모든 위치가 실패하면 플레이어 위치로 텔레포트
            transform.position = fsm.Target.position;
            Debug.Log("크로노몽크 텔레포트 실패, 플레이어 위치로 이동");
        }
    }

#if UNITY_EDITOR
    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        
        // 발사체 스폰 포인트 표시
        if (projectileSpawnPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(projectileSpawnPoint.position, 0.2f);
        }
    }
#endif
}
