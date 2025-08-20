using UnityEngine;

public class PuzzleRoom3Manager : PuzzleRoomManager
{
    [SerializeField] private Transform clearPosition;
    [SerializeField] private float detectRadius = 0.5f;
    [SerializeField] private LayerMask playerLayer;

    private void Update()
    {
        if(isCleared || isActivated) return;

        CheckPuzzle();
    }

    protected override void CheckPuzzle()
    {
        Collider[] hits = Physics.OverlapSphere(clearPosition.position, detectRadius, playerLayer);
        foreach(var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                isCleared = true;
                OnPuzzleSolved();
                break;
            }
        }
    }
}
