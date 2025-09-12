using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public enum SaveIntent { Quick, Auto, Manual }

public class SaveManager : MonoBehaviour
{
    #region 싱글톤 및 초기화
    public static SaveManager Instance { get; private set; }

    public void Initialize()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning($"{GetType().Name} 인스턴스 감지됨, 초기화 스킵");
            return;
        }
        Instance = this;

        InputManager.Instance.OnQuickSave += QuickSave;
        OnAfterLoad += StartSession;
    }
    #endregion

    [Serializable]
    public class SaveMeta
    {
        public string scene;            // 저장 씬
        public string savedAt;          // 저장 시점
        public long playtimeSeconds;    // 누적 플레이타임(초)
        public string saveType;         // Quick, Auto, Manual
        public string thumbnail;        // 썸네일

        public static string FormatPlaytime(long seconds)
        {
            var ts = TimeSpan.FromSeconds(seconds);
            return $"{(int)ts.TotalHours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}";
        }
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
        public int version = 1;
        public SaveMeta meta = new();
        public List<SaveEntry> entries = new();
    }

    [Serializable]
    private class MetaWrapper
    {
        public int version;
        public SaveMeta meta;
    }

    // 게터
    private string GetPath(int slot) =>
        Path.Combine(Application.persistentDataPath, $"slot_{slot}.json");

    private string GetBackupPath(string path) => path + ".bak";

    // 이벤트 훅
    public event Action OnBeforeSave;   // 잠깐 멈춤
    public event Action OnSaved;        // 재개
    public event Action OnAfterLoad;    // 공용 로드 이벤트

    // 저장 토스트 메세지
    private static readonly Dictionary<SaveBlockTag, string> BlockMessages = new()
    {
        { SaveBlockTag.Boss,     "보스전 진행 중에는 저장할 수 없습니다" },
        { SaveBlockTag.Puzzle,   "퍼즐 진행 중에는 저장할 수 없습니다" },
        { SaveBlockTag.Cutscene, "연출 중에는 저장할 수 없습니다" },
        { SaveBlockTag.Default,  "지금은 저장할 수 없습니다" }
    };

    [SerializeField] private ThumbnailCapture thumbnailCapture;     // 썸네일 캡쳐용 카메라

    private float sessionStartTime;     // 게임 켜진 시간 (초)
    private long prevPlaytimeAtSessionStart; // 세션 시작 시점의 누적 플레이타임(초)

    // 저장 관련 필드
    private bool isSaving = false;
    private int autoSlots = 3;
    private const string Key = "Save_AutoIndex";

    private const int CURRENT_VERSION = 1;  // 현재 저장 버전

    private const int FirstSlotIndex = 1;
    private const int LastSlotIndex = 9;

    void OnEnable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnQuickSave += QuickSave;
    }

    void OnDisable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnQuickSave -= QuickSave;
    }   

    // 새 게임 시작 버튼 누를때 호출 (로드는 이미 Initialize에서 이벤트 등록하여 처리 중)
    // 새 게임 시작 시 prevPlaytimeAtSessionStart = 0으로 초기화 필수
    public void StartSession()
    {
        sessionStartTime = Time.realtimeSinceStartup;
    }

    // 퀵 세이브
    public void QuickSave()
    {
        DefaultSave(FirstSlotIndex, SaveIntent.Quick);
        Debug.Log("[SaveManager] 퀵세이브 → 슬롯 1");
    }

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
    public void DefaultLoad(int slot) => StartCoroutine(LoadRoutineWithUX(slot));

    public List<(int slotIndex, SaveMeta meta)> GetAllMeta()
    {
        var list = new List<(int, SaveMeta)>();

        for (int i = FirstSlotIndex; i <= LastSlotIndex; i++)
        {
            string path = GetPath(i);
            if (!File.Exists(path)) continue;

            try
            {
                string json = File.ReadAllText(path);
                var wrapper = JsonUtility.FromJson<MetaWrapper>(json);
                if (wrapper.meta != null)
                    list.Add((i, wrapper.meta));
            }
            catch { continue; }
        }

        return list;
    }

    private IEnumerator BackgroundSaveRoutine(int slot, SaveIntent intent)
    {
        if(isSaving) yield break;

        if (intent == SaveIntent.Manual &&
                SaveGuard.Instance != null && !SaveGuard.Instance.CanSave)
        {
            var tag = SaveGuard.Instance.GetCurrentMainBlock();
            string msg = BlockMessages.TryGetValue(tag, out var result) ? result : "저장할 수 없습니다";
            UIManager.Instance?.ShowToast(msg);
            yield break;
        }

        isSaving = true;

        try
        {
            SaveGuard.Instance?.Block();

            InputManager.Instance?.SetInputEnabled(false);  // 입력 잠금
            OnBeforeSave?.Invoke();
            yield return new WaitForEndOfFrame();   // 프레임 경계에서 캡쳐

            // --- 썸네일 캡처 시도 ---
            string thumbnailRel = null;
            if (thumbnailCapture != null)
            {
                string previewFullPath = GetPreviewPath(slot);
                bool captured = thumbnailCapture.CaptureToFile(previewFullPath);
                if (captured)
                    thumbnailRel = ToRelativePreviewPath(previewFullPath);
            }
            // ------------------------

            bool success = true;

            try
            {
                Save(slot, intent, thumbnailRel);
            }
            catch (Exception e)
            {
                success = false;
                Debug.LogException(e);
            }

            yield return null;

            InputManager.Instance?.SetInputEnabled(true);
            if(success)
            {
                OnSaved?.Invoke();      // 슬롯 업데이트
                UIManager.Instance?.ShowToast("저장 완료");
                Debug.Log(Application.persistentDataPath);
            }
            else
                UIManager.Instance?.ShowToast("저장 실패");
        }
        finally
        {
            SaveGuard.Instance?.Unblock();
            isSaving = false;
            InputManager.Instance?.SetInputEnabled(true);
        }
    }

    private IEnumerator LoadRoutineWithUX(int slot)
    {
        InputManager.Instance?.SetInputEnabled(false);
        yield return UIManager.Instance?.FadeUI.Show(0.25f);

        Load(slot);
        yield return null;

        yield return UIManager.Instance?.FadeUI.Hide(0.25f);
        InputManager.Instance?.SetInputEnabled(true);
        UIManager.Instance?.ShowToast("로드 완료");
    }

    // 자동저장 슬롯 인덱스
    private int NextAutoSlot()
    {
        int i = PlayerPrefs.GetInt(Key, 0);
        i = (i + 1) % autoSlots;
        PlayerPrefs.SetInt(Key, i);
        PlayerPrefs.Save();
        
        return i + FirstSlotIndex + 1;   // 퍼즐 슬롯은 2부터 시작
    }
    
    // 저장
    private void Save(int slot, SaveIntent intent = SaveIntent.Manual, string thumbnailRelPath = null)
    {
        var saveFile = new SaveFile();
        saveFile.version = CURRENT_VERSION;
        saveFile.meta.scene = SceneManager.GetActiveScene().name;
        saveFile.meta.savedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        // 누적 플레이타임 계산
        long sessionElapsed = (long)(Time.realtimeSinceStartup - sessionStartTime);
        saveFile.meta.playtimeSeconds = prevPlaytimeAtSessionStart + sessionElapsed;

        // 세이브 타입
        saveFile.meta.saveType = intent.ToString();

        // 썸네일 메타 기록
        saveFile.meta.thumbnail = thumbnailRelPath;

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
    }

    // 로드
    private void Load(int slot)
    {
        var path = GetPath(slot);
        var bak  = GetBackupPath(path);

        // 1) 메인 파일이 없으면 .bak만 시도
        if (!File.Exists(path))
        {
            if (File.Exists(bak))
            {
                var bakJson = File.ReadAllText(bak);
                TryLoadFromJson(bakJson, isBackup: true);
                return;
            }
            Debug.LogWarning($"[SaveManager] 저장 파일 없음: {path}");
            return;
        }

        // 2) 메인 읽고 파싱 시도 → 실패하면 .bak 한 번만 재시도
        var jsonText = File.ReadAllText(path);
        try
        {
            TryLoadFromJson(jsonText, isBackup: false);
            Debug.Log($"[SaveManager] {slot} 슬롯 로드 성공");
        }
        catch (Exception e)
        {
            if (File.Exists(bak))
            {
                Debug.LogWarning($"[SaveManager] 메인 파싱 실패 → 백업 시도: {path}\n{e}");
                var bakJson = File.ReadAllText(bak);
                TryLoadFromJson(bakJson, isBackup: true);
                return;
            }
            Debug.LogError($"[SaveManager] 세이브 파싱 실패: {path}\n{e}");
            UIManager.Instance?.ShowToast("세이브 파일이 손상되었습니다");
        }
    }

    // 제이슨 파일 읽고 로드
    private void TryLoadFromJson(string jsonText, bool isBackup)
    {
        var saveFile = JsonUtility.FromJson<SaveFile>(jsonText);

        // 버전 다르면 마이그레이션 (이미 만든 함수 사용)
        if (saveFile.version != CURRENT_VERSION)
        {
            if (!TryMigrate(ref saveFile, saveFile.version, CURRENT_VERSION))
            {
                UIManager.Instance?.ShowToast("세이브 파일 버전 호환 실패");
                throw new Exception("MigrationFailed");
            }
        }

        if (isBackup)
            UIManager.Instance?.ShowToast("백업 세이브로 복구했습니다");

        prevPlaytimeAtSessionStart = saveFile.meta.playtimeSeconds;

        if (saveFile.meta.scene != SceneManager.GetActiveScene().name)
            StartCoroutine(LoadSceneAndRestore(saveFile));
        else
            RestoreState(saveFile);
    }

    // 씬 로드 후 복구
    private IEnumerator LoadSceneAndRestore(SaveFile file)
    {
        var op = SceneManager.LoadSceneAsync(file.meta.scene);
        while (!op.isDone) yield return null;
        yield return null; // 초기화 1프레임 대기
        RestoreState(file);
    }

    // 같은 씬에서 복구
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
    }

    // 저장 중 파일 깨짐 대비
    private void WriteAtomic(string path, string content)
    {
        var tmp = path + ".tmp";
        File.WriteAllText(tmp, content);
        Debug.Log($"[SaveManager] 임시파일 생성 완료: {tmp}");

        var bak = GetBackupPath(path);

        // if (File.Exists(path)) File.Replace(tmp, path, bak);
        // else File.Move(tmp, path);

        try
        {
            if (File.Exists(path))
            {
                File.Replace(tmp, path, bak);
                Debug.Log($"[SaveManager] 기존 파일 덮어쓰기 성공: {path} → .bak: {bak}");
            }
            else
            {
                File.Move(tmp, path);
                Debug.Log($"[SaveManager] 신규 저장 파일 생성 완료: {path}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[SaveManager] WriteAtomic 실패: {e.Message}\n{e.StackTrace}");
        }
    }

    /// <summary>
    /// 구조가 바뀌면 구버전 세이브 로드가 실패할 수 있으므로, 버전 업그레이드시 마이그레이션으로 구출 가능
    /// </summary>
    private bool TryMigrate(ref SaveFile file, int fromVersion, int toVersion)
    {
        // TODO: 앞으로 스키마가 바뀔 때마다 단계적으로 올려주기
        if (file.meta == null) file.meta = new SaveMeta();
        int v = Mathf.Max(1, fromVersion);

        while (v < toVersion)
        {
            switch (v)
            {
                case 1:
                    v = 2;
                    break;
                default:
                    // 알 수 없는/스킵 불가 단계 → 실패 처리
                    Debug.LogWarning($"[SaveManager] 알 수 없는 마이그레이션 단계: v{v} → v{v+1}");
                    return false;
            }
        }

        file.version = toVersion;
        return true;
    }

    // 썸네일 파일 경로 유틸
    private string GetPreviewPath(int slot)
    {
        string dir = Path.Combine(Application.persistentDataPath, "Previews");
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        return Path.Combine(dir, $"slot_{slot}.png");
    }

    // 저장 파일 안에는 상대경로만 기록
    private string ToRelativePreviewPath(string fullPath)
    {
        return $"Previews/{Path.GetFileName(fullPath)}";
    }
}
