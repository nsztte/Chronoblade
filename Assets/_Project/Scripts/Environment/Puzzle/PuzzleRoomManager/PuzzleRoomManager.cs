using UnityEngine;

public abstract class PuzzleRoomManager : MonoBehaviour
{
    protected bool isCleared = false;
    public bool IsCleared => isCleared;

    protected abstract void CheckPuzzle();
    protected abstract void OnPuzzleSolved();
}
