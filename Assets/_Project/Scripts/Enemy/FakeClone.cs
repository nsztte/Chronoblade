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

    [Header("VFX")]
    [SerializeField] private Renderer coreRenderer;  // 코어 메쉬 Renderer
    [SerializeField] private Color emissionColor = new Color(0.2f, 3.0f, 2.5f);
    [SerializeField] private float blink = 10f;
    [SerializeField] private float emissionIntensity = 1.5f;

    private MirrorDuelist enemy;
    private float spawnTime;
    private bool isHit = false;
    private bool isReleased = false;

    private bool isWindUp = false;
    private float windUpTimer = 0f;

    private NavMeshAgent agent;
    private Animator animator;
    private EnemyTimeController timeController;

    private MaterialPropertyBlock mpb;
    private Coroutine blinkCo;
    private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");
    
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

        // NavMeshAgent 위치를 현재 transform 위치로 강제 고정 (땅에 쳐박히는 문제 방지)
        // Warp를 사용하여 NavMeshAgent가 위치를 자동 조정하지 않도록 함
        if (agent != null)
        {
            // agent가 활성화되어 있어야 Warp가 작동함
            if (!agent.enabled)
                agent.enabled = true;
                
            // 현재 위치가 NavMesh 위에 있는지 확인하고 Warp
            if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                transform.position = hit.position;
                agent.Warp(hit.position);
            }
            
            agent.isStopped = false;
            agent.updateRotation = false;
            agent.speed = moveSpeed;
        }

        timeController.SetSpeed(moveSpeed);
        StopEmissionBlink();
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

        StartEmissionBlink();

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
        StopEmissionBlink();
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
    
    #region VFX
    private void StartEmissionBlink()
    {
        if (coreRenderer == null) return;

        mpb ??= new MaterialPropertyBlock();

        StopEmissionBlink();
        blinkCo = StartCoroutine(EmissionBlinkRoutine());
    }

    private void StopEmissionBlink()
    {
        if (blinkCo != null)
        {
            StopCoroutine(blinkCo);
            blinkCo = null;
        }

        // 꺼진 상태로 리셋(풀링 때문에 중요)
        SetEmission(0f);
    }

    private System.Collections.IEnumerator EmissionBlinkRoutine()
    {
        while (true)
        {
            // 너는 EnemyTimeController로 dt를 조절하고 있으니까 그거 따라가는 게 자연스러움
            float dt = timeController != null ? timeController.GetAdjustedDeltaTime() : Time.deltaTime;

            float t = Time.time * blink;
            float on = (t - Mathf.Floor(t)) < 0.5f ? 1f : 0f;

            SetEmission(on * emissionIntensity);

            yield return null;
        }
    }

    private void SetEmission(float intensity)
    {
        if (coreRenderer == null) return;

        coreRenderer.GetPropertyBlock(mpb);

        // HDR 컬러 * intensity
        Color c = emissionColor * intensity;
        mpb.SetColor(EmissionColorId, c);

        coreRenderer.SetPropertyBlock(mpb);
    }
    #endregion

    public void ApplyKnockback(float force) {}
}
