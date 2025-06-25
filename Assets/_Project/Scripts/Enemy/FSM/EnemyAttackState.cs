using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private float lastAttackTime;

    public override void Enter(EnemyStateMachine enemy)
    {
        enemy.Animator.SetTrigger("IsAttacking");
        lastAttackTime = Time.time;
    }

    public override void Update(EnemyStateMachine enemy)
    {
        float distance = Vector3.Distance(enemy.transform.position, enemy.Target.position);
        if(distance > enemy.Enemy.AttackRange)
        {
            enemy.TransitionToState(enemy.ChaseState);
            return;
        }

        if(Time.time - lastAttackTime > enemy.Enemy.AttackCooldown)
        {
            enemy.Animator.SetTrigger("IsAttacking");
            lastAttackTime = Time.time;
            
            enemy.Enemy.PerformAttack();
        }
    }

    public override void Exit(EnemyStateMachine enemy){}
}
