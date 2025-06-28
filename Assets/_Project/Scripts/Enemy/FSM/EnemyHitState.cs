using UnityEngine;

public class EnemyHitState : EnemyBaseState
{
    private float hitDuration = 0.2f;
    private float elapsed = 0f;
    private float lastHitTime = 0f;
    
    public override void Enter(EnemyStateMachine enemy)
    {
        enemy.Agent.isStopped = true;
        enemy.Animator.SetTrigger("IsHit");
        elapsed = 0f;
        lastHitTime = Time.time;
    }

    public override void Update(EnemyStateMachine enemy)
    {
        elapsed += enemy.Enemy.GetAdjustedDeltaTime();
        
        if (enemy.Enemy.Type == EnemyType.ChronoMonk)
        {
            if (elapsed >= hitDuration * 2f)
            {
                enemy.TransitionToState(enemy.AttackState);
                return;
            }
        }
        
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
