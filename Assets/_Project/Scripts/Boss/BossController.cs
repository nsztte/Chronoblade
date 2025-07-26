using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Animator), typeof(BossPhaseManager))]
public class BossController : MonoBehaviour, IDamageable
{
    [Header("FSM")]
    private BossStateMachine stateMachine;
    [SerializeField] private BaseBossState currentState;

    [Header("스탯")]
    [SerializeField] private float maxHP = 1000f;
    [SerializeField] private float currentHP;
    [SerializeField] private float attackRange = 1.5f;
    // [SerializeField] private int damage = 20;

    [Header("공격 프리팹")]
    public GameObject slowZonePrefab;
    public GameObject minePrefab;
    public GameObject energyBoltPrefab;

    [Header("히트박스 마커")]
    [SerializeField] private Transform slashHitboxMarker;
    private Coroutine slashCoroutine;

    [Header("히트박스 세팅")]
    [SerializeField] private LayerMask hitboxLayer;

    [Header("매니저")]
    [SerializeField] private PuzzleClockManager puzzleClockManager;
    private BossPhaseManager phaseManager;

    [Header("취약점 세팅")]
    [SerializeField] private GameObject weakPointObject;

    // 참조
    private Animator animator;
    private Transform player;
    private Collider col;

    public BossPhaseManager PhaseManager => phaseManager;
    public PuzzleClockManager PuzzleClockManager => puzzleClockManager;
    public Transform Player => player;

    private void Awake()
    {
        stateMachine = new BossStateMachine();
        phaseManager = GetComponent<BossPhaseManager>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider>();

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
        if(Input.GetKeyDown(KeyCode.P)) // @@@@@@퍼즐 테스트용@@@@@ 나중에 반드시 지워야함
        {
            currentHP = 500;
        }
        
        stateMachine.Update();

        phaseManager.UpdatePhase(currentHP, maxHP);
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0);

