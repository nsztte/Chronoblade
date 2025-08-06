using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnRange = 5f;
    [SerializeField] private int spawnCount = 5;
    [SerializeField] private float spawnCooldown = 10f;
    private float lastSpawnTime = -Mathf.Infinity;

    public void Spawn()
    {
        if (Time.time - lastSpawnTime < spawnCooldown || enemyPrefab == null || spawnPoint == null)
            return;

        Debug.Log("와쳐 소환");

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-spawnRange, spawnRange),
                0f,
                Random.Range(-spawnRange, spawnRange)
            );

            Vector3 spawnPos = spawnPoint.position + randomOffset;

            Instantiate(enemyPrefab, spawnPos, spawnPoint.rotation);
        }

        lastSpawnTime = Time.time;
    }
}
