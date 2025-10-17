using System;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    #region 싱글톤 및 초기화
    public static VFXManager Instance { get; private set; }

    public void Initialize()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning($"{GetType().Name} 인스턴스 감지됨, 초기화 스킵");
            return;
        }
        Instance = this;

        Init();
    }
    #endregion

    [Serializable]
    public class VfxEntry
    {
        public string key;
        public VFXPool pool;
        public float autoRelease = 1.0f;
    }

    [SerializeField] private List<VfxEntry> entries = new();
    private readonly Dictionary<string, VfxEntry> map = new();

    private void Init()
    {
        foreach (var e in entries)
        {
            if (e == null || string.IsNullOrEmpty(e.key) || e.pool == null) continue;
            map[e.key] = e;
            e.pool.InitPool();
        }
    }

    public Transform Spawn(string key, Vector3 pos, Quaternion rot, Transform parent = null, float scale = 1f)
    {
        if (!map.TryGetValue(key, out var e))
        {
            Debug.LogWarning($"[VFX] 없는 키: {key}");
            return null;
        }

        var t = e.pool.Get();
        if (t == null) return null;

        if (parent != null) t.SetParent(parent, true);
        t.SetPositionAndRotation(pos, rot);
        if (!Mathf.Approximately(scale, 1f)) t.localScale = Vector3.one * scale;

        // 자동 반환
        e.pool.ReleaseAfter(t, Mathf.Max(0.05f, e.autoRelease));
        return t;
    }
}
