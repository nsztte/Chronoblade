using UnityEngine;

[System.Serializable]
public class ComboAttackInfo
{
    public float damage;
    public float knockbackPower;
    public bool isFinalHit;
    public StatusEffectType statusEffect;
    public float statusDuration;
}
