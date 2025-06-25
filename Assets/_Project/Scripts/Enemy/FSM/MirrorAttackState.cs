using System.Collections.Generic;
using UnityEngine;

public class MirrorAttackState : EnemyAttackState
{
    private float lastAttackTime;
    private bool clonesSpawned = false;
    private List<GameObject> spawnedClones = new List<GameObject>();

    public override void Enter(EnemyStateMachine enemy)
    {
        enemy.Agent.isStopped = true;
        if(!clonesSpawned)
        {
            SpawnClones(enemy);
            SwapWithClone(enemy);
            clonesSpawned = true;
        }
        Attack(enemy);
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
            Attack(enemy);
        }
    }

    public override void Exit(EnemyStateMachine enemy)
    {
        enemy.Agent.isStopped = false;
        clonesSpawned = false;
        spawnedClones.Clear();
    }

    private void SpawnClones(EnemyStateMachine enemy)
    {
        for(int i = 0; i < enemy.Enemy.NumberOfClones; i++)
        {
            Vector3 offset = Random.insideUnitSphere * enemy.Enemy.CloneSpread;
            offset.y = 0;
            Vector3 spawnPosition = enemy.transform.position + offset;

            GameObject clone = GameObject.Instantiate(enemy.Enemy.FakeClonePrefab, spawnPosition, Quaternion.identity);

            if(!clone.TryGetComponent(out FakeClone fakeClone))
            {
                fakeClone = clone.AddComponent<FakeClone>();
            }

            fakeClone.Initialize(enemy.Enemy);
            spawnedClones.Add(clone);
        }
    }

    private void SwapWithClone(EnemyStateMachine enemy)
    {
        if(spawnedClones.Count == 0) return;
        
        int index = Random.Range(0, spawnedClones.Count);
        GameObject clone = spawnedClones[index];

        Vector3 tempPosition = enemy.transform.position;
        enemy.transform.position = clone.transform.position;
        clone.transform.position = tempPosition;
    }

    private void Attack(EnemyStateMachine enemy)
    {
        enemy.Animator.SetTrigger("IsAttacking");
        lastAttackTime = Time.time;
        
        // Enemy의 PerformAttack 메서드 호출
        // enemy.Enemy.PerformAttack();
    }
}
