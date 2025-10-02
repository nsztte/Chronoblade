using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyTimeController))]
public class FakeClone : MonoBehaviour, IDamageable
{
    [Header("폭발 설정")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float turnSpeed = 540f;
    [SerializeField] private float triggerRadius = 2f;
    [SerializeField] private float explodeDelay = 0.4f;
    [SerializeField] private float explodeRadius = 2.5f;
    [SerializeField] private int explodeDamage = 20;
    [SerializeField] private LayerMask playerMask;

    private MirrorDuelist enemy;
    private float spawnTime;
    private bool isHit = false;
    private bool isReleased = false;

    private bool isWindUp = false;
    private float windUpTimer = 0f;

    private NavMeshAgent agent;
    private Animator animator;
    private EnemyTimeController timeController;
    
    public void Initialize(MirrorDuelist enemy)
    {
        this.enemy = enemy;
        spawnTime = Time.time;
        isHit = false;
        isReleased = false;
        isWindUp = false;
        windUpTimer = 0f;

        if (animator == null)
            animator = GetComponent<Animator>();

        if (timeController == null)
            timeController = GetComponent<EnemyTimeController>();

        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        agent.isStopped = false;
        agent.updateRotation = false;
        agent.speed = moveSpeed;

        timeController.SetSpeed(moveSpeed);
    }

    private void Update()
    {
        if (isReleased || isHit) return;

        float dt = timeController != null ? timeController.GetAdjustedDeltaTime() : Time.deltaTime;

        // 수명 끝남
        if(Time.time - spawnTime > enemy.CloneLifetime)
        {
            ReleaseClone();
            return;
        }

        var target = enemy?.Fsm?.Target;
        if (target == null)
        {
            Explode(); // 타겟이 없어도 자폭
            return;
        }

        if (!isWindUp)
        {
            // 회전만 수동
            Vector3 toTarget = target.position - transform.position;
            Vector3 flatDir = new Vector3(toTarget.x, 0f, toTarget.z).normalized;
            if (flatDir.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(flatDir);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, turnSpeed * dt);
            }

            // 목적지 설정
            agent.SetDestination(target.position);

            animator.SetBool("IsRunning", true);

            if (toTarget.sqrMagnitude <= triggerRadius * triggerRadius)
            {
                StartWindUp();
            }
        }
        else
        {
            windUpTimer -= dt;
            if (windUpTimer <= 0f)
            {
                Explode();
            }
        }
    }

    public void TakeDamage(int damage)
    {
        isHit = true;
        Explode();
    }

    public void TriggerChainExplosion()
    {
        if (isReleased) return;
        isHit = true;
        Explode();
    }

    private void StartWindUp()
    {
        isWindUp = true;
        windUpTimer = explodeDelay;

        if (agent != null)
            agent.isStopped = true;

        animator.SetBool("IsRunning", false);

        // TODO: 예열 애니메이션/이펙트/사운드
        Debug.Log("FakeClone 예열 시작");
    }

    private void Explode()
    {
        if (isReleased) return;

        Debug.Log("FakeClone 폭발");

        // 플레이어 피해
        Collider[] hits = Physics.OverlapSphere(transform.position, explodeRadius, playerMask);
        foreach (var col in hits)
        {
            if (col.TryGetComponent(out IDamageable dmg))
            {
                dmg.TakeDamage(explodeDamage);
            }
        }

        // 연쇄 폭발 대상 탐색
        Collider[] allHits = Physics.OverlapSphere(transform.position, explodeRadius);
        foreach (var col in allHits)
        {
            if (col.TryGetComponent(out FakeClone other) && other != this)
            {
                if (!other.isReleased && !other.isHit)
                {
                    Debug.Log("FakeClone 연쇄 폭발 유도");
                    other.TriggerChainExplosion(); // 내부적으로 Explode()
                }
            }
        }

        // TODO: 폭발 애니메이션/이펙트/사운드
        ReleaseClone();
    }

    private void ReleaseClone()
    {
        if (isReleased) return;
        isReleased = true;

        if (agent != null)
        {
            agent.ResetPath();
            agent.isStopped = true;
        }
        
        //TODO: 클론 파괴 효과 추가
        enemy.UnregisterClone(this);
        FakeClonePool.Instance?.Release(this);
    }

    public void ApplyKnockback(float force) {}
}
