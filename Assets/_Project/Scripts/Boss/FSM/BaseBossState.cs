using System.Collections;
using UnityEngine;

public abstract class BaseBossState
{
    protected BossController boss;
    protected BossStateMachine stateMachine;
    protected Coroutine transitionCoroutine;

    public BaseBossState(BossController boss, BossStateMachine stateMachine)
    {
        this.boss = boss;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }

    protected void WaitAndChangeToState(float delay, BaseBossState nextState)
    {
        transitionCoroutine = boss.StartCoroutine(WaitAndChangeCoroutine(delay, nextState));
    }

    protected void StopTransitionCoroutine()
    {
        if(transitionCoroutine != null)
        {
            boss.StopCoroutine(transitionCoroutine);
            transitionCoroutine = null;
        }
    }

    private IEnumerator WaitAndChangeCoroutine(float delay, BaseBossState nextState)
    {
        yield return new WaitForSeconds(delay);
        stateMachine.ChangeState(nextState);
    }
}
