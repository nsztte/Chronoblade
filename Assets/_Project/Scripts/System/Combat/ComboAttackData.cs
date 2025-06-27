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
    public ComboAttackData nextCombo;

    [Header("연출")]
    public AudioClip soundEffect;
    public GameObject vfxPrefab;

}
