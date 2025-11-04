using UnityEngine;

public enum AbilityKind { Dash, TimeSlow, TimeStop, TimeRewind, TimeFastForward }
public class AbilityUnlockTrigger : MonoBehaviour
{
    [Header("해금할 능력들")]
    [SerializeField] private AbilityKind[] abilities;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        foreach (var a in abilities)
        {
            switch (a)
            {
                case AbilityKind.Dash:
                    PlayerManager.Instance.UnlockDash();
                    break;

                case AbilityKind.TimeSlow:
                    TimeManager.Instance.UnlockTimeSkill(TimeState.Slow);
                    break;
                case AbilityKind.TimeStop:
                    TimeManager.Instance.UnlockTimeSkill(TimeState.Stop);
                    break;
                case AbilityKind.TimeRewind:
                    TimeManager.Instance.UnlockTimeSkill(TimeState.Rewind);
                    break;
                case AbilityKind.TimeFastForward:
                    TimeManager.Instance.UnlockTimeSkill(TimeState.FastForward);
                    break;
            }

            UIManager.Instance.ShowToast($"{a} 스킬 해금");
        }

        // TODO: 플레이어에게 능력이 흡수되는 연출, 기술 설명 UI
        
        Destroy(gameObject, 2f);
    }
}