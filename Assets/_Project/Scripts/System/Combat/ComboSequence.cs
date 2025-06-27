using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ComboSequence", menuName = "Combat/ComboSequence")]
public class ComboSequence : ScriptableObject
{
    public string comboName;
    public Sprite icon;
    public List<ComboAttackData> attackSequence = new List<ComboAttackData>();
}
