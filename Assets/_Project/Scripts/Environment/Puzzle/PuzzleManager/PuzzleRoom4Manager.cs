using UnityEngine;

public class PuzzleRoom4Manager : PuzzleRoomManager
{
    private void Update()
    {
        if(isCleared)
            CheckPuzzle();
    }

    protected override void CheckPuzzle()
    {
        OnPuzzleSolved();
    }

    public void ChangeIsCleared()
    {
        isCleared = true;
    }
}
