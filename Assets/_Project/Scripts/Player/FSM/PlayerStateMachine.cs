using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerBaseState currentState;

    private void Start()
    {
        ChangeState(new PlayerLocomotionState(this));
    }

    private void Update()
    {
        currentState?.Update();
    }

    public void ChangeState(PlayerBaseState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
}
