using UnityEngine;

public abstract class PlayerBaseState
{
    protected PlayerStateMachine stateMachine;

    protected virtual float MovementFactor => 1f;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void UpdateMovement()
    {
        stateMachine.PlayerController.LocomotionUpdate(MovementFactor);
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
