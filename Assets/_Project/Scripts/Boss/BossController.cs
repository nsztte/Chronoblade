using UnityEngine;
using System.Collections;
using System;
using Tiny;

[RequireComponent(typeof(Animator), typeof(BossPhaseManager))]
public class BossController : MonoBehaviour, IDamageable
{
    [SerializeField] private bool isEndingTest = false;
    public bool IsEndingTest => isEndingTest;

    [Header("FSM")]
    private BossStateMachine stateMachine;

    [Header("스탯")]
    [SerializeField] private float maxHP = 1000f;
    [SerializeField] private float currentHP;

    [Header("Phase 튜닝")]
    [SerializeField] private float phase1AttackRange = 2.3f;
    [SerializeField] private float phase2AttackRange = 1.5f;

    [SerializeField] private float phase1DashSpeed = 13f;
    [SerializeField] private float phase1DashStopDistance = 4f;

    public float Phase1AttackRange => phase1AttackRange;
    public float Phase2AttackRange => phase2AttackRange;
    public float Phase1DashSpeed => phase1DashSpeed;
    public float Phase1DashStopDistance => phase1DashStopDistance;

    [Header("공격 프리팹")]
    public GameObject slowZonePrefab;
    public GameObject minePrefab;
    public GameObject energyBoltPrefab;

    [Header("히트박스 마커")]
    [SerializeField] private Transform slashHitboxMarker;
    private Coroutine slashCoroutine;

    [Header("히트박스 세팅")]
    [SerializeField] private LayerMask hitboxLayer;

    [Header("VFX")]
    [SerializeField] private Trail swordTrail;

    [Header("매니저")]
    [SerializeField] private PuzzleClockManager puzzleClockManager;
    private BossPhaseManager phaseManager;

    [Header("취약점 세팅")]
    [SerializeField] private Animator heartAnimator;
    [SerializeField] private GameObject weakPointObject;

    [Header("시네머신 카메라")]
    [SerializeField] private GameObject clockPuzzleCamera;

    [Header("컷씬")]
    [SerializeField] private BossPhaseTransitionCutscene phaseTransitionCutscene;
    [SerializeField] private BossEndingCutscene bossEndingCutscene;

    [Header("히트 피드백")]
    [SerializeField] private Transform hitShakeRoot;
    [SerializeField] private float hitShakeDuration = 0.08f;
    [SerializeField] private float hitShakeAmplitude = 0.02f;
    [SerializeField] private float hitShakeCooldown = 0.03f; // 연타시 과도한 흔들림 방지

    [Header("인트로 실행 상태")]
    [SerializeField] private bool hasAwakened; // 디버그용으로 SerializeField 추천
    public bool HasAwakened => hasAwakened;

    private Coroutine hitShakeCo;
    private Vector3 hitShakeRootDefaultLocalPos;
    private float lastHitShakeTime = -999f;

    // 참조
    private Animator animator;
    private Transform player;
    private Collider col;

    public BossStateMachine BossSM => stateMachine;
    public BossPhaseManager PhaseManager => phaseManager;
    public PuzzleClockManager PuzzleClockManager => puzzleClockManager;
    public Transform Player => player;
    public GameObject ClockPuzzleCamera => clockPuzzleCamera;
    public BossPhaseTransitionCutscene TC => phaseTransitionCutscene;
    public BossEndingCutscene EC => bossEndingCutscene;

    public Animator Animator => animator;
    public Animator HeartAnimator => heartAnimator;

    public int CurrentHpPercentInt
    {
        get
        {
            if (Mathf.Approximately(maxHP, 0f)) return 0;
            return Mathf.RoundToInt((currentHP / maxHP) * 100f);
        }
    }

    private void Awake()
    {
        stateMachine = new BossStateMachine();
        phaseManager = GetComponent<BossPhaseManager>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider>();

        currentHP = maxHP;

        if (hitShakeRoot != null)
            hitShakeRootDefaultLocalPos = hitShakeRoot.localPosition;
    }

    private void OnEnable()
    {
        EnemyManager.Instance?.RegisterBossCombat();
    }

    private void OnDisable()
    {
        EnemyManager.Instance?.UnregisterBossCombat();
    }

    private void Start()
    {
        player = PlayerManager.Instance.PlayerTransform;

        animator.Play("StartIdle", 0);
        // ResetToPreIntroState();
    }

    private void Update()
    {      
        stateMachine.Update();

        if (Input.GetKeyDown(KeyCode.T))
        {
            SetHPWithPercent(50);
        }
    }

    public void StartIntroState()
    {
        var introState = new BossIntroState(this, stateMachine);
        stateMachine.Initialize(introState);
    }

    public void StartPhaseTransitionState()
    {
        var transitionState = new BossPhaseTransitionState(this, stateMachine);
        stateMachine.ChangeState(transitionState);
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0);

        UpdateBossHUD();

        phaseManager.UpdatePhase(currentHP, maxHP);
        Debug.Log($"Boss HP: {currentHP}, Phase: {phaseManager.CurrentPhase}");

        PlayHitShake(damage);
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

