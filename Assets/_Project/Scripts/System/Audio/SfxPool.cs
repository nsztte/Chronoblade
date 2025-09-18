using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SfxPool : Pool<AudioSource>
{
    [Header("SFX 옵션")]
    [SerializeField] private float returnMargin = 0.05f; // 재생 종료 후 추가 여유시간

    // Addressables로 로드했을 경우의 핸들(있으면 Release 필요)
    private AsyncOperationHandle<GameObject>? prefabHandle = null;
    private bool prefabFromAddressables = false;

    // 루프 관리: key -> AudioSource (같은 key로 PlayLoop 호출 시 이전 루프 자동 종료)
    private readonly Dictionary<string, AudioSource> activeLoopByKey = new Dictionary<string, AudioSource>();

    /// <summary>
    /// 에디터에서 prefab을 지정하고 사용하는 경우: 그냥 InitPool() 호출
    /// </summary>
    public override void InitPool()
    {
        base.InitPool();
    }

    /// <summary>
    /// Addressables 주소로 prefab을 로드해서 풀 초기화
    /// </summary>
    public IEnumerator InitFromAddressableAsync(string prefabAddress, int preloadCount = -1)
    {
        if (prefabFromAddressables && prefab != null)
        {
            if (preloadCount > 0) initialSize = preloadCount;
            InitPool();
            yield break;
        }

        var handle = Addressables.LoadAssetAsync<GameObject>(prefabAddress);
        yield return handle;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            prefab = handle.Result;
            prefabHandle = handle;
            prefabFromAddressables = true;

            if (preloadCount > 0) initialSize = preloadCount;
            InitPool();
        }
        else
        {
            Debug.LogWarning($"[SfxPool] Addressables 로드 실패: {prefabAddress}");
        }
    }

    /// <summary>
    /// AudioClip으로 위치 기반 3D 재생. 자동 반환.
    /// </summary>
    public AudioSource PlayAt(AudioClip clip, Vector3 pos, float volume = 1f, float pitch = 1f)
    {
        if (clip == null) return null;

        var src = Get();
        if (src == null)
        {
            // 풀 비어있고 확장 안됨 -> 폴백: 로컬 임시 오디오소스 사용 또는 무시
            Debug.LogWarning($"[{name}] SfxPool 비어있음 - 플레이 스킵 {clip.name}");
            return null;
        }

        // 설정
        src.transform.position = pos;
        src.clip = clip;
        src.volume = volume;
        src.pitch = pitch;
        src.spatialBlend = 1f; // 3D
        src.loop = false;
        src.Play();

        // 자동 반환 (clip.length 기준)
        ReleaseAfter(src, clip.length + returnMargin);
        return src;
    }

    /// <summary>
    /// 루프 음원 재생
    /// key: 같은 key 사용 시 기존 루프를 종료하고 새 루프로 교체
    /// </summary>
    public AudioSource PlayLoop(AudioClip clip, Vector3 pos, string key = null, bool stopPrevious = true, float volume = 1f, float pitch = 1f)
    {
        if (clip == null) return null;

        if (!string.IsNullOrEmpty(key) && stopPrevious)
        {
            if (activeLoopByKey.TryGetValue(key, out var prev) && prev != null)
            {
                Release(prev); // 안전하게 정지+반환
                activeLoopByKey.Remove(key);
            }
        }

        var src = Get();
        if (src == null) return null;

        src.transform.position = pos;
        src.clip = clip;
        src.loop = true;
        src.volume = volume;
        src.pitch = pitch;
        src.spatialBlend = 1f;
        src.Play();

        // 키가 주어지면 맵에 보관 — 나중에 StopLoopByKey로 제어 가능
        if (!string.IsNullOrEmpty(key))
            activeLoopByKey[key] = src;

        return src;
    }

    // 수동 정지: AudioSource로 직접 중지 후 반환
    public void StopLoop(AudioSource src)
    {
        if (src == null) return;
        // 키 맵에서 제거(있다면)
        var keys = activeLoopByKey.Where(kv => kv.Value == src).Select(kv => kv.Key).ToArray();
        foreach (var k in keys) activeLoopByKey.Remove(k);

        // 실제 반환
        Release(src);
    }

    public void StopLoopByKey(string key)
    {
        if (string.IsNullOrEmpty(key)) return;
        if (activeLoopByKey.TryGetValue(key, out var src) && src != null)
        {
            activeLoopByKey.Remove(key);
            Release(src);
        }
    }

    // 풀에서 Release가 호출될 때 오디오 상태 정리하도록 훅 구현
    protected override void OnBeforeRelease(AudioSource item)
    {
        if (item == null) return;
        try
        {
            // 루프 중이거나 재생중이면 정지
            if (item.isPlaying) item.Stop();
            item.clip = null;
            item.loop = false;
            item.volume = 1f;
            item.pitch = 1f;
        }
        catch { /* 안전성: 예외 무시 */ }

        // activeLoopByKey에 등록되어 있으면 제거
        var keys = activeLoopByKey.Where(kv => kv.Value == item).Select(kv => kv.Key).ToArray();
        foreach (var k in keys) activeLoopByKey.Remove(k);
    }

    protected virtual void OnDestroy()
    {
        // Addressables로 로드한 prefab 핸들 해제
        if (prefabFromAddressables && prefabHandle.HasValue)
        {
            if (prefabHandle.Value.IsValid())
                Addressables.Release(prefabHandle.Value);
            prefabHandle = null;
        }
    }
}
