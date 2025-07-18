using UnityEngine;

public interface IStatusEffectable
{
    void ApplyStatus(ComboAttackData attackData);
    void ApplyStatus(StatusEffectType effect);
    void RemoveStatus(StatusEffectType effect);
}
