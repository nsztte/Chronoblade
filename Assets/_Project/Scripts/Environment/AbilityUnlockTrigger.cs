using UnityEngine;
using System.Collections;

public enum AbilityKind { Dash, TimeSlow, TimeStop, TimeRewind, TimeFastForward }

[RequireComponent(typeof(SaveId)), RequireComponent(typeof(GenericInteractionSaveProxy))]
public class AbilityUnlockTrigger : MonoBehaviour, IInteractableSavable
{
    [Header("해금할 능력들")]
    [SerializeField] private AbilityKind[] abilities;

    [Header("흡수 연출 세팅")]
    [SerializeField] private float absorbDuration = 0.6f;       // 전체 연출 시간
    [SerializeField] private Vector3 absorbOffset = new Vector3(0, 1.5f, 0); // 플레이어 머리 위 정도

    private Collider col;
    private ParticleSystem[] systems;
    private float[] startSpeeds;
    private Vector3 initialScale;

    // 세이브용 상태
    [SerializeField] private bool activated;

    private void Awake()
    {
        col = GetComponent<Collider>();
        col.isTrigger = true;

        systems = GetComponentsInChildren<ParticleSystem>(includeInactive: true);
        startSpeeds = new float[systems.Length];

        for (int i = 0; i < systems.Length; i++)
            startSpeeds[i] = systems[i].main.simulationSpeed;

        if (activated)
        {
            if (col != null) col.enabled = false;
            gameObject.SetActive(false);
        }

        initialScale = transform.localScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;
        if (!other.CompareTag("Player")) return;

        activated = true;

        foreach (var a in abilities)
        {
            switch (a)
            {
                case AbilityKind.Dash:
                    PlayerManager.Instance.UnlockDash();
                    UIManager.Instance?.TutorialUI?.ShowTutorial(
                        "Ability_Dash",
                        "<대쉬 해금>\n[Left Alt] 대쉬\n빠르게 돌진한다"
                    );
                    break;

                case AbilityKind.TimeSlow:
                    TimeManager.Instance.UnlockTimeSkill(TimeState.Slow);
                    UIManager.Instance?.TutorialUI?.ShowTutorial(
                        "Time_Slow",
                        "<시간 슬로우 해금>\n[Q 누르기] 시간 슬로우\n시간을 느리게 만든다"
                    );
                    break;
                case AbilityKind.TimeStop:
                    TimeManager.Instance.UnlockTimeSkill(TimeState.Stop);
                    UIManager.Instance?.TutorialUI?.ShowTutorial(
                        "Time_Stop",
                        "<시간 정지 해금>\n[E 누르기] 시간 정지\n시간을 멈춘다"
                    );
                    break;
                case AbilityKind.TimeRewind:
                    TimeManager.Instance.UnlockTimeSkill(TimeState.Rewind);
                    UIManager.Instance?.TutorialUI?.ShowTutorial(
                        "Time_Rewind",
                        "<시간 되감기 해금>\n[Q 길게 누르기] 시간 되감기\n시간을 과거로 돌린다"
                    );
                    break;
                case AbilityKind.TimeFastForward:
                    TimeManager.Instance.UnlockTimeSkill(TimeState.FastForward);
                    UIManager.Instance?.TutorialUI?.ShowTutorial(
                        "Time_FastForward",
                        "<시간 빨리감기 해금>\n[E 길게 누르기] 시간 빨리감기\n시간이 빠르게 흐른다"
                    );
                    break;
            }
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

    #region IInteractableSavable 구현
    public bool IsActivated()
    {
        return activated;
    }

    public bool IsHeld()
    {
        return false;
    }

    public bool TryGetWorldPose(out Vector3 pos, out Quaternion rot)
    {
        pos = transform.position;
        rot = transform.rotation;
        return true;
    }

    public void ApplyWorldPose(Vector3 pos, Quaternion rot)
    {
        transform.SetPositionAndRotation(pos, rot);
    }

    public void ApplyActivated(bool value)
    {
        activated = value;

        if (activated)
        {
            if (col != null) col.enabled = false;
            gameObject.SetActive(false);
        }
        else
        {
            if (col != null) col.enabled = true;
            gameObject.SetActive(true);

            // 스케일 복구
            transform.localScale = initialScale;

            // 파티클 복구
            for (int i = 0; i < systems.Length; i++)
            {
                var main = systems[i].main;
                main.simulationSpeed = startSpeeds[i];
                systems[i].Clear();
                systems[i].Play();
            }
        }
    }

    public void ApplyHeld(bool value){}
    #endregion
}