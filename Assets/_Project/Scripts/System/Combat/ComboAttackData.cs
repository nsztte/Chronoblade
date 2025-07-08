using UnityEngine;

public enum AttackType { Light, Heavy }
public enum StatusEffectType { None, Stun, Freeze, Pull, AOE }

[CreateAssetMenu(fileName = "ComboAttackData", menuName = "Combat/ComboAttack")]
public class ComboAttackData : ScriptableObject
{
    [Header("기본 정보")]
    public string comboName;
    public float damage;
    // public int staminaCost;
    
    [Header("콤보 패턴")]
    public AttackType attackType; // Light, Heavy
    public int beatPosition; // 몇 번째 비트에 입력해야 하는지 (0부터 시작)
    public bool isFinalHit; // 이 공격이 콤보의 마지막 타격인지 여부
    
    [Header("물리 효과")]
    public float knockbackPower; // 넉백 파워
    
    [Header("연출")]
    public AnimationClip animationClip;
    public AudioClip soundEffect;

    [Header("상태 이상 효과")]
    public StatusEffectType statusEffect;
    public float statusDuration;
}
