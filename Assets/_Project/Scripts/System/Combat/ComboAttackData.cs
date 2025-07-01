using UnityEngine;

public enum AttackType
{
    Light,  // 약공격
    Heavy,  // 강공격
    Rest    // 휴식
}

[CreateAssetMenu(fileName = "ComboAttackData", menuName = "Combat/ComboAttack")]
public class ComboAttackData : ScriptableObject
{
    [Header("기본 정보")]
    public string comboName;
    public float damage;
    
    [Header("콤보 패턴")]
    public AttackType attackType; // Light, Heavy
    public int beatPosition; // 몇 번째 비트에 입력해야 하는지 (0부터 시작)
    
    [Header("물리 효과")]
    public float knockbackPower; // 넉백 파워
    
    [Header("연출")]
    public AnimationClip animationClip;
    public AudioClip soundEffect;
}
