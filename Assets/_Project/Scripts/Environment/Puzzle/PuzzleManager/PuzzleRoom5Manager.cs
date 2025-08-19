using UnityEngine;

public class PuzzleRoom5Manager : PuzzleRoomManager
{
    [SerializeField] private GameObject Pedestals;
    private PuzzlePlate[] puzzlePlates;

    private void Awake()
    {
        puzzlePlates = Pedestals.GetComponentsInChildren<PuzzlePlate>();

        if(clearedPortal)
            clearedPortal.SetActive(false);

        if(clearedReward)
            clearedReward.SetActive(false);
    }

    private void Update()
    {
        CheckPuzzle();
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
