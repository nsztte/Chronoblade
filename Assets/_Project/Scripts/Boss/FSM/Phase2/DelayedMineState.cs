using UnityEngine;

public class DelayedMineState : BaseBossState
{
    public DelayedMineState(BossController boss, BossStateMachine stateMachine) : base(boss, stateMachine)
    {

    }

    public override void Enter()
    {
        Debug.Log("DelayedMineState: 지뢰 생성");

        boss.PlayAnimation("DelayedMine");

        float duration = boss.GetAnimationClipLengthFromState("DelayedMine");
        WaitAndChangeToState(duration, new CheckPhaseEndState(boss, stateMachine));
    }

    public override void Update()
    {
        boss.LookAtPlayer(6f);
    }

    public void SpawnMine()
    {
        Vector3 center = boss.Player.position;
        int count = 3;
        float radius = 3f;

        for(int i = 0; i < count; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * radius;
            Vector3 spawnPosition = center + new Vector3(randomCircle.x, 0, randomCircle.y);

            boss.SpawnMineAtPosition(spawnPosition);
            Debug.Log($"지뢰 생성: {spawnPosition}");
        }
    }
}
