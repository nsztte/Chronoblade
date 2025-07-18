using TreeEditor;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(BossPhaseManager))]
public class BossController : MonoBehaviour, IDamageable
{
    [Header("FSM")]
    private BossStateMachine stateMachine;
    [SerializeField] private BaseBossState currentState;

    [Header("페이즈")]
    private BossPhaseManager phaseManager;

    [Header("스탯")]
    [SerializeField] private float maxHP = 1000f;
    [SerializeField] private float currentHP;
    [SerializeField] private int damage = 20;

    private Animator animator;
    private Transform player;

    public BossPhaseManager PhaseManager => phaseManager;

    private void Awake()
    {
        stateMachine = new BossStateMachine();
        phaseManager = GetComponent<BossPhaseManager>();
        animator = GetComponent<Animator>();

        currentHP = maxHP;
    }

    private void Start()
    {
        player = PlayerManager.Instance.PlayerTransform;

        // 초기 상태 설정
        var introState = new BossIntroState(this, stateMachine);
        stateMachine.Initialize(introState);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0);

        phaseManager.UpdatePhase(currentHP, maxHP);
        Debug.Log($"Boss HP: {currentHP}, Phase: {phaseManager.CurrentPhase}");
    }

    public void PlayAnimation(string triggerName)
    {
        animator.ResetTrigger(triggerName);
        animator.SetTrigger(triggerName);
    }

    public float GetCurrentAnimationLength()
    {
        if(animator.GetCurrentAnimatorClipInfoCount(0) == 0) return 0f;

        var clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        return clipInfo[0].clip.length;
    }

    public float GetAnimationClipLengthFromState(string stateName)
    {
        RuntimeAnimatorController controller = animator.runtimeAnimatorController;

        foreach(var clip in controller.animationClips)
        {
            if(clip.name == stateName)
            {
                return clip.length;
            }
        }

        Debug.LogWarning($"애니메이션 클립 '{stateName}'을 찾을 수 없습니다.");
        return 1f;
    }

    public void SpawnSlowZoneAtPosition(Vector3 position)
    {
        Debug.Log($"슬로우존 생성");
        // TODO: 슬로우존 생성 로직 구현 (Instantiate or Pooling)
    }

    public void StartTimeStopEffect()
    {
        Debug.Log("타임 스탑 효과 시작");
        // TODO: 화면 흑백효과, 타임스케일 조정 등
    }

    public void EndTimeStopEffect()
    {
        Debug.Log("타임 스탑 효과 종료");
        // TODO: 복원
    }

    public void LookAtPlayer()
    {
        if(player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }

    public void ApplyKnockback(float force){}
}
