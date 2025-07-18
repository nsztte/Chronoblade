using System.Collections;
using UnityEngine;

public class BaseBossAttackState : BaseBossState
{
    protected string animationTrigger;
    protected string animationStateName;
    public bool isWindingUp = true;

    public BaseBossAttackState(BossController boss, BossStateMachine stateMachine, string animationTrigger, string stateName) : base(boss, stateMachine)
    {
        this.animationTrigger = animationTrigger;
        this.animationStateName = stateName;
    }

    public override void Enter()
    {
        isWindingUp = true;
        boss.PlayAnimation(animationTrigger);

        float duration = boss.GetAnimationClipLengthFromState(animationStateName);
        WaitAndChangeToState(duration, new CheckPhase1EndState(boss, stateMachine));
    }

    public override void Update()
    {
        if(isWindingUp)
        {
            boss.LookAtPlayer();
        }
    }
}
