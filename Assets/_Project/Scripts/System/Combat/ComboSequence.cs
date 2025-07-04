using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ComboSequence", menuName = "Combat/ComboSequence")]
public class ComboSequence : ScriptableObject
{
    public string comboName;
    public Sprite icon;
    public List<ComboAttackData> attackSequence = new List<ComboAttackData>();
    public float lastAttackAnimSpeed = 1f; // 마지막 타의 애니메이션 속도 보정값
}
