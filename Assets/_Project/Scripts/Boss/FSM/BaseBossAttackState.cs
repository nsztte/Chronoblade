
public class BaseBossAttackState : BaseBossState
{
    protected string animationTrigger;
    protected string animationStateName;
    public BaseBossAttackState(BossController boss, BossStateMachine stateMachine, string animationTrigger, string stateName) : base(boss, stateMachine)
    {
        this.animationTrigger = animationTrigger;
        this.animationStateName = stateName;
    }

    public override void Enter()
    {
        boss.PlayAnimation(animationTrigger);

        float duration = boss.GetAnimationClipLengthFromState(animationStateName);
        WaitAndReturnToIdle(duration);
    }
}
