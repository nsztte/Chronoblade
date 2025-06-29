using UnityEngine;
using UnityEngine.AI;

public class ChronoAttackState : EnemyAttackState
{
    public override void Enter(EnemyStateMachine enemy)
    {
        enemy.Agent.isStopped = true;
        lastAttackTime = 0;
    }

    public override void Update(EnemyStateMachine enemy)
    {
        LookAtPlayer(enemy);
        float distance = GetCachedDistance(enemy);
        
        if(distance > enemy.Enemy.DetectionRange)
        {
            enemy.TransitionToState(enemy.ChaseState);
            return;
        }

        if(Time.time - lastAttackTime < enemy.Enemy.AttackCooldown) return;

        var chronoMonk = GetChronoMonk(enemy);
        if (chronoMonk == null) return;

        // 확률적 텔레포트 (10% 확률로 랜덤 텔레포트)
        if(Random.Range(0f, 1f) < 0.1f && distance > chronoMonk.RetreatRange)
        {
            enemy.Animator.SetTrigger("IsTeleporting");
            Debug.Log($"크로노몽크 확률적 텔레포트 (거리: {distance})");
            lastAttackTime = Time.time;
            return;
        }

        // 너무 가까우면 텔레포트 애니메이션 재생
        if(distance < chronoMonk.RetreatRange)
        {
            enemy.Animator.SetTrigger("IsTeleporting");
            Debug.Log($"크로노몽크 텔레포트 애니메이션 시작 (거리: {distance})");
            lastAttackTime = Time.time;
            return;
        }

        if(distance > enemy.Enemy.AttackRange)
        {
            enemy.Animator.SetTrigger("IsTeleporting");
            Debug.Log($"크로노몽크 기습 텔레포트 애니메이션 시작 (거리: {distance})");
            lastAttackTime = Time.time;
            return;
        }

        // 적당한 거리에서만 발사체 공격 애니메이션
        Attack(enemy);
    }

    public override void Exit(EnemyStateMachine enemy)
    {
        enemy.Agent.isStopped = false;
    }

    private void TryTeleport(EnemyStateMachine enemy)
    {
        if (enemy?.Target == null) return;

        var chronoMonk = GetChronoMonk(enemy);
        if (chronoMonk == null) return;

        Debug.Log("크로노몽크 후면 기습 텔레포트 시도");
        
        // 플레이어의 뒤쪽 방향 계산
        Vector3 direction = (enemy.Target.position - enemy.transform.position).normalized;
        
        // 플레이어 위치에서 뒤쪽으로 teleportDistance만큼 떨어진 위치 계산
        Vector3 behindPlayerPosition = enemy.Target.position - direction * chronoMonk.TeleportDistance;
        
        // 플레이어가 바라보는 방향의 반대쪽으로 텔레포트
        Vector3 playerForward = enemy.Target.forward;
        Vector3 behindPosition = enemy.Target.position - playerForward * chronoMonk.TeleportDistance;

        // NavMesh 위의 유효한 위치인지 확인 (후면 위치 우선)
        NavMeshHit hit;
        if(NavMesh.SamplePosition(behindPosition, out hit, 2f, NavMesh.AllAreas))
        {
            enemy.transform.position = hit.position;
            Debug.Log($"크로노몽크 후면 기습 성공: {hit.position}");
        }
        else if(NavMesh.SamplePosition(behindPlayerPosition, out hit, 2f, NavMesh.AllAreas))
        {
            // 후면 위치가 실패하면 기존 로직 사용
            enemy.transform.position = hit.position;
            Debug.Log($"크로노몽크 대체 위치 텔레포트: {hit.position}");
        }
        else
        {
            // 모든 위치가 실패하면 플레이어 위치로 텔레포트
            enemy.transform.position = enemy.Target.position;
            Debug.Log("크로노몽크 텔레포트 실패, 플레이어 위치로 이동");
        }
    }

    // ChronoMonk 컴포넌트 가져오기
    private ChronoMonk GetChronoMonk(EnemyStateMachine enemy)
    {
        if (enemy?.Enemy == null)
        {
            Debug.LogError("에너미스테이트머신 또는 에너미가 null");
            return null;
        }

        var chronoMonk = enemy.Enemy as ChronoMonk;
        if (chronoMonk == null)
        {
            Debug.LogError($"에너미가 크로노몽크가 아님, 실제 타입: {enemy.Enemy.GetType().Name}");
            return null;
        }

        return chronoMonk;
    }

    // 애니메이션 이벤트용 텔레포트 메서드
    public void OnTeleport(EnemyStateMachine enemy)
    {
        TryTeleport(enemy);
        Debug.Log("크로노몽크 애니메이션 이벤트로 텔레포트 실행");
    }
}

