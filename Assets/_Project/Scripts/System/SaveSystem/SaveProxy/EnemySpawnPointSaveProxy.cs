using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemySpawnPoint), typeof(SaveId))]
public class EnemySpawnPointSaveProxy : SaveableBehaviour
{
    [Serializable]
    private struct EnemySnap
    {
        public Vector3 pos;
        public float yaw;
        public float hpRatio; // 0~1
    }

    [Serializable]
    private class Data
    {
        public bool valid;            // allowRespawn==false 일 때 true
        public List<EnemySnap> list;  // 살아있는 적 스냅샷
    }

    private EnemySpawnPoint sp;

    private void Awake()
    {
        sp = GetComponent<EnemySpawnPoint>();
    }

    public override string CaptureStateJson()
    {
        if (sp == null || sp.AllowRespawn) return null;

        var snaps = new List<EnemySnap>();
        var active = sp.ActiveEnemies;

        if (active != null)
        {
            foreach (var e in active)
            {
                if (e == null || !e.gameObject.activeInHierarchy) continue;

                snaps.Add(new EnemySnap
                {
                    pos = e.transform.position,
                    yaw = e.transform.rotation.eulerAngles.y,
                    hpRatio = e.GetHpRatio()
                });
            }
        }

        var d = new Data { valid = true, list = snaps };
        return JsonUtility.ToJson(d);
    }

    public override void RestoreStateJson(string json)
    {
        if (sp == null || string.IsNullOrEmpty(json)) return;

        sp.SpawnOnStart = false; // 초기 자동 스폰 차단
        var d = JsonUtility.FromJson<Data>(json);
        if (d == null || !d.valid) return;

        sp.DespawnAllEnemies();

        foreach (var s in d.list)
        {
            var enemy = sp.SpawnOneAt(s.pos, s.yaw);
            if (enemy == null) continue;

            enemy.SetHpRatio(Mathf.Clamp01(s.hpRatio));
        }
    }
}
