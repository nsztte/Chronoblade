using System.Collections;
using UnityEngine;

public class BaseBossAttackState : BaseBossState
{
    protected string animationTrigger;
    protected string animationStateName;
    protected bool isWindingUp = true;
    protected float windingUpDelay = 0.5f;  // 상속받는 스크립트에서 초기화
    public BaseBossAttackState(BossController boss, BossStateMachine stateMachine, string animationTrigger, string stateName) : base(boss, stateMachine)
    {
        this.animationTrigger = animationTrigger;
        this.animationStateName = stateName;
    }

    public override void Enter()
    {
        isWindingUp = true;
        boss.PlayAnimation(animationTrigger);

        // 공격전 예고 구간 동안은 플레이어 방향으로 회전
        boss.StartCoroutine(EndWindingUpAfterDelay(windingUpDelay));

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

    private IEnumerator EndWindingUpAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isWindingUp = false;
    }
}
