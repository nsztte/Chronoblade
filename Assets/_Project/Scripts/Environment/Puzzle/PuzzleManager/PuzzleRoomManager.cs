using UnityEngine;

public abstract class PuzzleRoomManager : MonoBehaviour
{    
    [SerializeField] protected int roomId;
    [SerializeField] protected GameObject clearedPortal;
    [SerializeField] protected GameObject clearedReward;
    protected bool isCleared = false;
    public bool IsCleared => isCleared;
    protected bool isActivated = false;
    public bool IsActivated => isActivated;

    protected abstract void CheckPuzzle();
    protected void OnPuzzleSolved()
    {
        if (clearedPortal) clearedPortal.SetActive(true);
        if (clearedReward) clearedReward.SetActive(true);
        
        PuzzleProgressManager.Instance.MarkCleared(roomId);
    }

    public void ChangeState(bool isActive)
    {
        isActivated = isActive;
    }
}
