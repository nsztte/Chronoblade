using UnityEngine;
using System.Collections;

public enum AbilityKind { Dash, TimeSlow, TimeStop, TimeRewind, TimeFastForward }
public class AbilityUnlockTrigger : MonoBehaviour
{
    [Header("해금할 능력들")]
    [SerializeField] private AbilityKind[] abilities;

    [Header("흡수 연출 세팅")]
    [SerializeField] private float absorbDuration = 0.6f;       // 전체 연출 시간
    [SerializeField] private Vector3 absorbOffset = new Vector3(0, 1.5f, 0); // 플레이어 머리 위 정도

    private Collider col;
    private ParticleSystem[] systems;
    private float[] startSpeeds;
    private bool isTriggered;

    private void Awake()
    {
        col = GetComponent<Collider>();
        col.isTrigger = true;

        systems = GetComponentsInChildren<ParticleSystem>(includeInactive: true);
        startSpeeds = new float[systems.Length];

        for (int i = 0; i < systems.Length; i++)
            startSpeeds[i] = systems[i].main.simulationSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTriggered) return;
        if (!other.CompareTag("Player")) return;

        isTriggered = true;

        foreach (var a in abilities)
        {
            switch (a)
            {
                case AbilityKind.Dash:
                    PlayerManager.Instance.UnlockDash();
                    break;

                case AbilityKind.TimeSlow:
                    TimeManager.Instance.UnlockTimeSkill(TimeState.Slow);
                    break;
                case AbilityKind.TimeStop:
                    TimeManager.Instance.UnlockTimeSkill(TimeState.Stop);
                    break;
                case AbilityKind.TimeRewind:
                    TimeManager.Instance.UnlockTimeSkill(TimeState.Rewind);
                    break;
                case AbilityKind.TimeFastForward:
                    TimeManager.Instance.UnlockTimeSkill(TimeState.FastForward);
                    break;
            }

            UIManager.Instance.ShowToast($"{a} 스킬 해금");
        }

        // TODO: 플레이어에게 능력이 흡수되는 연출, 기술 설명 UI
        col.enabled = false;

        StartCoroutine(AbsorbRoutine(other.transform));
    }

    private IEnumerator AbsorbRoutine(Transform player)
    {
        float t = 0f;

        Vector3 startPos = transform.position;
        Vector3 targetPos = player.position + absorbOffset;

        Vector3 startScale = transform.localScale;

        while (t < absorbDuration)
        {
            t += Time.deltaTime;
            float ratio = Mathf.Clamp01(t / absorbDuration);
            float speedRatio = 1f - ratio; // 1 → 0 으로 감소

            // 위치: 플레이어 쪽으로 이동
            transform.position = Vector3.Lerp(startPos, targetPos, ratio);

            // 스케일: 점점 줄어듦
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, ratio);

            // 파티클 재생 속도도 서서히 줄이기
            for (int i = 0; i < systems.Length; i++)
            {
                var main = systems[i].main;
                main.simulationSpeed = startSpeeds[i] * speedRatio;
            }

            yield return null;
        }

        // 끝났으면 완전히 정지 + 정리
        for (int i = 0; i < systems.Length; i++)
        {
            var main = systems[i].main;
            main.simulationSpeed = 0f;
            systems[i].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        gameObject.SetActive(false);
    }
}