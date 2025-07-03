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

    private void Start()
    {
        // TimingComboManager의 비트 이벤트 구독
        if (TimingComboManager.Instance != null)
        {
            TimingComboManager.Instance.OnBeat += OnBeat;
        }
    }

    private void OnDestroy()
    {
        if (TimingComboManager.Instance != null)
        {
            TimingComboManager.Instance.OnBeat -= OnBeat;
        }
    }

    /// <summary>
    /// 콤보 타이밍 체크 (외부에서 호출)
    /// </summary>
    public void CheckComboTiming()
    {
        // (이전 콤보 시스템에서만 사용, 현재는 필요 없음)
    }

    /// <summary>
    /// 비트마다 호출되는 타이밍 체크
    /// </summary>
    private void OnBeat()
    {
        CheckComboTiming();
    }
}
