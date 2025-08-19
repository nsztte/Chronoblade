using UnityEngine;

public class PuzzleRoom6Manager : PuzzleRoomManager
{
    [SerializeField] private Transform endPoint;
    [SerializeField] private float detectRadius = 0.5f;
    [SerializeField] private LayerMask statueLayer;

    private void Update()
    {
        if(isCleared || !isActivated) return;

        CheckPuzzle();
    }

    protected override void CheckPuzzle()
    {
        Collider[] hits = Physics.OverlapSphere(endPoint.position, detectRadius, statueLayer);
        foreach(var hit in hits)
        {
            if (hit.TryGetComponent(out PlayerMazeController controller) && controller.IsPossessed)
            {
                isCleared = true;
                OnPuzzleSolved();
                break;
            }
        }
    }

    protected override void OnPuzzleSolved()
    {
        Debug.Log("퍼즐 성공");
        if (clearedPortal) clearedPortal.SetActive(true);
        if (clearedReward) clearedReward.SetActive(true);
    }
}
