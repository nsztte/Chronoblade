using UnityEngine;

public class CombatTutorialManager : MonoBehaviour
{
    public static CombatTutorialManager Instance { get; private set; }

    private enum Step
    {
        None,
        Attack,
        Block,
        Parry,
        TimingCombo,
        Done
    }

    [Header("튜토리얼 활성 여부")]
    [SerializeField] private bool enableTutorial = true;

    [Header("마지막 안내 튜토리얼 설정")]
    [SerializeField] private float finishTutorialHoldTime = 2.0f;

    private Step currentStep = Step.None;
    private bool hasCompleted;

    public bool IsRunning => enableTutorial && currentStep != Step.None && currentStep != Step.Done;
    public bool HasCompleted => hasCompleted;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    /// <summary>
    /// 검을 획득했을 때, 전투 튜토리얼을 시작할 시점에서 호출
    /// </summary>
    public void StartCombatTutorial()
    {
        if (!enableTutorial)
            return;

        if (hasCompleted || IsRunning)
            return;

        currentStep = Step.Attack;
        ShowCurrentStep();
    }

    private void ShowCurrentStep()
    {
        if (UIManager.Instance == null || UIManager.Instance.TutorialUI == null)
            return;

        switch (currentStep)
        {
            case Step.Attack:
                UIManager.Instance.TutorialUI.ShowPersistentTutorial(
                    "Combat_Attack",
                    "[LMB] 공격"
                );
                break;

            case Step.Block:
                UIManager.Instance.TutorialUI.ShowPersistentTutorial(
                    "Combat_Block",
                    "[RMB] 방어"
                );
                break;

            case Step.Parry:
                UIManager.Instance.TutorialUI.ShowPersistentTutorial(
                    "Combat_Parry",
                    "[RMB] 정확한 타이밍에 방어를 해제하면 패링"
                );
                break;

            case Step.TimingCombo:
                UIManager.Instance.TutorialUI.ShowPersistentTutorial(
                    "Combat_TimingCombo",
                    "[LMB] 리듬에 맞춰 공격하면 타이밍 콤보 발동"
                );
                break;

            case Step.Done:
                // 마지막 단계: 짧은 안내만 띄우고 자동으로 종료
                UIManager.Instance.TutorialUI.ShowTutorial(
                    "Combat_Finish",
                    "전투 튜토리얼 완료!",
                    finishTutorialHoldTime,
                    false
                );
                break;
        }
    }

    /// <summary>
    /// 튜토리얼 대상 에너미에게 플레이어 공격이 적중했을 때 호출
    /// (Enemy/Watcher 쪽에서 조건 맞을 때 한 번 호출)
    /// </summary>
    public void OnAttackHitEnemy()
    {
        if (!IsRunning || currentStep != Step.Attack)
            return;

        currentStep = Step.Block;
        ShowCurrentStep();
    }

    /// <summary>
    /// 플레이어가 방어로 적의 공격을 성공적으로 막았을 때 호출
    /// </summary>
    public void OnBlockSuccess()
    {
        if (!IsRunning || currentStep != Step.Block)
            return;

        currentStep = Step.Parry;
        ShowCurrentStep();
    }

    /// <summary>
    /// 플레이어가 패링에 성공했을 때 호출
    /// </summary>
    public void OnParrySuccess()
    {
        if (!IsRunning || currentStep != Step.Parry)
            return;

        currentStep = Step.TimingCombo;
        ShowCurrentStep();
    }

    /// <summary>
    /// 플레이어가 타이밍 콤보를 한 번 성공했을 때 호출
    /// </summary>
    public void OnTimingComboSuccess()
    {
        if (!IsRunning || currentStep != Step.TimingCombo)
            return;

        currentStep = Step.Done;
        hasCompleted = true;

        ShowCurrentStep(); // 마지막 안내 튜토리얼 한 번 더 띄우고 자동 종료
    }
}
