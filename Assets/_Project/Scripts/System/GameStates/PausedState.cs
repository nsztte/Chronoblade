using UnityEngine;

[CreateAssetMenu(menuName="GameState/Paused")]
public class PausedState : GameBaseState
{
    public override void Enter()
    {
        Debug.Log("[GameState] PausedState Enter");

        if(!UIManager.Instance.IsAnyUIOpen)
            UIManager.Instance.ShowPause();

        TimeManager.Instance.SetTimeScale(0f);

        // 퍼즐 중이라면 Puzzle 블락 유지
        if (GameManager.Instance.PreviousGameState is PuzzleState)
            SaveGuard.Instance?.Block(SaveBlockTag.Puzzle);
    }

    public override void Exit()
    {
        TimeManager.Instance.SetTimeScale(1f);
    }
}
