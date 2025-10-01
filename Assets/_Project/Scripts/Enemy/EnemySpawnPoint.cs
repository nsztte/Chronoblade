using UnityEngine;
using UnityEngine.AI;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private bool spawnOnStart = true;

    [SerializeField] private float spawnRadius = 5f;
    [SerializeField] private int minSpawnCount = 3;
    [SerializeField] private int maxSpawnCount = 5;

    private void Start()
    {
        if (spawnOnStart)
        {
            TrySpawnEnemy();
        }
    }

    public void TrySpawnEnemy()
    {
        if (EnemyManager.Instance == null)
        {
            Debug.LogWarning($"[SpawnPoint] EnemyManager 인스턴스 없음");
            return;
        }

        int spawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1);

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
            randomOffset.y = 0f;

            Vector3 spawnPos = transform.position + randomOffset;

            // NavMesh 유효성 체크
            if (NavMesh.SamplePosition(spawnPos, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                Enemy enemy = EnemyManager.Instance.SpawnEnemy(enemyType, hit.position, Quaternion.identity);
                if (enemy == null)
                {
                    Debug.LogWarning($"[SpawnPoint] EnemyType {enemyType} 스폰 실패");
                }
            }
            else
            {
                Debug.LogWarning($"[SpawnPoint] 위치 무효: {spawnPos}");
            }
        }
    }

    #if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.2f, 0.2f, 0.3f);
        Gizmos.DrawSphere(transform.position, spawnRadius);
    }
    #endif
}
