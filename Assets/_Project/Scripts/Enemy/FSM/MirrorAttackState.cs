using System.Collections.Generic;
using UnityEngine;

public class MirrorAttackState : EnemyAttackState
{
    public bool isAttacking = false;
    public bool isSpawned = false;

    public override void Enter(EnemyStateMachine enemy)
    {
        enemy.Agent.isStopped = true;
        SpawnClones(enemy);
    }

    public override void Update(EnemyStateMachine enemy)
    {
        LookAtPlayer(enemy);
        float distance = Vector3.Distance(enemy.transform.position, enemy.Target.position);
        
        // 공격 중이거나 스폰 중일 때는 chase 상태로 전환하지 않음
        if(distance > enemy.Enemy.AttackRange && !isAttacking && !isSpawned)
        {
            enemy.TransitionToState(enemy.ChaseState);
            return;
        }

        if(Time.time - lastAttackTime > enemy.Enemy.AttackCooldown && !isSpawned)
        {
            Attack(enemy);
        }
    }

    protected override void Attack(EnemyStateMachine enemy)
    {
        base.Attack(enemy);
        isAttacking = true;
    }

    private void SpawnClones(EnemyStateMachine enemy)
    {
        enemy.Animator.SetTrigger("IsSpawnClones");
        isSpawned = true;
    }

    public override void Exit(EnemyStateMachine enemy)
    {
        enemy.Agent.isStopped = false;
        isAttacking = false;
        isSpawned = false;
    }

    // 애니메이션 이벤트로 호출될 메서드 (공격 완료)
    public void OnMirrorAttackEnd()
    {
        isAttacking = false;
    }

    // 애니메이션 이벤트로 호출될 메서드 (클론 스폰 완료)
    public void OnMirrorSpawnEnd()
    {
        isSpawned = false;
    }
}
