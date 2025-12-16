using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;


public class VolumeSnapshotController : MonoBehaviour
{
    private static VolumeSnapshotController current;
    public static VolumeSnapshotController Current
    {
        get
        {
            // 이미 캐시되어 있으면 그대로 사용
            if (current != null) return current;

            // 씬에서 한 번만 찾아서 캐시
            current = FindFirstObjectByType<VolumeSnapshotController>();
            if (current == null)
                Debug.LogWarning("[VolumeSnapshotController] 현재 씬에서 찾을 수 없음");
            return current;
        }
    }

    [SerializeField] private Volume exploration;
    [SerializeField] private Volume combat;
    [SerializeField] private Volume timeStop;
    [SerializeField] private Volume dashVolume;
    [SerializeField] private Volume hitVignette;
    [SerializeField] private Volume timeAbilityVolume;


    [SerializeField,Range(0.05f, 2f)]
    private float blendTime = 0.35f;

    [Header("대쉬 웨이트")]
    [Range(0f, 1f)] public float dashScaleInExploration = 1f;
    [Range(0f, 1f)] public float dashScaleInCombat = 0.5f;
    [Range(0f, 1f)] public float dashScaleInTimeStop = 0.7f;

    private Coroutine blend;
    private Coroutine dashPulse;
    private Coroutine hitPulse;
    private Coroutine timeAbilityBlend;

    public enum Snapshot { Exploration, Combat, TimeStop }
    private Snapshot currentSnapshot = Snapshot.Exploration;

    private void Awake()
    {
        current = this;

        SetSnapshot(Snapshot.Exploration);

        if (dashVolume != null)
            dashVolume.weight = 0f;

        if (hitVignette != null)
            hitVignette.weight = 0f;

        if (timeAbilityVolume != null)
            timeAbilityVolume.weight = 0f;
    }

    private void OnDestroy()
    {
        if (current == this)
            current = null;
    }

    // 스냅샷 전환
    public void SetSnapshot(Snapshot target)
    {
        currentSnapshot = target;

        if(blend != null)
            StopCoroutine(blend);

        blend = StartCoroutine(BlendTo(target));
    }

    private IEnumerator BlendTo(Snapshot target)
    {
        float t = 0f;
        float e0 = exploration.weight;
        float c0 = combat.weight;
        float s0 = timeStop.weight;

        float e1 = (target == Snapshot.Exploration) ? 1f : 0f;
        float c1 = (target == Snapshot.Combat)      ? 1f : 0f;
        float s1 = (target == Snapshot.TimeStop)    ? 1f : 0f;

        while (t < blendTime)
        {
            t += Time.unscaledDeltaTime; // 시간정지에도 부드럽게
            float k = Mathf.SmoothStep(0, 1, t / blendTime);
            exploration.weight = Mathf.Lerp(e0, e1, k);
            combat.weight      = Mathf.Lerp(c0, c1, k);
            timeStop.weight    = Mathf.Lerp(s0, s1, k);
            yield return null;
        }

        exploration.weight = e1; combat.weight = c1; timeStop.weight = s1;
        blend = null;
    }

    // 대쉬 펄스
    public void PlayDashPulse(float maxWeight, float duration)
    {
        if (dashVolume == null)
            return;

        float scale = currentSnapshot switch
        {
            Snapshot.Combat   => dashScaleInCombat,
            Snapshot.TimeStop => dashScaleInTimeStop,
            _                 => dashScaleInExploration
        };

        maxWeight = Mathf.Clamp01(maxWeight) * scale;
        duration  = Mathf.Max(0.01f, duration);

        if (dashPulse != null)
            StopCoroutine(dashPulse);

        dashPulse = StartCoroutine(DashPulseRoutine(maxWeight, duration));
    }

    private IEnumerator DashPulseRoutine(float maxWeight, float duration)
    {
        float t = 0f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float n = t / duration;
            float eased = 1f - (n * n); // ease-out

            dashVolume.weight = maxWeight * eased;
            yield return null;
        }

        dashVolume.weight = 0f;
        dashPulse = null;
    }

    public void PlayHitVignette(float maxWeight = 1f, float duration = 0.2f)
    {
        if (hitVignette == null)
            return;

        if (hitPulse != null)
            StopCoroutine(hitPulse);

        hitPulse = StartCoroutine(HitPulseRoutine(maxWeight, duration));
    }

    private IEnumerator HitPulseRoutine(float maxWeight, float duration)
    {
        hitVignette.weight = 0f;

        float half = duration * 0.3f;
        float t = 0f;

        // 올라가는 구간
        while (t < half)
        {
            t += Time.unscaledDeltaTime;
            hitVignette.weight = Mathf.Lerp(0f, maxWeight, t / half);
            yield return null;
        }

        // 내려가는 구간
        t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            hitVignette.weight = Mathf.Lerp(maxWeight, 0f, t / duration);
            yield return null;
        }

        hitVignette.weight = 0f;
        hitPulse = null;
    }

    public void SetTimeAbilityWeight(float targetWeight, float duration = 0.25f)
    {
        if (timeAbilityVolume == null)
        return;

        targetWeight = Mathf.Clamp01(targetWeight);
        duration = Mathf.Max(0.01f, duration);

        if (timeAbilityBlend != null)
            StopCoroutine(timeAbilityBlend);

        timeAbilityBlend = StartCoroutine(TimeAbilityBlendRoutine(targetWeight, duration));
    }

    private IEnumerator TimeAbilityBlendRoutine(float targetWeight, float duration)
    {
        float start = timeAbilityVolume.weight;
        float t = 0f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float k = Mathf.SmoothStep(0f, 1f, t / duration);
            timeAbilityVolume.weight = Mathf.Lerp(start, targetWeight, k);
            yield return null;
        }

        timeAbilityVolume.weight = targetWeight;
        timeAbilityBlend = null;
    }
}
