using UnityEngine;

public enum GameState
{
    MainMenu,
    Loading,
    Exploration,
    Combat,
    Puzzle,
    Cutscene,
    Paused,
    GameOver
}

public class GameStateMachine : MonoBehaviour
{
    [SerializeField] private GameBaseState currentState;
    public GameBaseState CurrentState => currentState;

    public void ChangeState(GameBaseState newState)
    {
        if(CurrentState == newState) return;

        CurrentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
