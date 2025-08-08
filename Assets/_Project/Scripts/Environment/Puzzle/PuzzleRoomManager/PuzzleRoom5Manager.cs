using UnityEngine;

public class PuzzleRoom5Manager : PuzzleRoomManager
{
    [SerializeField] private GameObject Pedestals;
    [SerializeField] private GameObject clearedPortal;
    [SerializeField] private GameObject clearedReward;
    private PuzzlePlate[] puzzlePlates;

    private void Awake()
    {
        puzzlePlates = Pedestals.GetComponentsInChildren<PuzzlePlate>();
    }
    
    protected override void CheckPuzzle()
    {
        if(IsCleared) return;

        foreach(var plate in puzzlePlates)
        {
            if(plate.IsCorrect())
                return;
        }

        isCleared = true;

        OnPuzzleSolved();
    }

    protected override void OnPuzzleSolved()
    {
        if(clearedPortal)
            clearedPortal.SetActive(true);

        if(clearedReward)
            clearedReward.SetActive(true);
    }
}
