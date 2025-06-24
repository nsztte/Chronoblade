using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public override void Enter(EnemyStateMachine enemy)
    {
        enemy.Agent.isStopped = false;
        enemy.Animator.SetBool("IsChasing", true);
    }

    public override void Update(EnemyStateMachine enemy)
    {
        enemy.Agent.SetDestination(enemy.Target.position);

        float distance = Vector3.Distance(enemy.transform.position, enemy.Target.position);
        if (distance < enemy.Enemy.AttackRange)
        {
            enemy.TransitionToState(enemy.AttackState);
        }
    }

    public override void Exit(EnemyStateMachine enemy)
    {
        enemy.Agent.isStopped = true;
        enemy.Animator.SetBool("IsChasing", false);
    }
}