        phaseManager.UpdatePhase(currentHP, maxHP);
        Debug.Log($"Boss HP: {currentHP}, Phase: {phaseManager.CurrentPhase}");
    }

    public void LookAtPlayer(float rotationSpeed)
    {
        if(player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public bool IsPlayerInAttackRange()
    {
        return Vector3.Distance(player.position, transform.position) <= attackRange;
    }

    public void SetInvincibility(bool isInvincible)
    {
        col.enabled = !isInvincible;
        Debug.Log($"무적상태: {isInvincible}, 콜라이더 활성화: {col.enabled}");
    }

    public void SetHPWithPercent(int percent)
    {
        currentHP = maxHP * percent / 100f;
    }

    #region 애니메이션 관련 함수
    public void PlayAnimation(string triggerName)
    {
        animator.ResetTrigger(triggerName);
        animator.SetTrigger(triggerName);
    }

    public void PlayAnimation(string boolName, bool value)
    {
        animator.SetBool(boolName, value);
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
    #endregion

    #region 패턴 관련 함수
    public void SpawnSlowZoneAtPosition(Vector3 position)
    {
        Debug.Log($"슬로우존 생성");
        GameObject slowZone = GameObject.Instantiate(slowZonePrefab, position, Quaternion.identity);
    }

    public void SpawnMineAtPosition(Vector3 position)
    {
        Debug.Log($"마인 생성");
        GameObject mine = GameObject.Instantiate(minePrefab, position, Quaternion.identity);
    }

    public void SpawnEnergyBolt(Vector3 position, Vector3 direction, bool isHoming = false)
    {
        GameObject energyBolt = GameObject.Instantiate(energyBoltPrefab, position, Quaternion.LookRotation(direction));
        if (energyBolt.TryGetComponent(out EnergyBoltProjectile bolt))
        {
            if(isHoming)
            {
                bolt.SetTarget(player);
            }
            else
            {
                bolt.SetDirection(direction);
            }
        }
        else
        {
            Debug.LogWarning("EnergyBoltProjectile 스크립트가 프리팹에 없음");
        }
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

    public void StartPuzzle()
    {
        // 퍼즐 시작
        puzzleClockManager.gameObject.SetActive(true);
        puzzleClockManager.StartPuzzle();
    }

    public void EndPuzzle()
    {
        // 퍼즐 종료
        puzzleClockManager.gameObject.SetActive(false);
    }

    public void SetClockPartsTarget(bool isPlayer)
    {
        puzzleClockManager.SetClockPartsTarget(isPlayer);
    }

    public void WaitPartsArrival(Action onComplete)
    {
        StartCoroutine(CoWaitArrived(onComplete));
    }

    private IEnumerator CoWaitArrived(Action onComplete)
    {
        yield return new WaitUntil(() => puzzleClockManager.AreAllPartsArrived());
        onComplete?.Invoke();
    }

    public void ExposeWeakPoint(float duration, Action onComplete = null)
    {
        StartCoroutine(ExposeCoroutine(duration, onComplete));
    }

    private IEnumerator ExposeCoroutine(float duration, Action onComplete)
    {
        weakPointObject.SetActive(true);
        animator.SetBool("IsWeakExposed", true);

        yield return new WaitForSeconds(duration);

        weakPointObject.SetActive(false);
        animator.SetBool("IsWeakExposed", false);

        onComplete?.Invoke();
    }
    #endregion

    #region 공격 애니메이션 이벤트 등록 함수
    // 패이즈1
    public void TriggerHorizontalSlash()
    {
        if(stateMachine.CurrentState is HorizontalSlashState horizontalSlashState)
        {
            horizontalSlashState.isWindingUp = false;
            TriggerFollowSlashHitbox(0.1f, horizontalSlashState.damage);
        }
    }
    public void TriggerVerticalSlash()
    {
        if(stateMachine.CurrentState is VerticalSmashState verticalSmashState)
        {
            verticalSmashState.isWindingUp = false;
            TriggerFollowSlashHitbox(0.1f, verticalSmashState.damage);
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
            timeStopState.isWindingUp = false;
        }
    }

    // TriggerTimeStopAttack, LeapSmash, TriggerDoubleSlashCombo 에서 공통으로 사용
    public void TriggerAttackHitbox()
    {
        if(stateMachine.CurrentState is TimeStopAttackState timeStopState)
        {
            TriggerParryHitbox(0.1f, timeStopState.damage);
        }
        else if(stateMachine.CurrentState is LeapSmashState leapSmashState)
        {
            TriggerParryHitbox(0.05f, leapSmashState.damage);
        }
        else if(stateMachine.CurrentState is DoubleSlashComboState doubleSlashComboState)
        {
            TriggerFollowSlashHitbox(0.1f, doubleSlashComboState.damage);
        }
    }

    // 패이즈2
    public void TriggerDelayedMine()
    {
        if(stateMachine.CurrentState is DelayedMineState delayedMineState)
        {
            delayedMineState.SpawnMine();
        }
    }

    public void TriggerDoubleSlashCombo()
    {
        if(stateMachine.CurrentState is DoubleSlashComboState doubleSlashComboState)
        {
            doubleSlashComboState.isWindingUp = false;
            // 각 공격 프레임에 TriggerAttackHitbox 이벤트 등록
        }
    }

    public void TriggerRapidEnergyShot()
    {
        if(stateMachine.CurrentState is RapidEnergyShotState rapidEnergyShotState)
        {
            rapidEnergyShotState.isWindingUp = false;
        }
    }

    public void TriggerLeapSmash()
    {
        if(stateMachine.CurrentState is LeapSmashState leapSmashState)
        {
            leapSmashState.isWindingUp = false;
        }
    }
    #endregion

    #region 히트박스 트리거
    private void TriggerFollowSlashHitbox(float duration, int damage)
    {
        if(slashCoroutine != null) StopCoroutine(slashCoroutine);
        slashCoroutine = StartCoroutine(FollowSlashHitbox(duration, damage));
    }

    private IEnumerator FollowSlashHitbox(float duration, int damage)
    {
        float timer = 0f;

        while(timer < duration)
        {
            Vector3 center = slashHitboxMarker.position;
            Vector3 halfSize = slashHitboxMarker.localScale * 0.5f;
            Quaternion rotation = slashHitboxMarker.rotation;

            Collider[] hits = Physics.OverlapBox(center, halfSize, rotation, hitboxLayer);
            foreach(var hit in hits)
            {
                if(hit.TryGetComponent(out IDamageable damageable))
                {
                    Debug.Log($"히트박스 히트: {hit.name}");
                    damageable.TakeDamage(damage);
                    yield break;
                }
            }

            yield return null;
            timer += Time.deltaTime;
        }

        slashCoroutine = null;
    }
    
    private void TriggerParryHitbox(float duration, int damage)
    {
        if(slashCoroutine != null) StopCoroutine(slashCoroutine);
        slashCoroutine = StartCoroutine(ParrySlashHitbox(duration, damage));
    }

    private IEnumerator ParrySlashHitbox(float duration, int damage)
    {
        float timer = 0f;

        while(timer < duration)
        {
            Vector3 center = slashHitboxMarker.position;
            Vector3 halfSize = slashHitboxMarker.localScale * 0.5f;
            Quaternion rotation = slashHitboxMarker.rotation;

            Collider[] hits = Physics.OverlapBox(center, halfSize, rotation, hitboxLayer);
            foreach(var hit in hits)
            {
                if(hit.TryGetComponent(out IDamageable damageable))
                {
                    if(damageable is PlayerManager player)
                    {
                        if(player.TryParry(Time.time))
                        {
                            Debug.Log("패리 성공");
                            stateMachine.ChangeState(new StaggerCheckState(this, stateMachine));
                            yield break;
                        }
                    }
                    damageable.TakeDamage(damage);
                    yield break;
                }
            }

            yield return null;
            timer += Time.deltaTime;
        }

        slashCoroutine = null;
    }

    private void OnDrawGizmos()
    {
        if (slashHitboxMarker == null) return;

        Vector3 center = slashHitboxMarker.position;
        Vector3 halfSize = slashHitboxMarker.localScale * 0.5f;
        Quaternion rotation = slashHitboxMarker.rotation;

        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(center, rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, halfSize * 2f);
    }
    #endregion

    public void ApplyKnockback(float force){}
}
