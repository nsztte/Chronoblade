using UnityEngine;

public enum BossPhase
{
    Phase1,
    Puzzle1,
    Phase2,
    FinalPuzzle,
    Ending
}

public class BossPhaseManager : MonoBehaviour
{
    public BossPhase CurrentPhase { get; private set; } = BossPhase.Phase1;
    private bool isPuzzle1Cleared = false;
    private bool isFinalPuzzleCleared = false;

    public void UpdatePhase(float currentHP, float maxHP)
    {
        if(currentHP <= 0)
        {
            CurrentPhase = BossPhase.Ending;
        }
        else if(currentHP <= maxHP * 0.05f && !isFinalPuzzleCleared)
        {
            CurrentPhase = BossPhase.FinalPuzzle;
        }
        else if(currentHP <= maxHP * 0.5f && !isPuzzle1Cleared)
        {
            CurrentPhase = BossPhase.Puzzle1;
        }
        else
        {
            CurrentPhase = BossPhase.Phase1;
        }
    }

    public void SetPhase(BossPhase phase)
    {
        CurrentPhase = phase;
        if(phase == BossPhase.Phase2)
        {
            isPuzzle1Cleared = true;
        }
        else if(phase == BossPhase.Ending)
        {
            isFinalPuzzleCleared = true;
        }

        Debug.Log($"BossPhaseManager: 페이즈 변경 - {phase}");
    }
}
