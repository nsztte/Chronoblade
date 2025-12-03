using UnityEngine;

public class CombatTutorialManager : MonoBehaviour
{
    public static CombatTutorialManager Instance { get; private set; }

    private enum Step
    {
        None,
        Attack,
        Block,
        Parry,
        TimingCombo,
        Done
    }

    
}
