
public class BossStateMachine
{
    public BaseBossState CurrentState { get; private set; }

    public void Initialize(BaseBossState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(BaseBossState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public void Update()
    {
        CurrentState?.Update();
    }
}
