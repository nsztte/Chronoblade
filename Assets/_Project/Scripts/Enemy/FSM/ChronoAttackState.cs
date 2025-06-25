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
        float distance = Vector3.Distance(enemy.transform.position, enemy.Target.position);
        
        if(distance > enemy.Enemy.DetectionRange)
        {
            enemy.TransitionToState(enemy.ChaseState);
            return;
        }

        if(Time.time - lastAttackTime < enemy.Enemy.AttackCooldown) return;

        if(distance > enemy.Enemy.AttackRange)
        {
            TryTeleport(enemy);
        }

        enemy.Animator.SetTrigger("IsAttacking");
        lastAttackTime = Time.time;
        
        // Enemy의 PerformAttack 메서드 호출
        // enemy.Enemy.PerformAttack();
    }

    public override void Exit(EnemyStateMachine enemy)
    {
        enemy.Agent.isStopped = false;
    }

    private void TryTeleport(EnemyStateMachine enemy)
    {
        Vector3 direction = (enemy.Target.position - enemy.transform.position).normalized;
        Vector3 teleportPosition = enemy.Target.position - direction * enemy.Enemy.TeleportDistance;

        NavMeshHit hit;
        if(NavMesh.SamplePosition(teleportPosition, out hit, 1f, NavMesh.AllAreas))
        {
            enemy.transform.position = hit.position;
        }
    }

    // //TODO: 플레이어 디버프 효과 추가
    // private void Attack(EnemyStateMachine enemy)
    // {
    //     enemy.Animator.SetTrigger("IsAttacking");
    //     PlayerManager.Instance.TakeDamage(enemy.Enemy.Damage);
    //     lastAttackTime = Time.time;
    // }
}
