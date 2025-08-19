using UnityEngine;

public abstract class PuzzleRoomManager : MonoBehaviour
{    
    [SerializeField] protected GameObject clearedPortal;
    [SerializeField] protected GameObject clearedReward;
    protected bool isCleared = false;
    public bool IsCleared => isCleared;
    protected bool isActivated = false;
    public bool IsActivated => isActivated;

    protected abstract void CheckPuzzle();
    protected abstract void OnPuzzleSolved();

    public void ChangeState(bool isActive)
    {
        isActivated = isActive;
    }
}
