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
        WaitAndChangeToState(duration, new CheckPhase1EndState(boss, stateMachine));
    }
}
