using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public enum PatrolMode { None, RandomInRadius, WaypointsLoop }

[RequireComponent(typeof(EnemyStateMachine), typeof(EnemyTimeController), typeof(FinalComboController))]
public abstract class Enemy : MonoBehaviour, IDamageable
{
    [Header("디폴트")]
    [SerializeField] protected EnemyBehaviorData behaviorData;
    [SerializeField] protected int currentHP;
    [SerializeField] protected float destroyTime = 5f;
    [SerializeField] private Vector3 hpUIOffset = new Vector3(0, 0.3f, 0);
    [SerializeField] private GameObject hpUIPrefab;
    [SerializeField] private LayerMask obstacleLayer;

    [Header("공격 판정")]
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected float stunDuration = 1.5f;
    [SerializeField] protected float stunKnockbackForce = 1f;

    [Header("패트롤 설정")]
    [SerializeField] private PatrolMode patrolMode = PatrolMode.None;
    [SerializeField] private float patrolRadius = 6f;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float waitAtPoint = 1.0f;
    [SerializeField] private bool startAtNearest = true;
    public Vector3 HomePosition { get; private set; }

    // 리스폰 용
    public event Action<Enemy> OnDied;
    public event Action<Enemy> OnDespawned;
    private bool deathEventSent;
    private bool despawnEventSent;

    protected EnemyStateMachine fsm;
    protected EnemyTimeController timeController;
    protected FinalComboController finalComboController;

    private Collider col;
    private Animator animator;
    private NavMeshAgent agent;
    protected EnemyHPUI hpUI;

    private bool hasDetectedPlayer = false;     // 플레이어 감지
    public float LastSeenPlayerTime { get; private set; } = float.NegativeInfinity;

    #region 전투 감지 이벤트
    // 전투 감지 이벤트
    public static event Action OnCombatStarted;

    // 전투 감지 이벤트를 호출하는 정적 메서드
    public static void TriggerCombatStarted()
    {
        OnCombatStarted?.Invoke();
    }
    #endregion
   
    #region 패트롤 관련
    public PatrolMode PatrolMode => patrolMode;
    public float PatrolRadius => patrolRadius;
    public Transform[] PatrolPoints => patrolPoints;
    public float WaitAtPoint => waitAtPoint;
    public bool StartAtNearest => startAtNearest;

    [Serializable]
    public struct PatrolConfig
    {
        public PatrolMode mode;
        public float radius;
        public Transform[] points;
        public float waitAtPoint;
        public bool startAtNearest;
        public Vector3 homePosition;
    }

    public void ApplyPatrolConfig(in PatrolConfig c)
    {
        patrolMode = c.mode;
        patrolRadius = c.radius;
        patrolPoints = c.points;
        waitAtPoint = c.waitAtPoint;
        startAtNearest = c.startAtNearest;
        HomePosition = c.homePosition;
    }
    #endregion

    #region Getter
    public EnemyBehaviorData BehaviorData => behaviorData;
    public EnemyStateMachine Fsm => fsm;
    public EnemyType Type => behaviorData.enemyType;
    public FinalComboController FinalComboController => finalComboController;
    public int MaxHP => behaviorData.maxHP;
    public int Damage => behaviorData.damage;
    public float MoveSpeed => behaviorData.moveSpeed;
    public float DetectionRange => behaviorData.detectionRange;
    public float AttackRange => behaviorData.attackRange;
    public float AttackCooldown => behaviorData.attackCooldown;
    public float AttackSpeed => behaviorData.attackSpeed;
    #endregion

    protected virtual void Awake()
    {
        currentHP = behaviorData.maxHP;
        fsm = GetComponent<EnemyStateMachine>();
        timeController = GetComponent<EnemyTimeController>();
        finalComboController = GetComponent<FinalComboController>();

        col = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        SetupHPUI();
    }
    
    private void OnEnable()
    {
        EnemyManager.Instance?.RegisterEnemy(this);
    }

    private void OnDisable()
    {
        EnemyManager.Instance?.UnregisterEnemy(this);
    }

