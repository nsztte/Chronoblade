using UnityEngine;

public class EnemyHitState : EnemyBaseState
{
    private float hitDuration = 0.5f;
    private float elapsed = 0f;
    public override void Enter(EnemyStateMachine enemy)
    {
        enemy.Agent.isStopped = true;
        enemy.Animator.SetTrigger("IsHit");
        elapsed = 0f;
    }

    public override void Update(EnemyStateMachine enemy)
    {
       elapsed += Time.deltaTime;
       if(elapsed >= hitDuration)
       {
            enemy.TransitionToState(enemy.ChaseState);
       }
    }

    public override void Exit(EnemyStateMachine enemy)
    {
        enemy.Agent.isStopped = false;
    }
}
