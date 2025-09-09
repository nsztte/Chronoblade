using System;
using UnityEngine;

[RequireComponent(typeof(BossController))]
public class BossControllerSaveProxy : SaveableBehaviour
{
    [Serializable]
    private class Data
    {
        public int phase;       // BossPhase enum을 int로 저장
        public int hpPercent;   // 0~100
    }

    private BossController boss;

    private void Awake()
    {
        boss = GetComponent<BossController>();
    }

    public override string CaptureStateJson()
    {
        if (!boss) return null;

        var d = new Data
        {
            phase = (int)boss.PhaseManager.CurrentPhase,
            hpPercent = Mathf.Clamp(boss.CurrentHpPercentInt, 0, 100)
        };

        return JsonUtility.ToJson(d);
    }

    public override void RestoreStateJson(string json)
    {
        if (!boss || string.IsNullOrEmpty(json)) return;

        var d = JsonUtility.FromJson<Data>(json);

        boss.SetHPWithPercent(Mathf.Clamp(d.hpPercent, 0, 100));
        boss.PhaseManager.SetPhaseFromSave((BossPhase)Mathf.Clamp(d.phase, 0, (int)BossPhase.Ending));
    }
}
