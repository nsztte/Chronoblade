using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : EnemyBaseState
{
    private int index;
    private float waitTimer;

    public override void Enter(EnemyStateMachine enemy)
    {
        enemy.Agent.isStopped = false;
        var e = enemy.Enemy;
        e.ResetDetection();

        if (e.PatrolMode == PatrolMode.WaypointsLoop)
        {
            if (e.PatrolPoints == null || e.PatrolPoints.Length == 0)
            { 
                enemy.TransitionToState(enemy.IdleState);
                return;
            }

            index = e.StartAtNearest ? NearestIndex(e.transform.position, e.PatrolPoints) : 0;
            enemy.Agent.SetDestination(e.PatrolPoints[index].position);
        }
        else if (e.PatrolMode == PatrolMode.RandomInRadius)
        {
            enemy.Agent.SetDestination(RandomAround(e.HomePosition, e.PatrolRadius));
        }
        else 
        {
            enemy.TransitionToState(enemy.IdleState);
        }
    }

    public override void Update(EnemyStateMachine enemy)
    {
        var e = enemy.Enemy;
        if (e.CanSeePlayer())
        {
            e.DetectPlayer();
            return; 
        }

        if (!enemy.Agent.pathPending && enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer < e.WaitAtPoint) return;

            waitTimer = 0f;

            if (e.PatrolMode == PatrolMode.RandomInRadius)
            {
                enemy.Agent.SetDestination(RandomAround(e.HomePosition, e.PatrolRadius));
            }
            else
            {
                index = (index + 1) % e.PatrolPoints.Length;
                enemy.Agent.SetDestination(e.PatrolPoints[index].position);
            }
        }
    }
    
    public override void Exit(EnemyStateMachine enemy)
    {
        enemy.Agent.isStopped = true;
    }

    private int NearestIndex(Vector3 from, Transform[] points)
    {
        if (points == null || points.Length == 0) return 0;

        int best = 0;
        float bestSqr = float.MaxValue;

        for (int i = 0; i < points.Length; i++)
        {
            var p = points[i];
            if (p == null) continue;

            float sqr = (p.position - from).sqrMagnitude;
            if (sqr < bestSqr)
            {
                bestSqr = sqr;
                best = i;
            }
        }
        return best;
    }

    Vector3 RandomAround(Vector3 center, float radius, float sampleMaxDistance = 2f, int attempts = 3)
    {
        for (int i = 0; i < attempts; i++)
        {
            var v = Random.insideUnitSphere * radius;
            v.y = 0f;
            var raw = center + v;

            if (NavMesh.SamplePosition(raw, out var hit, sampleMaxDistance, NavMesh.AllAreas))
                return hit.position;
        }

        // 폴백: 중심점 자체를 샘플
        if (NavMesh.SamplePosition(center, out var centerHit, sampleMaxDistance, NavMesh.AllAreas))
            return centerHit.position;

        return center;
    }
}
