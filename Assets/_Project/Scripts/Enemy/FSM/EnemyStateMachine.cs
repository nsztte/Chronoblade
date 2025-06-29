using UnityEngine;
using UnityEngine.AI;

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

        currentStateType = newState switch
        {
            EnemyIdleState => EnemyState.Idle,
            EnemyChaseState => EnemyState.Chase,
            EnemyAttackState => EnemyState.Attack,
            EnemyHitState => EnemyState.Hit,
            EnemyDeadState => EnemyState.Dead,
            _ => currentStateType
        };
    }
}
