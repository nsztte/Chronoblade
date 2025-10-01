using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    #region Singleton
    public static EnemyManager Instance { get; private set; }

    public void Initialize()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning($"{GetType().Name} 인스턴스 감지됨, 초기화 스킵");
            return;
        }
        Instance = this;
    }
    #endregion
    
    [SerializeField] private List<EnemyPool> enemyPools;
    private Dictionary<EnemyType, EnemyPool> poolMap = new();

    private List<Enemy> activeEnemies = new List<Enemy>();

    private void Awake()
    {
        foreach (var pool in enemyPools)
        {
            if (pool == null || pool.Prefab == null) continue;

            Enemy enemy = pool.Prefab.GetComponent<Enemy>();
            if (enemy == null)
            {
                Debug.LogWarning($"[EnemyManager] EnemyPool의 프리팹에 Enemy 컴포넌트가 없음");
                continue;
            }

            EnemyType type = enemy.BehaviorData.enemyType;
            if (poolMap.ContainsKey(type))
            {
                Debug.LogWarning($"[EnemyManager] EnemyType {type} 중복 등록 시도");
                continue;
            }

            poolMap[type] = pool;
        }
    }

    public void RegisterEnemy(Enemy enemy)
    {
        if(!activeEnemies.Contains(enemy))
        {
            activeEnemies.Add(enemy);
        }
    }
    
    public void UnregisterEnemy(Enemy enemy)
    {
        if(activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }

        if(activeEnemies.Count == 0 && GameManager.Instance.CurrentGameState is CombatState)
        {
            GameManager.Instance.EnterExploration();
        }
    }

    public Enemy SpawnEnemy(EnemyType type, Vector3 position, Quaternion rotation)
    {
        if (!poolMap.TryGetValue(type, out var pool))
        {
            Debug.LogError($"[EnemyManager] EnemyType {type}에 대한 풀을 찾을 수 없음");
            return null;
        }

        Enemy enemy = pool.Get();
        if (enemy == null)
        {
            Debug.LogWarning($"[EnemyManager] EnemyType {type} 풀에서 Get 실패");
            return null;
        }

        enemy.transform.SetPositionAndRotation(position, rotation);
        enemy.enabled = true;
        enemy.ResetState();
        
        return enemy;
    }

    public void ReleaseEnemy(Enemy enemy)
    {
        if (!poolMap.TryGetValue(enemy.Type, out var pool))
        {
            Debug.LogWarning($"[EnemyManager] EnemyType {enemy.Type}에 대한 풀을 찾을 수 없음");
            return;
        }

        pool.Release(enemy);
    }
}
