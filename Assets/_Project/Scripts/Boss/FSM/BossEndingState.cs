using UnityEngine;

public class BossEndingState : BaseBossState
{
    public BossEndingState(BossController boss, BossStateMachine stateMachine) : base(boss, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("BossEndingState: 보스 엔딩 시작");
        boss.HideBossHUD();
    }
}
