using System.Collections;
using UnityEngine;

public class BossFinalComboController : MonoBehaviour, IStatusEffectable
{
    [SerializeField] private float impactCooldown = 0.25f;      // 연속 피격 방지
    [SerializeField] private float freezeDuration = 0.5f;
    [SerializeField] private float slowDuration  = 0.3f;

    private float lastImpactTime = -999f;
    private bool phaseImmune = false;

    private BossController bossController;
    private BossStateMachine sm;

    public void PhaseShiftStart() => phaseImmune = true;
    public void PhaseShiftEnd() => phaseImmune = false;

    private void Awake()
    {
        bossController = GetComponent<BossController>();
        sm = bossController.BossSM;
    }

    // 슬로우 / 프리즈 효과 대신 스태거
    public void ApplyStatus(ComboAttackData attackData)
    {
        if (phaseImmune) return;
        if (!attackData.isFinalHit) return;                      // 최종타만 적용
        if (Time.time - lastImpactTime < impactCooldown) return; // 스턴락 방지
        lastImpactTime = Time.time;

        // TODO: 카메라쉐이크, sfx, vfx 연결

        switch (attackData.statusEffectType)
        {
            case StatusEffectType.Freeze:
                sm.ChangeState(new StaggerCheckState(bossController, sm, freezeDuration));
            break;
            case StatusEffectType.Slow:
                sm.ChangeState(new StaggerCheckState(bossController, sm, slowDuration));
            break;
        }
    }

    public void ApplyStatus(StatusEffectType effect, float duration = 0){}

    public void RemoveStatus(StatusEffectType effect){}
}
