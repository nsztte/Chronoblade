using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : EnemyBaseState
{
    private int index;
    private float waitTimer;

    // 제자리 회전용 필드
    private bool isTurning;
    private Vector3 nextPatrolDestination;

    // 회전 속도, 각도
    // private const float TurnSpeedDegPerSecond = 300f;   // 도/초
    private const float TurnAngleThreshold = 5f;        //도

    public override void Enter(EnemyStateMachine enemy)
    {
        enemy.Agent.isStopped = false;
        var e = enemy.Enemy;
        e.ResetDetection();

        waitTimer = 0f;
        isTurning = false;

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

        // 1) 플레이어 감지
        if (e.CanSeePlayer())
        {
            e.DetectPlayer();
            return; 
        }

        // 2) 회전 처리
        if (isTurning)
        {
            enemy.Animator.SetFloat("Speed", 0.7f);
            HandleTurn(enemy);
            return;
        }

        // 3) 목적지 도착, 기다리기
        var agent = enemy.Agent;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer < e.WaitAtPoint) return;

            waitTimer = 0f;

            // 4) 다음 목적지 계산 (이동은 회전로직 내에서 처리)
            if (e.PatrolMode == PatrolMode.RandomInRadius)
            {
                // agent.SetDestination(RandomAround(e.HomePosition, e.PatrolRadius));
                nextPatrolDestination = RandomAround(e.HomePosition, e.PatrolRadius);
            }
            else
            {
                index = (index + 1) % e.PatrolPoints.Length;
                // agent.SetDestination(e.PatrolPoints[index].position);
                nextPatrolDestination = e.PatrolPoints[index].position;
            }

            // 5) 회전 시작
            isTurning = true;
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

    private Vector3 RandomAround(Vector3 center, float radius, float sampleMaxDistance = 2f, int attempts = 3)
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

    private void HandleTurn(EnemyStateMachine enemy)
    {
        var e = enemy.Enemy;
        var t = e.transform;

        // 다음 목적지 방향 (수평)
        Vector3 dir = nextPatrolDestination - t.position;
        dir.y = 0f;

        // 회전이 목적지 근처면 바로 이동
        if (dir.sqrMagnitude < 0.01f)
        {
            isTurning = false;
            enemy.Agent.SetDestination(nextPatrolDestination);
            return;
        }

        Quaternion targetRot = Quaternion.LookRotation(dir);
        float maxStep = e.BehaviorData.turnSpeedDegPerSecond * Time.deltaTime;

        // 현재 회전에서 타겟 회전으로 천천히 회전
        t.rotation = Quaternion.RotateTowards(t.rotation, targetRot, maxStep);

        // 남은 각도 확인해서 충분히 돌았으면 이동 시작
        float remainingAngle = Quaternion.Angle(t.rotation, targetRot);
        if (remainingAngle <= TurnAngleThreshold)
        {
            isTurning = false;
            enemy.Agent.SetDestination(nextPatrolDestination);
        }
    }
}
