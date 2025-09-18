using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AudioManager : MonoBehaviour
{
    #region 싱글톤 및 초기화
    public static AudioManager Instance { get; private set; }

    public void Initialize()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning($"{GetType().Name} 인스턴스 감지됨, 초기화 스킵");
            return;
        }
        Instance = this;

        sfxPool.InitPool();
    }
    #endregion

    [Header("오디오소스")]
    [SerializeField] private AudioSource uiSfxSource;      // 2D PlayOneShot 전용 (Output => SFX 그룹)
    [SerializeField] private AudioSource bgmSourceA;       // BGM A (크로스페이드용)
    [SerializeField] private AudioSource bgmSourceB;       // BGM B (크로스페이드용)

    [Header("SFX Pool")]
    [SerializeField] private SfxPool sfxPool;

    [Header("오디오믹서")]
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private string masterParam = "MasterVol";
    [SerializeField] private string bgmParam = "BGMVol";
    [SerializeField] private string sfxParam = "SFXVol";

    // PlayerPrefs 키
    private const string PREF_MASTER = "opt_master";
    private const string PREF_BGM = "opt_bgm";
    private const string PREF_SFX = "opt_sfx";

    // 내부 상태
    private AudioSource currentBgmSource;       // 현재 소스(A 또는 B)
    // private AudioSource fadingBgmSource;        // 이전 소스(페이드 아웃)
    private readonly Dictionary<string, AudioClip> clipCache = new(); // name -> clip
    private readonly Dictionary<string, AsyncOperationHandle<AudioClip>> clipHandles = new();
    private readonly Dictionary<string, AsyncOperationHandle<IList<AudioClip>>> groupHandles = new();
    private readonly Dictionary<string, AsyncOperationHandle<AudioClip>> bgmHandles = new();

    // 최근 set values (linear 0~1)
    private float masterLinear = 1f;
    private float bgmLinear = 1f;
    private float sfxLinear = 1f;

    private void Awake()
    {
        // 기본 currentBgmSource 설정
        currentBgmSource = bgmSourceA != null ? bgmSourceA : bgmSourceB;
        // fadingBgmSource = null;
    }

    private void Start()
    {
        LoadVolumesFromPrefs();
        ApplyMixerVolumes();
    }

    #region 믹서 볼륨 API

    /// <summary>선형(0~1) 값 -> dB 변환 후 Mixer에 설정</summary>
    private void SetMixerVolume(string paramName, float linear)
    {
        float db = (linear <= 0f) ? -80f : Mathf.Log10(Mathf.Clamp01(linear)) * 20f;
        if (mixer != null) mixer.SetFloat(paramName, db);
    }

    public void SetMaster(float linear)
    {
        masterLinear = Mathf.Clamp01(linear);
        SetMixerVolume(masterParam, masterLinear);
        PlayerPrefs.SetFloat(PREF_MASTER, masterLinear);
    }

    public void SetBGM(float linear)
    {
        bgmLinear = Mathf.Clamp01(linear);
        SetMixerVolume(bgmParam, bgmLinear);
        PlayerPrefs.SetFloat(PREF_BGM, bgmLinear);
    }

    public void SetSFX(float linear)
    {
        sfxLinear = Mathf.Clamp01(linear);
        SetMixerVolume(sfxParam, sfxLinear);
        PlayerPrefs.SetFloat(PREF_SFX, sfxLinear);
    }

    public float GetMaster() => masterLinear;
    public float GetBGM() => bgmLinear;
    public float GetSFX() => sfxLinear;

    private void LoadVolumesFromPrefs()
    {
        masterLinear = PlayerPrefs.GetFloat(PREF_MASTER, 1f);
        bgmLinear = PlayerPrefs.GetFloat(PREF_BGM, 1f);
        sfxLinear = PlayerPrefs.GetFloat(PREF_SFX, 1f);
    }

    private void ApplyMixerVolumes()
    {
        SetMixerVolume(masterParam, masterLinear);
        SetMixerVolume(bgmParam, bgmLinear);
        SetMixerVolume(sfxParam, sfxLinear);
    }
    #endregion

    #region UI SFX 프리로드, 플레이

    /// <summary>
    /// 라벨(label)에 해당하는 AudioClip들을 전부 로드해서 clipCache에 저장.
    /// 로드 핸들은 groupHandles[label]에 보관 -> ReleaseGroup으로 일괄 해제 가능.
    /// </summary>
    public void PreloadGroup(string label)
    {
        if (string.IsNullOrEmpty(label)) return;
        if (groupHandles.ContainsKey(label)) return; // 이미 로드 중/로딩됨

        // LoadAssetsAsync는 IList<AudioClip> 형태의 handle을 반환.
        var handle = Addressables.LoadAssetsAsync<AudioClip>(label, (clip) =>
        {
            if (clip == null) return;
            if (!clipCache.ContainsKey(clip.name))
                clipCache[clip.name] = clip;
        });

        groupHandles[label] = handle;
    }

    /// <summary>
    /// label에 해당하는 로드 핸들 모두 Release 및 캐시에서 해당 클립 제거
    /// </summary>
    public void ReleaseGroup(string label)
    {
        if (string.IsNullOrEmpty(label)) return;
        if (!groupHandles.TryGetValue(label, out var handle)) return;

        // 핸들이 성공적으로 로드된 경우 결과 목록에서 이름을 뽑아 캐시에서 제거
        if (handle.IsValid() && handle.Status == AsyncOperationStatus.Succeeded)
        {
            var list = handle.Result;
            if (list != null)
            {
                foreach (var clip in list)
                {
                    if (clip != null && clipCache.ContainsKey(clip.name))
                        clipCache.Remove(clip.name);
                }
            }
        }

        Addressables.Release(handle);
        groupHandles.Remove(label);
    }

    /// <summary>
    /// UI용으로 프리로드된 clip을 PlayOneShot
    /// </summary>
    public void PlayUIFromCache(string key)
    {
        if (uiSfxSource == null) return;
        if (string.IsNullOrEmpty(key)) return;

        if (clipCache.TryGetValue(key, out var clip) && clip != null)
        {
            uiSfxSource.PlayOneShot(clip);
            return;
        }

        // 이미 로드 중인 핸들이 있으면 그 핸들에 콜백 붙이고 종료 (중복 로드 방지)
        if (clipHandles.TryGetValue(key, out var existingHandle) && existingHandle.IsValid())
        {
            existingHandle.Completed += op =>
            {
                if (op.Status == AsyncOperationStatus.Succeeded && op.Result != null)
                    uiSfxSource.PlayOneShot(op.Result);
            };
            return;
        }

        // 캐시에 없으면 비동기 로드 후 재생(UI는 로드 전에 미리 PreloadGroup 하자)
        var handle = Addressables.LoadAssetAsync<AudioClip>(key);
        clipHandles[key] = handle; // 먼저 등록해 중복 호출 방지

        Addressables.LoadAssetAsync<AudioClip>(key).Completed += (op) =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                var c = op.Result;
                clipCache[key] = c;
                uiSfxSource.PlayOneShot(c);
            }
            else
            {
                Debug.LogWarning($"AudioManager: UI SFX 로드 실패: {key}");

                // 실패 시 등록된 핸들 정리
                if (clipHandles.TryGetValue(key, out var h) && h.Equals(op))
                    clipHandles.Remove(key);
            }
        };
    }
    #endregion

    #region 3D SFX 플레이 (SfxPool)
    /// <summary>
    /// Addressable 주소로부터 AudioClip을 로드하여 SfxPool로 재생
    /// 로드 핸들은 clipHandles[address]에 보관 (자주 사용하면 캐시로 유지)
    /// </summary>
    public void Play3dSfxByAddress(string address, Vector3 pos, float volume = 1f, float pitch = 1f, bool cacheClip = true)
    {
        if (string.IsNullOrEmpty(address) || sfxPool == null) return;

        // 이미 캐시된 경우 즉시 재생
        if (clipCache.TryGetValue(address, out var cached) && cached != null)
        {
            sfxPool.PlayAt(cached, pos, volume, pitch);
            return;
        }

        // 로드 후 재생
        var h = Addressables.LoadAssetAsync<AudioClip>(address);
        h.Completed += op =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded && op.Result != null)
            {
                var clip = op.Result;
                sfxPool.PlayAt(clip, pos, volume, pitch);
                if (cacheClip)
                {
                    clipCache[address] = clip;
                    clipHandles[address] = op;
                }
            }
            else
            {
                Debug.LogWarning($"AudioManager: 3D SFX 로드 실패: {address}");
            }
        };
    }

    /// <summary>
    /// 캐시된 AudioClip으로 재생 (Addressables 로드 없이)
    /// </summary>
    public void Play3dSfxFromCache(string key, Vector3 pos, float volume = 1f, float pitch = 1f)
    {
        if (sfxPool == null) return;
        if (string.IsNullOrEmpty(key)) return;
        if (clipCache.TryGetValue(key, out var clip) && clip != null)
        {
            sfxPool.PlayAt(clip, pos, volume, pitch);
        }
    }

    /// <summary>
    /// 캐시된 개별 클립(주소) 언로드 (사용자 호출로 메모리 관리)
    /// </summary>
    public void ReleaseClip(string address)
    {
        if (string.IsNullOrEmpty(address)) return;
        if (clipHandles.TryGetValue(address, out var h))
        {
            if (h.IsValid()) Addressables.Release(h);
            clipHandles.Remove(address);
        }
        if (clipCache.ContainsKey(address)) clipCache.Remove(address);
    }
    #endregion

    #region BGM (Addressables + Crossfade)
    /// <summary>
    /// BGM 주소를 비동기 로드해서 재생(크로스페이드)
    /// 로드 핸들을 보관하면 이후 재사용 또는 Release 가능
    /// </summary>
    public void PlayBGMAddress(string address, float fadeTime = 0.6f, bool cacheBgm = true)
    {
        if (string.IsNullOrEmpty(address)) return;

        // 이미 캐시되었으면 바로 크로스페이드
        if (bgmHandles.TryGetValue(address, out var existingHandle) && existingHandle.IsValid() && existingHandle.Status == AsyncOperationStatus.Succeeded)
        {
            CrossfadeToNewBgm(existingHandle.Result, fadeTime);
            return;
        }

        // 로드
        var h = Addressables.LoadAssetAsync<AudioClip>(address);
        h.Completed += op =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded && op.Result != null)
            {
                var clip = op.Result;
                if (cacheBgm)
                {
                    bgmHandles[address] = op;
                }
                CrossfadeToNewBgm(clip, fadeTime);
            }
            else
            {
                Debug.LogWarning($"AudioManager: BGM 로드 실패: {address}");
            }
        };
    }

    private Coroutine bgmFadeCoroutine = null;

    private void CrossfadeToNewBgm(AudioClip newClip, float fadeTime)
    {
        // 중복 호출 안전성: 이미 같은 clip이면 무시
        if (currentBgmSource != null && currentBgmSource.clip == newClip) return;

        if (bgmFadeCoroutine != null) StopCoroutine(bgmFadeCoroutine);
        bgmFadeCoroutine = StartCoroutine(CrossfadeRoutine(newClip, fadeTime));
    }

    private IEnumerator CrossfadeRoutine(AudioClip newClip, float fadeTime)
    {
        // 페이드 아웃 이전 소스
        var from = currentBgmSource;
        // 교체 대상은 other
        var to = (currentBgmSource == bgmSourceA) ? bgmSourceB : bgmSourceA;

        // 준비: to에 클립 할당
        to.clip = newClip;
        to.volume = 0f;
        to.loop = true;
        to.Play();

        float t = 0f;
        float fromStartVol = (from != null) ? from.volume : 1f;
        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime;
            float p = Mathf.Clamp01(t / fadeTime);
            if (from != null) from.volume = Mathf.Lerp(fromStartVol, 0f, p);
            to.volume = Mathf.Lerp(0f, bgmLinear, p); // target vol respects bgmLinear
            yield return null;
        }

        // 완전 전환
        if (from != null)
        {
            from.Stop();
            from.clip = null;
        }

        to.volume = bgmLinear;
        currentBgmSource = to;
        bgmFadeCoroutine = null;
    }
    #endregion

    #region 해제/클린업
    private void OnDestroy()
    {
        // group handles 해제
        foreach (var kv in groupHandles)
        {
            var h = kv.Value;
            if (h.IsValid()) Addressables.Release(h);
        }
        groupHandles.Clear();

        // 개별 clip handles 해제
        foreach (var kv in clipHandles)
        {
            var h = kv.Value;
            if (h.IsValid()) Addressables.Release(h);
        }
        clipHandles.Clear();
        clipCache.Clear();

        // bgm handles 해제
        foreach (var kv in bgmHandles)
        {
            var h = kv.Value;
            if (h.IsValid()) Addressables.Release(h);
        }
        bgmHandles.Clear();
    }
    #endregion
}
