using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    #region Singleton
    public static SaveManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    [Serializable]
    public class SaveMeta
    {
        public string scene;
        public string savedAt;
    }

    [Serializable]
    private class SaveEntry
    {
        public string id;
        public string json;
    }

    [Serializable]
    private class SaveFile
    {
        public SaveMeta meta = new();
        public List<SaveEntry> entries = new();
    }

    private string GetPath(int slot) =>
        Path.Combine(Application.persistentDataPath, $"slot_{slot}.json");

    
    // 저장
    public void Save(int slot)
    {
        var saveFile = new SaveFile();
        saveFile.meta.scene = SceneManager.GetActiveScene().name;
        saveFile.meta.savedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        var saveables = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<ISaveable>();
        
        var ids = new HashSet<string>();

        foreach (var s in saveables)
        {
            var json = s.CaptureStateJson();
            if (!string.IsNullOrEmpty(json))
            {
                if(!ids.Add(s.SaveId))
                {
                    Debug.LogWarning($"[SaveManager] 중복 SaveId 스킵: {s.SaveId} ({s})");
                    continue;
                }

                saveFile.entries.Add(new SaveEntry { id = s.SaveId, json = json });
            }   
        }

        var jsonText = JsonUtility.ToJson(saveFile, true);
        File.WriteAllText(GetPath(slot), jsonText);
        Debug.Log($"[SaveManager] 저장 완료: {GetPath(slot)}");
    }

    // 로드
    public void Load(int slot)
    {
        var path = GetPath(slot);
        if (!File.Exists(path))
        {
            Debug.LogWarning($"[SaveManager] 저장 파일 없음: {path}");
            return;
        }

        var jsonText = File.ReadAllText(path);
        var saveFile = JsonUtility.FromJson<SaveFile>(jsonText);

        if (saveFile.meta.scene != SceneManager.GetActiveScene().name)
        {
            StartCoroutine(LoadSceneAndRestore(saveFile));
        }
        else
        {
            RestoreState(saveFile);
        }
    }

    private IEnumerator LoadSceneAndRestore(SaveFile file)
    {
        var op = SceneManager.LoadSceneAsync(file.meta.scene);
        while (!op.isDone) yield return null;
        RestoreState(file);
    }

    private void RestoreState(SaveFile file)
    {
        var map = file.entries.ToDictionary(e => e.id, e => e.json);
        var saveables = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<ISaveable>();
        foreach (var s in saveables)
        {
            if (map.TryGetValue(s.SaveId, out var json))
                s.RestoreStateJson(json);
        }

        Debug.Log("[SaveManager] 로드 완료");
    }
}
