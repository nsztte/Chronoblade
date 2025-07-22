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

    public void UpdatePhase(float currentHP, float maxHP)
    {
        if(currentHP <= 0)
        {
            CurrentPhase = BossPhase.Ending;
        }
        else if(currentHP <= maxHP * 0.05f)
        {
            CurrentPhase = BossPhase.FinalPuzzle;
        }
        else if(currentHP <= maxHP * 0.5f)
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
        Debug.Log($"BossPhaseManager: 페이즈 변경 - {phase}");
    }
}
