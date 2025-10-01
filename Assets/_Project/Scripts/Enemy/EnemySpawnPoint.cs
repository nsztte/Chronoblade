using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private bool spawnOnStart = true;

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

        Enemy enemy = EnemyManager.Instance.SpawnEnemy(enemyType, transform.position, transform.rotation);
        if (enemy == null)
        {
            Debug.LogWarning($"[SpawnPoint] EnemyType {enemyType} 스폰 실패");
        }
    }

}
