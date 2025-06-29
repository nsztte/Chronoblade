using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public enum EnemyState
{
    Idle,
    Chase,
    Attack,
    Hit,
    Dead
}

[RequireComponent(typeof(Animator), typeof(NavMeshAgent), typeof(Enemy))]
public class EnemyStateMachine : MonoBehaviour
{
    // 참조 변수
    private Animator animator;
    private NavMeshAgent agent;
    private Enemy enemy;
    [SerializeField] private Transform target;

    // 상태 트래킹
    public EnemyState currentStateType;
    private EnemyBaseState currentState;

    // 상태 인스턴스
    private readonly EnemyIdleState idleState = new EnemyIdleState();
    private readonly EnemyChaseState chaseState = new EnemyChaseState();
    private EnemyAttackState attackState;
    private readonly EnemyHitState hitState = new EnemyHitState();
    private readonly EnemyDeadState deadState = new EnemyDeadState();

    // 상태 타입 매핑 Dictionary
    private readonly Dictionary<System.Type, EnemyState> stateTypeMap = new Dictionary<System.Type, EnemyState>();

    // 상태 인스턴스 접근자
    public EnemyIdleState IdleState => idleState;
    public EnemyChaseState ChaseState => chaseState;
    public EnemyAttackState AttackState => attackState;
    public EnemyHitState HitState => hitState;
    public EnemyDeadState DeadState => deadState;

    // 참조 인스턴스 접근자
    public Animator Animator => animator;
    public NavMeshAgent Agent => agent;
    public Enemy Enemy => enemy;
    public Transform Target => target;
    public EnemyBaseState CurrentState => currentState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<Enemy>();

        // 동적으로 공격 상태 생성
        attackState = enemy.BehaviorData.CreateAttackState();
        
        // 상태 타입 매핑 초기화
        InitializeStateTypeMap();
    }

    private void InitializeStateTypeMap()
    {
        stateTypeMap[typeof(EnemyIdleState)] = EnemyState.Idle;
        stateTypeMap[typeof(EnemyChaseState)] = EnemyState.Chase;
        stateTypeMap[typeof(EnemyAttackState)] = EnemyState.Attack;
        stateTypeMap[typeof(EnemyHitState)] = EnemyState.Hit;
        stateTypeMap[typeof(EnemyDeadState)] = EnemyState.Dead;
        
        // 파생된 공격 상태들도 매핑
        stateTypeMap[typeof(ChronoAttackState)] = EnemyState.Attack;
        stateTypeMap[typeof(MirrorAttackState)] = EnemyState.Attack;
    }

    private void Start()
    {
        TransitionToState(idleState);
    }

    private void Update()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);
        currentState?.Update(this);
    }

    public void TransitionToState(EnemyBaseState newState)
    {
        currentState?.Exit(this);
        currentState = newState;
        currentState.Enter(this);

        // Dictionary를 사용한 상태 타입 매핑
        currentStateType = stateTypeMap.GetValueOrDefault(newState.GetType(), currentStateType);
    }
}
