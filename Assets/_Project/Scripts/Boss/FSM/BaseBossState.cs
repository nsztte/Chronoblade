using UnityEngine;

public abstract class BaseBossState
{
    protected BossController boss;
    protected BossStateMachine stateMachine;

    public BaseBossState(BossController boss, BossStateMachine stateMachine)
    {
        this.boss = boss;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
