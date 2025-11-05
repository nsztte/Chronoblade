using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private float patrolDelay = 0.3f; // 짧은 지연으로 깔끔하게
    private float timer;

    public override void Enter(EnemyStateMachine enemy)
    {
        enemy.Agent.isStopped = true;
        timer = 0f;
        enemy.Enemy.ResetDetection();
    }

    public override void Update(EnemyStateMachine enemy)
    {
        if (enemy.Enemy.CanSeePlayer())
        {
            enemy.Enemy.DetectPlayer();
            return;
        }

        if (enemy.Enemy.PatrolMode != PatrolMode.None)
        {
            timer += Time.deltaTime;
            if (timer >= patrolDelay)
            {
                enemy.TransitionToState(enemy.PatrolState);
                return;
            }
        }
    }

    public override void Exit(EnemyStateMachine enemy){}
}
