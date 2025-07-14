using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public override void Enter(EnemyStateMachine enemy)
    {
        enemy.Agent.isStopped = true;
    }

    public override void Update(EnemyStateMachine enemy)
    {
        float distance = Vector3.Distance(enemy.transform.position, enemy.Target.position);
        if(distance < enemy.Enemy.DetectionRange)
        {
            if(GameManager.Instance.CurrentGameState is ExplorationState || GameManager.Instance.CurrentGameState is PuzzleState)
            {
                Enemy.TriggerCombatStarted();
            }

            enemy.TransitionToState(enemy.ChaseState);
        }
    }

    public override void Exit(EnemyStateMachine enemy){}
}
