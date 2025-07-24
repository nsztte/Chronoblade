using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnergyBoltProjectile : MonoBehaviour
{
    [Header("설정")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float delay = 0.5f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float paralysisDuration = 1f;
    [SerializeField] private LayerMask targetLayer;

    [Header("추적")]
    [SerializeField] private bool isHoming = false;
    [SerializeField] private float homingEndDistance = 1.5f;
    private Transform target;

    [Header("이펙트")]
    [SerializeField] private GameObject electricEffectPrefab;


    private Vector3 direction;
    private float timeElapsed = 0f;
    private Collider col;

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    public void SetTarget(Transform target)
    {
        isHoming = true;
        this.target = target;
    }

    private void Start()
    {
        col = GetComponent<Collider>();
        col.isTrigger = true;

        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        if(isHoming && target != null)
        {
            float distance = Vector3.Distance(transform.position, target.position);

            if(distance <= homingEndDistance)
            {
                isHoming = false;
            }
            else
            {
                Vector3 targetPos = target.position;
                Collider targetCollider = target.GetComponent<Collider>();
                targetPos.y = targetCollider.bounds.center.y;
                direction = (targetPos - transform.position).normalized;
            }
        }

        if(direction != Vector3.zero && timeElapsed >= delay)
        {
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(((1 << other.gameObject.layer) & targetLayer) != 0)
        {
            if(other.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);

                if(other.TryGetComponent(out IStatusEffectable statusEffectable))
                {
                    statusEffectable.ApplyStatus(StatusEffectType.Paralysis, paralysisDuration);
                }

                Debug.Log($"에너지볼트 적중: {other.gameObject.name}에게 {damage} + 마비 {paralysisDuration}초 효과");
            }

            Explode();
        }
    }

    private void Explode()
    {
        if(electricEffectPrefab != null)
        {
            GameObject effect = Instantiate(electricEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 0.5f);
        }

        Destroy(gameObject);
        Debug.Log("에너지볼트 폭발");
    }
}
