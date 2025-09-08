using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public enum SaveIntent { Manual, Auto }

public class SaveManager : MonoBehaviour
{
    #region Singleton
    public static SaveManager Instance { get; private set; }

    public void Initialize()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning($"{GetType().Name} 인스턴스 감지됨, 초기화 스킵");
            return;
        }
        Instance = this;
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

    // 이벤트 훅
    public event Action OnBeforeSave;   // 잠깐 멈춤
    public event Action OnSaved;        // 재개
    public event Action OnAfterLoad;    // 공용 로드 이벤트

    private bool isSaving = false;

    private int autoSlots = 5;
    private const string Key = "Save_AutoIndex";

    // 자동저장
    public void AutoSave(string reason = null)
    {
        // 자동저장 슬롯 순환
        int slot = NextAutoSlot();
        DefaultSave(slot, SaveIntent.Auto);

        // 자동저장 사유 디버그
        if (!string.IsNullOrEmpty(reason))
            Debug.Log($"[SaveManager] 자동저장: {reason} → 슬롯 {slot}");
    }

    public void DefaultSave(int slot, SaveIntent intent = SaveIntent.Manual) => StartCoroutine(BackgroundSaveRoutine(slot, intent));

    private IEnumerator BackgroundSaveRoutine(int slot, SaveIntent intent)
    {
        if(isSaving) yield break;

        if (intent == SaveIntent.Manual &&
                SaveGuard.Instance != null && !SaveGuard.Instance.CanSave)
        {
            UIManager.Instance?.ShowToast("퍼즐 진행 중 저장 불가");
            yield break;
        }

        isSaving = true;

        try
        {
            SaveGuard.Instance?.Block();
            OnBeforeSave?.Invoke();
            yield return new WaitForEndOfFrame();   // 프레임 경계에서 캡쳐

            bool success = true;

            try
            {
                Save(slot, intent);
            }
            catch (Exception e)
            {
                success = false;
                Debug.LogException(e);
            }

            yield return null;

            if(success)
            {
                OnSaved?.Invoke();
                UIManager.Instance?.ShowToast("저장 완료");
            }
            else
                UIManager.Instance?.ShowToast("저장 실패");
        }
        finally
        {
            SaveGuard.Instance?.Unblock();
            isSaving = false;   
        }
    }

    // 자동저장 슬롯 인덱스
    private int NextAutoSlot()
    {
        int i = PlayerPrefs.GetInt(Key, 0);
        i = (i + 1) % autoSlots;
        PlayerPrefs.SetInt(Key, i);
        PlayerPrefs.Save();
        
        return i + 1;   // 퍼즐 슬롯은 1부터 시작
    }
    
    // 저장
    private void Save(int slot, SaveIntent intent = SaveIntent.Manual)
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

        var jsonText = JsonUtility.ToJson(saveFile, false);
        WriteAtomic(GetPath(slot), jsonText);
        // Debug.Log($"[SaveManager] 저장 완료: {GetPath(slot)}");
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
        yield return null; // 초기화 1프레임 대기
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

        OnAfterLoad?.Invoke();

        Debug.Log("[SaveManager] 로드 완료");
    }

    // 저장 중 파일 깨짐 대비
    private void WriteAtomic(string path, string content)
    {
        var tmp = path + ".tmp";
        File.WriteAllText(tmp, content);
        if (File.Exists(path)) File.Replace(tmp, path, null);
        else File.Move(tmp, path);
    }
}
