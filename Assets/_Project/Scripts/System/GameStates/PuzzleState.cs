using UnityEngine;

public class PuzzleState : GameBaseState
{
    public override void Enter()
    {
        Debug.Log("[GameState] PuzzleState Enter");
        UIManager.Instance.ShowPuzzleUI();

        var previousState = gameManager.PreviousGameState;

        if(previousState is MainMenuState || previousState is LoadingState || previousState is GameOverState || previousState is CutsceneState)
        {
            TimeManager.Instance.InitializeTimeState();
        }
        else
        {
            TimeManager.Instance.SetTimeState(TimeManager.Instance.CurrentTimeState);
        }
    }

    public override void Exit()
    {
        UIManager.Instance.HidePuzzleUI();
    }
}
