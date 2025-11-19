using UnityEngine;

[CreateAssetMenu(menuName="GameState/Puzzle")]
public class PuzzleState : GameBaseState
{
    public override void Enter()
    {
        Debug.Log("[GameState] PuzzleState Enter");

        // 퍼즐 상태 진입 시 저장 차단
        SaveGuard.Instance?.Block(SaveBlockTag.Puzzle);  

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
        // 퍼즐 상태 종료 시 안전 복구
        SaveGuard.Instance?.ClearTag(SaveBlockTag.Puzzle);

        TimeManager.Instance.InitializeTimeState();
        UIManager.Instance.ClearTimeState();
    }
}
