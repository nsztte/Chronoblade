using UnityEngine;

public interface IStatusEffectable
{
    void ApplyStatus(ComboAttackData attackData);
    void ApplyStatus(StatusEffectType effect, float duration = 0f);
    void RemoveStatus(StatusEffectType effect);
}
