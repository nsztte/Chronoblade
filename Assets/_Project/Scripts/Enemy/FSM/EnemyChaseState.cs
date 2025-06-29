using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    // 거리 계산 캐싱
    private float cachedDistance;
    private float lastDistanceUpdate;
    private const float DISTANCE_UPDATE_INTERVAL = 0.1f;

    public override void Enter(EnemyStateMachine enemy)
    {
        enemy.Agent.isStopped = false;
        lastDistanceUpdate = 0f; // 캐시 초기화
        // enemy.Animator.SetBool("IsChasing", true);
    }

    public override void Update(EnemyStateMachine enemy)
    {
        enemy.Agent.SetDestination(enemy.Target.position);
        // Debug.Log($"Enemy Destination: {enemy.Agent.destination}");

        float distance = GetCachedDistance(enemy);
        
        // 크로노몽크의 경우 특별한 거리 로직 적용
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
        // enemy.Animator.SetBool("IsChasing", false);
    }
}
