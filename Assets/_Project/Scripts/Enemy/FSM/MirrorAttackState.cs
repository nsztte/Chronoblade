using System.Collections.Generic;
using UnityEngine;

// 현재는 EnemyAttackState로 통합되어 미사용 중
// MirrorDuelist가 특수한 공격 FSM이 필요해질 경우 복원 예정
public class MirrorAttackState : EnemyAttackState
{
    public bool isAttacking = false;

    public override void Enter(EnemyStateMachine enemy)
    {
        // enemy.Agent.isStopped = true;
    }

    public override void Update(EnemyStateMachine enemy)
    {
        LookAtPlayer(enemy);
        float distance = GetCachedDistance(enemy);
        
        // 공격 중이거나 스폰 중일 때는 chase 상태로 전환하지 않음
        if(distance > enemy.Enemy.AttackRange && !isAttacking)
        {
            enemy.TransitionToState(enemy.ChaseState);
            return;
        }

        // 그 외에는 일반 공격 쿨타임
        if(Time.time - lastAttackTime > enemy.Enemy.AttackCooldown)
            Attack(enemy);
    }

    protected override void Attack(EnemyStateMachine enemy)
    {
        base.Attack(enemy);
        isAttacking = true;
    }

    public override void Exit(EnemyStateMachine enemy)
    {
        // enemy.Agent.isStopped = false;
        isAttacking = false;
    }

    // 애니메이션 이벤트로 호출될 메서드 (공격 완료)
    public void OnMirrorAttackEnd()
    {
        isAttacking = false;
    }
}
