using UnityEngine;

public interface IStatusEffectable
{
    void ApplyStatus(StatusEffectType type, float duration);
}
