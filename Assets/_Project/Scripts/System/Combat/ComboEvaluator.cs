using System;
using UnityEngine;
using System.Collections.Generic;

public class ComboEvaluator : MonoBehaviour
{
    #region Singleton
    public static ComboEvaluator Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    // 입력 시퀀스와 일치하는 콤보 후보군 반환
    public List<ComboSequence> GetMatchingCombos(List<AttackType> inputSequence)
    {
        var weapon = WeaponManager.Instance?.CurrentWeapon;
        var result = new List<ComboSequence>();
        if (weapon == null || weapon.weaponData.weaponType != WeaponType.Sword)
            return result;
        foreach (var combo in weapon.weaponData.swordCombos)
        {
            if (combo.attackSequence.Count < inputSequence.Count)
                continue;
            bool match = true;
            for (int i = 0; i < inputSequence.Count; i++)
            {
                if (combo.attackSequence[i].attackType != inputSequence[i])
                {
                    match = false;
                    break;
                }
            }
            if (match)
                result.Add(combo);
        }
        return result;
    }
}
