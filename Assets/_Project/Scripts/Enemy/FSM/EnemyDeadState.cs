using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    public override void Enter(EnemyStateMachine enemy)
    {
        enemy.Agent.isStopped = true;
        enemy.Animator.SetBool("IsDead", true);
        enemy.Enemy.Die();
    }

    public override void Update(EnemyStateMachine enemy){}

    public override void Exit(EnemyStateMachine enemy){}
}
