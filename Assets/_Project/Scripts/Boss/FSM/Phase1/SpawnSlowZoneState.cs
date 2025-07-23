using UnityEngine;

public class SpawnSlowZoneState : BaseBossState
{
    public SpawnSlowZoneState(BossController boss, BossStateMachine stateMachine) : base(boss, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("SpawnSlowZoneState: 슬로우 존 생성");

        boss.PlayAnimation("SpawnSlowZone");
        // TODO: 이펙트 생성, 위치 계산 등 처리

        float duration = boss.GetAnimationClipLengthFromState("SpawnSlowZone");
        WaitAndChangeToState(duration, new CheckPhaseEndState(boss, stateMachine));
    }
    
    public override void Update()
    {
        boss.LookAtPlayer(6f);
    }

    public void SpawnSlowZone()
    {
        Vector3 center = boss.Player.position;
        int count = 3;  // 슬로우 존 개수
        float radius = 3f;  // 슬로우 존 반경

        for(int i = 0; i < count; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * radius;
            Vector3 spawnPosition = center + new Vector3(randomCircle.x, 0, randomCircle.y);

            boss.SpawnSlowZoneAtPosition(spawnPosition);
            Debug.Log($"슬로우 존 생성: {spawnPosition}");
        }
    }
}
