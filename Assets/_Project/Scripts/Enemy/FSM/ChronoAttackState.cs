using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;

public class ChronoAttackState : EnemyAttackState
{
    private float lastAttackTime;

    public override void Enter(EnemyStateMachine enemy)
    {
        enemy.Agent.isStopped = true;
        lastAttackTime = Time.time;
    }

    public override void Update(EnemyStateMachine enemy)
    {
        LookAtPlayer(enemy);
        float distance = Vector3.Distance(enemy.transform.position, enemy.Target.position);
        
        if(distance > enemy.Enemy.DetectionRange)
        {
            enemy.TransitionToState(enemy.ChaseState);
            return;
        }

        if(Time.time - lastAttackTime < enemy.Enemy.AttackCooldown) return;

        // 너무 가까우면 텔레포트 애니메이션 재생
        if(distance < enemy.Enemy.RetreatRange)
        {
            enemy.Animator.SetTrigger("IsTeleporting");
            Debug.Log($"크로노몽크 텔레포트 애니메이션 시작 (거리: {distance})");
            lastAttackTime = Time.time;
            return;
        }

        if(distance > enemy.Enemy.AttackRange)
        {
            enemy.Animator.SetTrigger("IsTeleporting");
            Debug.Log($"크로노몽크 기습 텔레포트 애니메이션 시작 (거리: {distance})");
            lastAttackTime = Time.time;
            return;
        }

        // 적당한 거리에서만 발사체 공격 애니메이션
        enemy.Animator.SetTrigger("IsAttacking");
        Debug.Log($"크로노몽크 발사체 공격 애니메이션 (거리: {distance})");
        
        lastAttackTime = Time.time;
    }

    public override void Exit(EnemyStateMachine enemy)
    {
        enemy.Agent.isStopped = false;
    }

    private void LookAtPlayer(EnemyStateMachine enemy)
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
}

