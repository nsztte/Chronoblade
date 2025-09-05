using UnityEngine;

public class PuzzleRoom4Manager : PuzzleRoomManager
{
    protected override void CheckPuzzle() {}

    public void ChangeIsCleared()
    {
        OnPuzzleSolved();
    }
}
