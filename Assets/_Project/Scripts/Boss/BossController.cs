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

    [Header("공격 프리팹")]
    public GameObject slowZonePrefab;

    private Animator animator;
    private Transform player;

    public BossPhaseManager PhaseManager => phaseManager;
    public Transform Player => player;

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
        GameObject slowZone = GameObject.Instantiate(slowZonePrefab, position, Quaternion.identity);
    }

    public void StartTimeStopEffect()
    {
        Debug.Log("타임 스탑 효과 시작");
        // TODO: 화면 흑백효과
        TimeManager.Instance.SetTimeStop(true);
        PlayerManager.Instance.PlayerController.ApplyStatus(StatusEffectType.Freeze);
    }

    public void EndTimeStopEffect()
    {
        Debug.Log("타임 스탑 효과 종료");
        // TODO: 복원
        TimeManager.Instance.SetTimeStop(false);
        PlayerManager.Instance.PlayerController.RemoveStatus(StatusEffectType.Freeze);
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

    #region 공격 애니메이션 이벤트 등록 함수
    public void TriggerHorizontalSlash()
    {
        if(stateMachine.CurrentState is HorizontalSlashState horizontalSlashState)
        {
            horizontalSlashState.isWindingUp = false;
        }
    }
    public void TriggerVerticalSlash()
    {
        if(stateMachine.CurrentState is VerticalSmashState verticalSmashState)
        {
            verticalSmashState.isWindingUp = false;
        }
    }
    public void TriggerEnergyBolt()
    {
        if(stateMachine.CurrentState is EnergyBoltState energyBoltState)
        {
            energyBoltState.isWindingUp = false;
        }
    }

    public void TriggerSpawnSlowZone()
    {
        if(stateMachine.CurrentState is SpawnSlowZoneState slowZoneState)
        {
            slowZoneState.SpawnSlowZone();
        }
    }

    public void TriggerTimeStopAttack()
    {
        if(stateMachine.CurrentState is TimeStopAttackState timeStopState)
        {
            // 광역 슬래시 히트박스 생성 or 타격 이펙트
            timeStopState.isWindingUp = false;
        }
    }
    #endregion

    public void ApplyKnockback(float force){}
}
