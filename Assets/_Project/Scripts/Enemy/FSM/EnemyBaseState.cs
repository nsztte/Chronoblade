using UnityEngine;
public abstract class EnemyBaseState
{
    public abstract void Enter(EnemyStateMachine enemy);
    public abstract void Update(EnemyStateMachine enemy);
    public abstract void Exit(EnemyStateMachine enemy);
}
