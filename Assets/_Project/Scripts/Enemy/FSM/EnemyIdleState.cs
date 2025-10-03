using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public override void Enter(EnemyStateMachine enemy)
    {
        enemy.Agent.isStopped = true;
    }

    public override void Update(EnemyStateMachine enemy)
    {
        if (enemy.Enemy.CanSeePlayer())
        {
            enemy.Enemy.DetectPlayer();
        }
    }

    public override void Exit(EnemyStateMachine enemy){}
}
