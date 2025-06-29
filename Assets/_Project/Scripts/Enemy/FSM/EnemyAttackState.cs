using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    protected float lastAttackTime;

    public override void Enter(EnemyStateMachine enemy)
    {
        enemy.Animator.SetTrigger("IsAttacking");
        Debug.Log("공격 상태 진입");
        lastAttackTime = Time.time;
    }

    public override void Update(EnemyStateMachine enemy)
    {
        LookAtPlayer(enemy);
        float distance = Vector3.Distance(enemy.transform.position, enemy.Target.position);
        
        // 공격 범위를 벗어나면 추적 상태로 전환
        if(distance > enemy.Enemy.AttackRange)
        {
            enemy.TransitionToState(enemy.ChaseState);
            return;
        }

        // 공격 쿨타임이 지났으면 공격
        if(Time.time - lastAttackTime > enemy.Enemy.AttackCooldown)
        {
            Attack(enemy);
        }
    }

    protected virtual void Attack(EnemyStateMachine enemy)
    {
        enemy.Animator.SetTrigger("IsAttacking");
        lastAttackTime = Time.time;
    }

    protected void LookAtPlayer(EnemyStateMachine enemy)
    {
        if (enemy.Target == null) return;
        
        Vector3 direction = (enemy.Target.position - enemy.transform.position).normalized;
        direction.y = 0; // Y축 회전만 적용
        
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, enemy.Enemy.GetAdjustedDeltaTime() * 5f);
        }
    }

    public override void Exit(EnemyStateMachine enemy){}
}
