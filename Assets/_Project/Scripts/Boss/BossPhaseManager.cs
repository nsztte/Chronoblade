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

    // public void SetPhase(BossPhase phase)
    // {
    //     currentPhase = phase;
    //     if(phase == BossPhase.Phase2)
    //     {
    //         isPuzzle1Cleared = true;
    //     }
    //     else if(phase == BossPhase.Ending)
    //     {
    //         isFinalPuzzleCleared = true;
    //     }

    //     Debug.Log($"BossPhaseManager: 페이즈 변경 - {phase}");
    // }

    public void SetPhase(BossPhase phase)
    {
        currentPhase = phase;
        switch (phase)
        {
            case BossPhase.Phase1:
                // 보스전 시작: 수동 저장 전면 차단
                SaveGuard.Instance?.Block(SaveBlockTag.Boss);
                SaveManager.Instance?.AutoSave("Boss Phase1");
                break;
                
            case BossPhase.Phase2:
                isPuzzle1Cleared = true;
                // SaveManager.Instance?.AutoSave("Boss Phase2");
                break;

            case BossPhase.Ending:
                isFinalPuzzleCleared = true;
                SaveManager.Instance?.AutoSave("Boss Ending");
                break;
        }

        Debug.Log($"BossPhaseManager: 페이즈 변경 - {phase}");
    }

    // 엔딩 연출 이후 호출
    public void FinishBossSequence()
    {
        SaveGuard.Instance?.Unblock(SaveBlockTag.Boss);
    }

    // 오토세이브/세이브가드 등 부수효과 없음(복원 전용)
    public void SetPhaseFromSave(BossPhase phase)
    {
        currentPhase = phase;

        isPuzzle1Cleared = (phase == BossPhase.Phase2 || phase == BossPhase.FinalPuzzle || phase == BossPhase.Ending);
        isFinalPuzzleCleared = (phase == BossPhase.Ending);
    }
}
