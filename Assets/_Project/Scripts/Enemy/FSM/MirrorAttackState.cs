using System.Collections.Generic;
using UnityEngine;

public class MirrorAttackState : EnemyAttackState
{
    public bool isAttacking = false;
    public bool isSpawned = false;
    private float lastCloneSpawnTime = -999f;
    private float cloneCooldown = 12f;

    public override void Enter(EnemyStateMachine enemy)
    {
        enemy.Agent.isStopped = true;
    }

    public override void Update(EnemyStateMachine enemy)
    {
        LookAtPlayer(enemy);
        float distance = GetCachedDistance(enemy);
        
        // 공격 중이거나 스폰 중일 때는 chase 상태로 전환하지 않음
        if(distance > enemy.Enemy.AttackRange && !isAttacking && !isSpawned)
        {
            enemy.TransitionToState(enemy.ChaseState);
            return;
        }

        // 스폰 우선
        if(!isSpawned)
            SpawnClones(enemy);

        // 그 외에는 일반 공격 쿨타임
        if(Time.time - lastAttackTime > enemy.Enemy.AttackCooldown && !isSpawned)
            Attack(enemy);
    }

    protected override void Attack(EnemyStateMachine enemy)
    {
        base.Attack(enemy);
        isAttacking = true;
    }

    private void SpawnClones(EnemyStateMachine enemy)
    {
        var duelist = enemy.Enemy as MirrorDuelist;
        if (duelist == null)
        {
            Debug.LogWarning("MirrorDuelist로 캐스팅 실패");
            return;
        }

        // 쿨타임 체크
        if (Time.time - lastCloneSpawnTime < cloneCooldown)
        {
            Debug.Log("클론 소환 쿨타임 미충족 - 생성 생략");
            return;
        }

        // 기존 클론 존재 시 스킵
        if (duelist.HasActiveClones())
        {
            Debug.Log("기존 클론 존재 - 재소환 생략");
            return;
        }
        
        enemy.Animator.SetTrigger("IsSpawnClones");
        isSpawned = true;
        lastCloneSpawnTime = Time.time;
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
