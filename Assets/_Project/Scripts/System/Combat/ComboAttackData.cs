using UnityEngine;

[CreateAssetMenu(fileName = "ComboAttackData", menuName = "Combat/ComboAttack")]
public class ComboAttackData : ScriptableObject
{
    public string comboName;
    public AnimationClip animationClip;
    public float damage;
    public float knockbackPower;
    
    [Header("타이밍")]
    public bool useTimingJudgement = true;

    [Header("연계")]
    public float inputWindow;
    public float comboTransitionDelay;

    [Header("연출")]
    public AudioClip soundEffect;
    public GameObject vfxPrefab;

    [Header("콤보 연결")]
    public string comboSequenceId; // 어떤 콤보 시퀀스에 속하는지 식별
    public int comboIndex; // 콤보 시퀀스 내에서의 순서 (0부터 시작)
}