    public virtual void ResetState()
    {
        // 코루틴 누수 방지
        StopAllCoroutines();

        // FSM/Agent/Animator 기본 복구
        if (fsm != null)
        {
            fsm.enabled = true;
            fsm.ResetToIdle();
        }

        if (agent != null)
        {
            if (!agent.enabled) agent.enabled = true;
            agent.isStopped = false;
            agent.ResetPath();
            agent.speed = MoveSpeed;

            // 우선순위
            agent.avoidancePriority = UnityEngine.Random.Range(40, 61);
        }

        if (animator != null)
        {
            // 스턴/트리거 잔상 제거
            animator.Rebind();
            animator.Update(0f);
            animator.SetBool("IsStunned", false);
        }

        hasDetectedPlayer = false;
        LastSeenPlayerTime = float.NegativeInfinity;
        deathEventSent = false;
        despawnEventSent = false;

        currentHP = behaviorData.maxHP;

        if (timeController != null) timeController.SetSpeed(MoveSpeed);

        if (hpUI != null)
        {
            hpUI.SetHP(currentHP, MaxHP);
            hpUI.gameObject.SetActive(true);
        }

        if (col != null) col.enabled = true;
    }

    #region HP 접근 유틸
    public float GetHpRatio()
    {
        int max = Mathf.Max(1, MaxHP);
        return Mathf.Clamp01((float)currentHP / max);
    }

    public void SetHpRatio(float ratio, bool syncUi = true)
    {
        int max = Mathf.Max(1, MaxHP);
        currentHP = Mathf.Clamp(Mathf.RoundToInt(ratio * max), 0, max);

        if (syncUi && hpUI != null)
        {
            hpUI.SetHP(currentHP, max);
        }
    }
    #endregion

    public bool CanSeePlayer()
    {
        // if (hasDetectedPlayer) return false;
        if (fsm.Target == null) return false;

        Vector3 toTarget = (fsm.Target.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, fsm.Target.position);

        if (distance > BehaviorData.detectionRange) return false;

        // 시야각 판정
        float angle = Vector3.Angle(transform.forward, toTarget);
        if (angle > BehaviorData.detectionAngle / 2f) return false;

        // 가림 판정
        if (Physics.Raycast(transform.position + Vector3.up, toTarget, distance, obstacleLayer))
            return false;

        LastSeenPlayerTime = Time.time;

        return true;
    }

    // 공격 용 LOS: 탐지 여부/시야각/탐지거리와 무관하게 장애물 가림만 검사
    public bool HasClearShotToTarget()
    {
        if (fsm == null || fsm.Target == null) return false;

        Vector3 toTarget = (fsm.Target.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, fsm.Target.position);

        // 장애물에 가려져 있으면 명확한 시야가 아님
        if (Physics.Raycast(transform.position + Vector3.up, toTarget, distance, obstacleLayer))
            return false;

        return true;
    }

    public void ResetDetection()
    {
        hasDetectedPlayer = false;
    }

    public void DetectPlayer()
    {
        if(hasDetectedPlayer) return;
        hasDetectedPlayer = true;

        LastSeenPlayerTime = Time.time;

        if (GameManager.Instance.CurrentGameState is ExplorationState or PuzzleState)
        {
            TriggerCombatStarted();
        }

        // 즉시 추적 상태로 전환
        fsm?.TransitionToState(fsm.ChaseState);
    }

    // 데미지 처리
    public virtual void TakeDamage(int damage)
    {
        DetectPlayer();

        currentHP -= damage;
        Debug.Log($"Enemy HP: {currentHP}");

        hpUI.SetHP(currentHP, MaxHP);

        // 플레이어가 적을 공격하면 전투 시작
        if (GameManager.Instance.CurrentGameState is ExplorationState || GameManager.Instance.CurrentGameState is PuzzleState)
        {
            TriggerCombatStarted();
        }

        if(fsm != null)
        {
            if(currentHP <= 0)
            {
                fsm.TransitionToState(fsm.DeadState);
            }
            else
            {
                fsm.TransitionToState(fsm.HitState);
            }
        }
    }

