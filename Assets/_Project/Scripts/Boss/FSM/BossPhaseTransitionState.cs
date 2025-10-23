using UnityEngine;

public class BossPhaseTransitionState : BaseBossState
{
    public BossPhaseTransitionState(BossController boss, BossStateMachine stateMachine) : base(boss, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("BossPhaseTransitionState: 보스 패이즈2 시작");
        
        boss.PlayAnimation("Intro");

        float duration = boss.GetCurrentAnimationLength() + 1.5f;
        WaitAndChangeToState(duration, new BossIdleState(boss, stateMachine));
    }

    public override void Exit()
    {
        boss.PhaseManager.SetPhase(BossPhase.Phase2);
        boss.ShowBossHUD();

        GameManager.Instance.EnterCombat();
    }
}
