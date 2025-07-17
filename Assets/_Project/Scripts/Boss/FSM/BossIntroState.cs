using UnityEngine;

public class BossIntroState : BaseBossState
{
    public BossIntroState(BossController boss, BossStateMachine stateMachine) : base(boss, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("BossIntroState: 보스 등장 시작");

        boss.PlayAnimation("Intro");

        float duration = boss.GetCurrentAnimationLength() + 0.2f;
        WaitAndReturnToIdle(duration);
    }
}
