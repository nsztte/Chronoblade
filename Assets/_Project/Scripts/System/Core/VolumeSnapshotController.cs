using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;


public class VolumeSnapshotController : MonoBehaviour
{
    [SerializeField] private Volume exploration;
    [SerializeField] private Volume combat;
    [SerializeField] private Volume timeStop;

    [Range(0.05f, 2f)]
    public float blendTime = 0.35f;

    private Coroutine blend;

    public enum Snapshot { Exploration, Combat, TimeStop }

    private void Start()
    {
        SetSnapshot(Snapshot.Exploration);
    }

    public void SetSnapshot(Snapshot target)
    {
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8)) SetSnapshot(Snapshot.Exploration);
        if (Input.GetKeyDown(KeyCode.Alpha9)) SetSnapshot(Snapshot.Combat);
        if (Input.GetKeyDown(KeyCode.Alpha0)) SetSnapshot(Snapshot.TimeStop);
    }
}
