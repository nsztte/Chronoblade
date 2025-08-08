using UnityEngine;

public abstract class PuzzleRoomManager : MonoBehaviour
{    
    [SerializeField] protected GameObject clearedPortal;
    [SerializeField] protected GameObject clearedReward;
    protected bool isCleared = false;
    public bool IsCleared => isCleared;

    protected abstract void CheckPuzzle();
    protected abstract void OnPuzzleSolved();
}
