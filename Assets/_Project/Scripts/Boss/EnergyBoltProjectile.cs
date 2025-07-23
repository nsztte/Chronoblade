using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnergyBoltProjectile : MonoBehaviour
{
    [Header("설정")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float delay = 0.5f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private int damage = 20;
    [SerializeField] private float paralysisDuration = 1f;
    [SerializeField] private LayerMask targetLayer;


    [Header("이펙트")]
    [SerializeField] private GameObject electricEffectPrefab;

    private Vector3 direction;
    private float timeElapsed = 0f;
    private Collider col;

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
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
    }
}
