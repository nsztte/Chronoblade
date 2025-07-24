using UnityEngine;

public class Mine : MonoBehaviour
{
    [Header("설정")]
    [SerializeField] private float minDelay = 1.5f;
    [SerializeField] private float maxDelay = 3f;
    [SerializeField] private float warningTime = 0.3f;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private int damage = 12;
    [SerializeField] private LayerMask targetLayer;

    private bool hasExploded = false;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        float delay = Random.Range(minDelay, maxDelay);
        Invoke(nameof(StartWarning), delay - warningTime);
        Invoke(nameof(Explode), delay);
    }

    private void StartWarning()
    {
        animator.SetTrigger("Warning");
    }

    private void Explode()
    {
        if(hasExploded) return;
        hasExploded = true;

        animator.SetTrigger("Explode");

        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, targetLayer);
        foreach(Collider hit in hits)
        {
            if(hit.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
                Debug.Log($"지뢰 폭발: {hit.name}에게 {damage}의 데미지 적용");
            }
        }

        Destroy(gameObject, 0.5f);  // 애니메이션 클립 길이만큼으로 수정
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