    public bool IsPlayerInAttackRange(float attackRange)
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
        percent = Mathf.Clamp(percent, 0, 100);
        currentHP = maxHP * percent / 100f;
        UpdateBossHUD();
        phaseManager.UpdatePhase(currentHP, maxHP);
    }

    public void SetAwakened(bool awakened)
    {
        hasAwakened = awakened;
    }

    public void ResetToPreIntroState(bool invincible = true, bool hideHud = true)
    {
        // 0) 필수 참조 보정 맟 초기화화
        if (animator == null) animator = GetComponent<Animator>();
        if (phaseManager == null) phaseManager = GetComponent<BossPhaseManager>();
        if (col == null) col = GetComponent<Collider>();

        if (player == null)
            player = PlayerManager.Instance?.PlayerTransform;

        CancelInvoke();
        StopAllCoroutines();

        // 1) 진행 중 코루틴/연출 정리
        if (slashCoroutine != null)
        {
            StopCoroutine(slashCoroutine);
            slashCoroutine = null;
        }

        if (hitShakeCo != null)
        {
            StopCoroutine(hitShakeCo);
            hitShakeCo = null;

            if (hitShakeRoot != null)
                hitShakeRoot.localPosition = hitShakeRootDefaultLocalPos;
        }

        // 칼 트레일 꺼두기 (혹시 켜져있던 상태 대비)
        if (swordTrail != null)
            swordTrail.StopTrail(clear: true);

        // 2) 전투/퍼즐/약점 관련 오브젝트를 '인트로 전' 기준으로 정렬
        if (weakPointObject != null)
            weakPointObject.SetActive(false);

        // 퍼즐 매니저는 꺼진 상태가 안전(인트로 전)
        if (puzzleClockManager != null)
            puzzleClockManager.gameObject.SetActive(false);

        // 컷씬 카메라(퍼즐 카메라 등)도 기본은 꺼두는 게 안전
        if (clockPuzzleCamera != null)
            clockPuzzleCamera.SetActive(false);

        // 3) 타임스탑/프로즌 잔여 상태 정리 (혹시 남아있을 수 있으니 안전망)
        // StartTimeStopEffect()를 보스가 걸 수 있으니, 복원 시점에 남아있으면 풀어줌
        EndTimeStopEffect();

        // 4) 콜라이더/무적 상태 정렬
        SetInvincibility(invincible);

        // 5) FSM 리셋 (전투 시작/인트로 시작은 외부 트리거가 하도록)
        // BossStateMachine.Update()가 null state를 안전하게 처리한다는 전제(대부분 이렇게 구현됨)
        stateMachine = new BossStateMachine();

        // 6) 애니메이션을 "앉아있는 대기"로 고정
        AnimatorUtils.ResetAnimatorParameters(animator);
        animator.Play(hasAwakened ? "Idle" : "StartIdle", 0);

        // 7) HUD 정리
        if (hideHud)
            HideBossHUD();
        else
            ShowBossHUD();
    }

    #region 타격 피드백
    private void PlayHitShake(int damage)
    {
        if (hitShakeRoot == null) return;

        Debug.Log("PlayHitShake");

        // 연타 입력 시 과도한 위치 튐 방지
        if (Time.time - lastHitShakeTime < hitShakeCooldown) return;
        lastHitShakeTime = Time.time;

        // 데미지 크기에 따라 아주 살짝만 가중
        float amp = hitShakeAmplitude * Mathf.Clamp01(damage / 50f); // 50 기준
        amp = Mathf.Max(amp, hitShakeAmplitude * 0.4f);             // 너무 약해지지 않게

        if (hitShakeCo != null) StopCoroutine(hitShakeCo);
        hitShakeCo = StartCoroutine(HitShakeRoutine(amp, hitShakeDuration));
    }

    private IEnumerator HitShakeRoutine(float amp, float duration)
    {
        float t = 0f;
        // 시작 전에 원위치로 한번 고정(누적 오차 방지)
        hitShakeRoot.localPosition = hitShakeRootDefaultLocalPos;

        while (t < duration)
        {
            t += Time.deltaTime;

            // 프레임마다 랜덤 오프셋(미세 진동)
            Vector3 offset = new Vector3(
                UnityEngine.Random.Range(-amp, amp),
                UnityEngine.Random.Range(-amp * 0.2f, amp * 0.2f), // Y는 약하게
                UnityEngine.Random.Range(-amp, amp)
            );

            hitShakeRoot.localPosition = hitShakeRootDefaultLocalPos + offset;
            yield return null;
        }

        hitShakeRoot.localPosition = hitShakeRootDefaultLocalPos;
        hitShakeCo = null;
    }
    #endregion

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

        HideBossHUD();
    }

    public void EndPuzzle()
    {
        // 퍼즐 종료
        puzzleClockManager.gameObject.SetActive(false);

        ShowBossHUD();
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
    public void OnBossSlashTrailOn()  => swordTrail.PlayTrail(clear: true);
    public void OnBossSlashTrailOff() => swordTrail.StopTrail(clear: false);

    // 패이즈1
    public void TriggerHorizontalSlash()
    {
        PlayBossSwingSfx();

        if(stateMachine.CurrentState is HorizontalSlashState horizontalSlashState)
        {
            horizontalSlashState.isWindingUp = false;
            TriggerFollowSlashHitbox(0.1f, horizontalSlashState.damage);
        }
    }
    public void TriggerVerticalSlash()
    {
        PlayBossSwingSfx();
        
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
        PlayBossSwingSfx();
        
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
        PlayBossCastSfx();

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
        PlayBossCastSfx();
        
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

    public void PlayBossSwingSfx()
    {
        AudioManager.Instance.Play3dSfxFromCache("Boss_Swing", transform.position, 0.5f, 1.0f);
    }

    public void PlayBossCastSfx()
    {
        AudioManager.Instance.Play3dSfxFromCache("Boss_Cast", transform.position, 0.3f, 1.0f);
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
                            stateMachine.ChangeState(new StaggerCheckState(this, stateMachine, 1f));
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

    #region UI 훅
    public void ShowBossHUD()  => UIManager.Instance?.ShowBoss(currentHP, maxHP);
    private void UpdateBossHUD()=> UIManager.Instance?.SetBossHP(currentHP, maxHP);
    public void HideBossHUD()  => UIManager.Instance?.HideBoss();
    #endregion

    public void ApplyKnockback(float force){}
}
