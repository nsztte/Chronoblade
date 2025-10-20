using UnityEngine;

public class StaggerCheckState : BaseBossState
{
    private readonly float staggerDur;

    public StaggerCheckState(BossController boss, BossStateMachine sm, float staggerDuration) : base(boss, sm)
    {
        staggerDur = staggerDuration;
    }

    public override void Enter()
    {
        Debug.Log("StaggerCheckState 진입");

        boss.PlayAnimation("Stagger");

        WaitAndChangeToState(staggerDur, new CheckPhaseEndState(boss, stateMachine));
    }
}
