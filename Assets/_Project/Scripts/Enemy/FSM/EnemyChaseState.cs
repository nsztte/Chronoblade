using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    // 거리 계산 캐싱
    private float cachedDistance;
    private float lastDistanceUpdate;
    private const float DISTANCE_UPDATE_INTERVAL = 0.1f;

    // 추격 해제
    private float lostSightTimer;
    private const float LOST_SIGHT_TIME = 3.0f;    // 시야에서 3초 연속으로 놓치면 복귀
    // private const float LEASH_DISTANCE = 50f;      // 홈 기준 리쉬

    public override void Enter(EnemyStateMachine enemy)
    {
        enemy.Agent.isStopped = false;
        lastDistanceUpdate = 0f;
        lostSightTimer = 0f;
    }

    public override void Update(EnemyStateMachine enemy)
    {
        if (!enemy.Enemy.CanSeePlayer()) lostSightTimer += Time.deltaTime;
        else lostSightTimer = 0f;

        if (lostSightTimer >= LOST_SIGHT_TIME)
        {
            // PatrolMode에 따라 복귀 목적지 다르게
            if (enemy.Enemy.PatrolMode == PatrolMode.None)
                enemy.TransitionToState(enemy.IdleState);
            else
                enemy.TransitionToState(enemy.PatrolState);
            return;
        }
        
        float distance = GetCachedDistance(enemy);

        // 미러 듀얼리스트 스폰
        if (enemy.Enemy.Type == EnemyType.MirrorDuelist)
        {
            var duelist = enemy.Enemy as MirrorDuelist;

            // 클론 스폰 중이면 이동 금지
            if (duelist.IsSpawning)
            {
                enemy.Agent.isStopped = true;
                return;
            }

            enemy.Agent.isStopped = false; // 클론 소환이 끝났다면 다시 이동 허용

            if (distance < duelist.DetectionRange && !duelist.HasActiveClones() && duelist.CanSpawnClone())
            {
                enemy.Animator.SetTrigger("IsSpawnClones");
                duelist.MarkCloneSpawned();
                Debug.Log("MirrorDuelist 클론 소환 트리거 발동");
            }
        }
        
        // 이동
        enemy.Agent.SetDestination(enemy.Target.position);
        
        // 크로노몽크 거리 로직 적용
        if (enemy.Enemy.Type == EnemyType.ChronoMonk)
        {
            ChronoMonk chronoMonk = enemy.Enemy as ChronoMonk;
            
            // 너무 가까우면 즉시 공격 상태로 전환 (텔레포트 실행)
            if (distance < chronoMonk.RetreatRange)
            {
                enemy.TransitionToState(enemy.AttackState);
                return;
            }
            
            // 공격 범위 안에 들어오면 공격 상태로 전환
            if (distance < enemy.Enemy.AttackRange)
            {
                enemy.TransitionToState(enemy.AttackState);
                return;
            }
        }
        else
        {
            // 다른 적들은 기존 로직 사용
            if (distance < enemy.Enemy.AttackRange)
            {
                enemy.TransitionToState(enemy.AttackState);
            }
        }
    }

    // 캐시된 거리 계산
    private float GetCachedDistance(EnemyStateMachine enemy)
    {
        if (enemy.Target == null) return float.MaxValue;
        
        if (Time.time - lastDistanceUpdate > DISTANCE_UPDATE_INTERVAL)
        {
            cachedDistance = Vector3.Distance(enemy.transform.position, enemy.Target.position);
            lastDistanceUpdate = Time.time;
        }
        return cachedDistance;
    }

    public override void Exit(EnemyStateMachine enemy)
    {
        enemy.Agent.isStopped = true;

        if(enemy.Enemy.Type == EnemyType.MirrorDuelist)
        {
            var duelist = enemy.Enemy as MirrorDuelist;
            duelist.OnMirrorSpawnEnd();
        }
    }
}