    // 넉백 처리
    public virtual void ApplyKnockback(float force)
    {
        // 에너미의 로컬 기준으로 뒤쪽 방향 계산 (transform.forward의 반대 방향)
        Vector3 localBackDirection = -transform.forward;
        
        // 약간 위로 올리는 효과 추가
        localBackDirection.y = 0.3f;
        localBackDirection = localBackDirection.normalized;
        
        Vector3 knockbackPosition = transform.position + localBackDirection * force;
        
        Debug.Log($"[넉백] 원래 위치: {transform.position}, 로컬 뒤쪽 방향: {localBackDirection}, 힘: {force}, 목표 위치: {knockbackPosition}");

        // NavMesh 검색 반경을 force의 절반으로 설정 (더 넓은 범위에서 검색)
        float searchRadius = Mathf.Max(force * 0.5f, 100f);
        
        if(NavMesh.SamplePosition(knockbackPosition, out NavMeshHit hit, searchRadius, NavMesh.AllAreas))
        {
            // 코루틴을 사용하여 부드러운 이동
            StartCoroutine(SmoothKnockback(hit.position, 0.3f));
            Debug.Log($"[넉백] 부드러운 이동 시작 - 목표 위치: {hit.position}");
        }
        else
        {
            Debug.LogWarning($"[넉백] 실패! NavMesh에서 유효한 위치를 찾을 수 없음. 검색 반경: {searchRadius}");
        }
    }

    // 부드러운 넉백 이동 코루틴
    private IEnumerator SmoothKnockback(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = transform.position;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += GetAdjustedDeltaTime();
            float t = elapsed / duration;
            
            // 선형 이동
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, t);
            
            // NavMesh 위에 있는지 확인하고 이동
            if (NavMesh.SamplePosition(newPosition, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            {
                transform.position = hit.position;
            }
            
            yield return null;
        }
        
        // 최종 위치 보정
        if (NavMesh.SamplePosition(targetPosition, out NavMeshHit finalHit, 1f, NavMesh.AllAreas))
        {
            transform.position = finalHit.position;
        }
        
        // Debug.Log($"[넉백] 부드러운 이동 완료 - 최종 위치: {transform.position}");
    }

    // 사망 처리
    public virtual void Die()
    {
        Debug.Log("Enemy Die");

        if (!deathEventSent)
        {
            deathEventSent = true;
            OnDied?.Invoke(this);

            int reward = behaviorData.rewardMoney;
            if (reward > 0)
            {
                PlayerManager.Instance?.AddGold(reward);
                UIManager.Instance.ShowToast($"+ {reward} G (보유: {PlayerManager.Instance?.Gold} G)");
            }
        }

        
        if(col != null) col.enabled = false;
        if (fsm != null) fsm.enabled = false;
        if (agent != null) agent.isStopped = true;
        
        // enabled = false;
        
        // 시간 조절을 반영한 해제 지연
        StartCoroutine(ReleaseWithTimeScale(destroyTime));
    }

    // 파괴 지연 처리
    private IEnumerator ReleaseWithTimeScale(float delay)
    {
        float elapsed = 0f;
        while (elapsed < delay)
        {
            elapsed += GetAdjustedDeltaTime();
            yield return null;
        }

        if (!despawnEventSent)
        {
            despawnEventSent = true;
            OnDespawned?.Invoke(this);
        }

        EnemyManager.Instance?.ReleaseEnemy(this);
    }

    // 시간 조절 처리
    public float GetAdjustedDeltaTime()
    {
        return timeController != null ? timeController.GetAdjustedDeltaTime() : Time.deltaTime;
    }

    // 공격 처리
    // 애니메이션 이벤트 등록해서 이용
    public void PerformAttack()
    {
        OnPerformAttack();
    }

    // 각 적 타입별로 구현할 추상 메서드
    protected abstract void OnPerformAttack();

    // 근접 공격 판정 (공통 기능)
    protected void DealDamagedWithCapsule(Transform startPosition, Transform endPosition, float radius)
    {
        // Debug.Log("근접 공격 판정");
        Collider[] hits = Physics.OverlapCapsule(startPosition.position, endPosition.position, radius, playerLayer);

        foreach(Collider hit in hits)
        {
            if(hit.TryGetComponent(out IDamageable damageable))
            {
                if(damageable is PlayerManager player)
                {
                    float attackTime = Time.time;
                    if(player.TryParry(attackTime))
                    {
                        OnParried();
                        continue;
                    }
                }

                damageable.TakeDamage(Damage);
                // Debug.Log($"에너미 {transform.name} 공격: {damageable.GetType().Name}이 {Damage} 입음");
            }
        }
    }

