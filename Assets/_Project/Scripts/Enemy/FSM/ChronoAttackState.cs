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

        if(distance > enemy.Enemy.AttackRange)
        {
            TryTeleport(enemy);
        }

        enemy.Animator.SetTrigger("IsAttacking");
        lastAttackTime = Time.time;
    }

    public override void Exit(EnemyStateMachine enemy)
    {
        enemy.Agent.isStopped = false;
    }

    private void TryTeleport(EnemyStateMachine enemy)
    {
        Debug.Log("TryTeleport");
        // Vector3 direction = (enemy.Target.position - enemy.transform.position).normalized;
        // Vector3 teleportPosition = enemy.Target.position - direction * enemy.Enemy.TeleportDistance;

        enemy.transform.position = enemy.Target.position;

        // NavMeshHit hit;
        // if(NavMesh.SamplePosition(teleportPosition, out hit, 1f, NavMesh.AllAreas))
        // {
        //     enemy.transform.position = hit.position;
        // }
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
