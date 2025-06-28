using UnityEngine;

public class ChronoProjectile : MonoBehaviour
{
    [Header("발사체 설정")]
    [SerializeField] private float speed = 15f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private int damage = 8;
    [SerializeField] private float slowDuration = 3f;
    
    private Vector3 direction;
    private float elapsedTime;
    private EnemyTimeController timeController;
    private Rigidbody rb;
    
    private void Awake()
    {
        timeController = FindFirstObjectByType<EnemyTimeController>();
    }
    
    public void Initialize(Vector3 targetDirection, int projectileDamage, float slowEffectDuration)
    {
        direction = targetDirection.normalized;
        damage = projectileDamage;
        slowDuration = slowEffectDuration;
        elapsedTime = 0f;
        
    //     // 발사체가 플레이어를 향해 정확히 바라보도록 회전
    //     if (direction != Vector3.zero)
    //     {
    //         transform.rotation = Quaternion.LookRotation(direction);
    //     }
    }
    
    private void Update()
    {
        float deltaTime = GetAdjustedDeltaTime();
        elapsedTime += deltaTime;
        
        if (elapsedTime >= lifetime)
        {
            Destroy(gameObject);
            return;
        }
        
        // 발사체를 직선으로 이동 (중력 영향 없음)
        transform.position += direction * speed * deltaTime;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // 플레이어 레이어와 충돌했는지 확인
        if (other.CompareTag("Player"))
        {
            Debug.Log("ChronoProjectile hit");
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
                Debug.Log($"ChronoProjectile hit {damageable.GetType().Name} for {damage} damage");
                
                // TODO: 플레이어에게 슬로우 디버프 적용
                // PlayerTimeController playerTimeController = other.GetComponent<PlayerTimeController>();
                // if (playerTimeController != null)
                // {
                //     playerTimeController.ApplySlowEffect(slowDuration);
                // }
            }
            
            // 충돌 후 발사체 파괴
            Destroy(gameObject);
        }
    }
    
    private float GetAdjustedDeltaTime()
    {
        return timeController != null ? timeController.GetAdjustedDeltaTime() : Time.deltaTime;
    }
} 