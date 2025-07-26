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
    [SerializeField] private BossPhase currentPhase = BossPhase.Phase1;
    public BossPhase CurrentPhase => currentPhase;
    private bool isPuzzle1Cleared = false;
    private bool isFinalPuzzleCleared = false;

    public void UpdatePhase(float currentHP, float maxHP)
    {
        if(currentPhase == BossPhase.Ending) return;

        if(currentHP <= maxHP * 0.05f)
        {
            if(!isFinalPuzzleCleared)
            {
                currentPhase = BossPhase.FinalPuzzle;
            }
            else
            {
                currentPhase = BossPhase.Ending;
            }
        }
        else if(currentHP <= maxHP * 0.5f)
        {
            if(!isPuzzle1Cleared)
            {
                currentPhase = BossPhase.Puzzle1;
            }
            else
            {
                currentPhase = BossPhase.Phase2;
            }
        }
        else
        {
            currentPhase = BossPhase.Phase1;
        }
    }

    public void SetPhase(BossPhase phase)
    {
        currentPhase = phase;
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