    // 근접 구체 공격 판정
    protected void DealDamagedWithSphere(Transform center, float radius)
    {
        Debug.Log("근접 구체 공격 판정");
        Collider[] hits = Physics.OverlapSphere(center.position, radius, playerLayer);

        foreach(Collider hit in hits)
        {
            if(hit.TryGetComponent(out IDamageable damageable))
            {
                if(damageable is PlayerManager player)
                {
                    float attackTime = Time.time;
                    if(player.TryParry(attackTime))
                    {
                        OnParried();
                        continue;
                    }
                }

                damageable.TakeDamage(Damage);
                Debug.Log($"에너미 {transform.name} 공격: {damageable.GetType().Name}이 {Damage} 입음");
            }
        }
    }

    private void OnParried()
    {
        fsm.enabled = false;
        agent.isStopped = true;
        animator.SetBool("IsStunned", true);

        ApplyKnockback(stunKnockbackForce);

        StartCoroutine(RecoverFromStun(stunDuration));
    }

    private IEnumerator RecoverFromStun(float duration)
    {
        yield return new WaitForSeconds(duration);

        fsm.enabled = true;
        agent.isStopped = false;
        animator.SetBool("IsStunned", false);
    }

    private void SetupHPUI()
    {
        if (hpUIPrefab == null)
        {
            hpUIPrefab = Resources.Load<GameObject>("UI/EnemyHPUI");
        }

        if (hpUIPrefab == null)
        {
            Debug.LogError("[Enemy] EnemyHPUI 프리팹 로드 실패: Resources/UI/EnemyHPUI");
            return;
        }

        Transform followTarget = transform;

        if (TryGetComponent(out Animator animator) && animator.isHuman && animator.avatar != null)
        {
            // 휴머노이드면 Head 본 사용
            var head = animator.GetBoneTransform(HumanBodyBones.Head);
            if (head != null)
                followTarget = head;
        }
        else
        {
            // 휴머노이드가 아닐 경우 이름에 "head"가 포함된 자식 찾기
            Transform found = TransformUtils.FindChildRecursive(transform, t => t.name.ToLower().Contains("head"));
            if (found != null)
            {
                followTarget = found;
            }
            else
            {
                Debug.LogWarning($"Enemy {name}: 'Head' 트랜스폼을 찾을 수 없어 기본 transform 사용");
            }
        }

        GameObject uiObj = Instantiate(hpUIPrefab, transform);
        hpUI = uiObj.GetComponent<EnemyHPUI>();
        hpUI.SetFollowTarget(followTarget);
        hpUI.SetOffset(hpUIOffset);
        hpUI.SetHP(currentHP, MaxHP);
    }

    #if UNITY_EDITOR
    protected virtual void OnDrawGizmosSelected()
    {
        if (fsm?.Target == null) return;

        // 시야 범위 색상
        Gizmos.color = Color.yellow;

        // 시야 거리 원
        Gizmos.DrawWireSphere(transform.position, BehaviorData.detectionRange);

        // 시야각 시각화 (부채꼴 방향 표시)
        Vector3 forward = transform.forward;
        float halfAngle = BehaviorData.detectionAngle * 0.5f;

        // 부채꼴 양쪽 라인
        Quaternion leftRot = Quaternion.Euler(0, -halfAngle, 0);
        Quaternion rightRot = Quaternion.Euler(0, halfAngle, 0);

        Vector3 leftDir = leftRot * forward;
        Vector3 rightDir = rightRot * forward;

        Gizmos.color = new Color(1f, 0.6f, 0f, 0.6f);
        Gizmos.DrawRay(transform.position + Vector3.up * 1f, leftDir * BehaviorData.detectionRange);
        Gizmos.DrawRay(transform.position + Vector3.up * 1f, rightDir * BehaviorData.detectionRange);
    }
    #endif
}
