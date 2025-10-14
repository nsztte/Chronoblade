using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemySpawnPoint : MonoBehaviour
{
    [Header("타입, 초기 설정")]
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private bool spawnOnStart = true;

    [Header("스폰 설정")]
    [SerializeField] private float spawnRadius = 5f;
    [SerializeField] private int minSpawnCount = 3;
    [SerializeField] private int maxSpawnCount = 5;
    [SerializeField] private float sampleMaxDistance  = 2f;

    [Header("리스폰 설정")]
    [SerializeField] private bool allowRespawn = true;
    [SerializeField] private float respawnCooldown = 10f;
    [SerializeField] private int maxAlive = 6;              // 동시에 존재 가능한 최대치
    [SerializeField] private int maxPerCount = 2;           // 한 번의 쿨다운 후 최대 스폰 수
    [SerializeField] private float minPlayerDistance = 12f; // 플레이어와의 최소 거리

    private readonly List<Enemy> active = new List<Enemy>();
    private float nextRespawnAt;
    private Transform player;

    private void Start()
    {
        player = PlayerManager.Instance.PlayerTransform;

        if (spawnOnStart)
        {
            TrySpawnEnemies(Random.Range(minSpawnCount, maxSpawnCount + 1));
        }
    }

    private void Update()
    {
        if (!allowRespawn) return;
        if (Time.time < nextRespawnAt) return;
        if (player && Vector3.Distance(player.position, transform.position) < minPlayerDistance) return;

        int need = Mathf.Clamp(maxAlive - active.Count, 0, maxPerCount);
        if (need > 0)
        {
            TrySpawnEnemies(need);
        }

        ScheduleNextRespawn();
    }

    private void ScheduleNextRespawn()
    {
        nextRespawnAt = Time.time + respawnCooldown;
    }


    public void TrySpawnEnemies(int spawnCount)
    {
        if (EnemyManager.Instance == null)
        {
            Debug.LogWarning($"[SpawnPoint] EnemyManager 인스턴스 없음");
            return;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            if(active.Count >= maxAlive) break;

            if (TryPickSpawnPosition(out Vector3 pos))
            {
                var enemy = EnemyManager.Instance.SpawnEnemy(enemyType, pos, transform.rotation);
                if (enemy == null)
                {
                    Debug.LogWarning($"[SpawnPoint] EnemyType {enemyType} 스폰 실패");
                    continue;
                }

                RegisterEnemy(enemy);
            }
            else
            {
                Debug.LogWarning($"[SpawnPoint] NavMesh 샘플 실패 (반경:{spawnRadius})");
            }
        }
    }

    private void RegisterEnemy(Enemy enemy)
    {
         if (enemy == null) return;

        // 중복 등록 방지
        if (!active.Contains(enemy))
            active.Add(enemy);

        // 사망/풀 반환 이벤트 구독
        enemy.OnDied += HandleEnemyGone;
        enemy.OnDespawned += HandleEnemyGone;
    }

    private void HandleEnemyGone(Enemy e)
    {
        if (e == null) return;

        e.OnDied -= HandleEnemyGone;
        e.OnDespawned -= HandleEnemyGone;

        active.Remove(e);
    }

    private bool TryPickSpawnPosition(out Vector3 position)
    {
        // 랜덤 원 안에서 시도 3회
        for (int t = 0; t < 3; t++)
        {
            Vector3 random = Random.insideUnitSphere * spawnRadius; random.y = 0f;
            Vector3 raw = transform.position + random;

            if (NavMesh.SamplePosition(raw, out NavMeshHit hit, sampleMaxDistance , NavMesh.AllAreas))
            {
                position = hit.position;
                return true;
            }
        }

        // 폴백: 중심점을 그대로 샘플
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit centerHit, sampleMaxDistance , NavMesh.AllAreas))
        {
            position = centerHit.position;
            return true;
        }

        position = default;
        return false;
    }

    #if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.2f, 0.2f, 0.3f);
        Gizmos.DrawSphere(transform.position, spawnRadius);
    }
    #endif
}
