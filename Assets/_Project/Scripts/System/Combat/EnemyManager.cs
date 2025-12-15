using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private bool bossCombatActive; // 보스전 중이면 전투 종료 판정을 잠금

    [Header("전투 종료 판정 설정")]
    [SerializeField] private float combatRecentSeenTime = 4f;          // 최근에 플레이어를 본 시간 기준
    [SerializeField] private float combatMaxHorizontalDistance = 25f;  // 전투로 취급할 최대 가로 거리(m)
    [SerializeField] private float combatExitCheckInterval = 0.25f;    // 전투 종료 체크 주기
    [SerializeField] private float combatNoEnemyDuration = 3f;         // "전투 적 없음" 상태가 지속되어야 하는 시간

    private float combatExitCheckTimer;
    private float combatNoEnemyTimer;

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

    private void OnEnable()
    {
        SceneManager.sceneUnloaded += HandleSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= HandleSceneUnloaded;
    }

    private void Update()
    {
        var gm = GameManager.Instance;
        var player = PlayerManager.Instance;

        if (gm == null || player == null) return;

        // 컴뱃 상태일 때만 전투 종료 판정
        if (gm.CurrentGameState is CombatState)
        {
            combatExitCheckTimer += Time.deltaTime;
            if (combatExitCheckTimer < combatExitCheckInterval)
                return;

            combatExitCheckTimer = 0f;

            bool hasRelevant = bossCombatActive || HasRelevantCombatEnemy(player.transform.position);

            if (hasRelevant)
            {
                // 아직 전투적으로 의미 있는 적이 있으면 타이머 리셋
                combatNoEnemyTimer = 0f;
            }
            else
            {
                // "전투 적 없음" 상태가 누적되면 탐험으로 복귀
                combatNoEnemyTimer += combatExitCheckInterval;  // 이미 combatExitCheckInterval 만큼 시간이 흘렀기 때문에 해당 수치를 누적
                if (!bossCombatActive && combatNoEnemyTimer >= combatNoEnemyDuration)
                {
                    gm.EnterExploration();
                }
            }
        }
        else
        {
            // 컴뱃이 아닐 때는 타이머 리셋
            combatExitCheckTimer = 0f;
            combatNoEnemyTimer = 0f;
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

        if(activeEnemies.Count == 0 && !bossCombatActive && GameManager.Instance.CurrentGameState is CombatState)
        {
            GameManager.Instance.EnterExploration();
        }
    }

    public void RegisterBossCombat()
    {
        bossCombatActive = true;
        combatExitCheckTimer = 0f;
        combatNoEnemyTimer = 0f;
    }

    public void UnregisterBossCombat()
    {
        bossCombatActive = false;
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

        var agent = enemy.Fsm.Agent;
        if(agent != null && agent.enabled)
        {
            agent.Warp(position);
            enemy.transform.rotation = rotation;
        }
        else
        {
            enemy.transform.SetPositionAndRotation(position, rotation);
        }

        enemy.enabled = true;
        enemy.ResetState();
        
        return enemy;
    }

    public void DespawnAllEnemiesInScene()
    {
        // 스폰포인트에 붙어 있는 애들 정리
        var spawnPoints = FindObjectsByType<EnemySpawnPoint>(FindObjectsSortMode.None);
        foreach (var sp in spawnPoints)
            sp.DespawnAllEnemies();

        // EnemyManager가 직접 관리하는 activeEnemies도 정리
        var copy = new List<Enemy>(activeEnemies);
        foreach (var enemy in copy)
        {
            if (enemy == null) continue;
            ReleaseEnemy(enemy);
        }

        activeEnemies.Clear();
        combatExitCheckTimer = 0f;
        combatNoEnemyTimer = 0f;
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

    private void HandleSceneUnloaded(Scene _)
    {
        // 씬이 내려가기 전에 적 풀을 정리해 다음 씬에 흔적이 남지 않도록 한다.
        if (activeEnemies.Count == 0) return;
        DespawnAllEnemiesInScene();
    }

    private bool HasRelevantCombatEnemy(Vector3 playerPosition)
    {
        float now = Time.time;
        float maxSqrDist = combatMaxHorizontalDistance * combatMaxHorizontalDistance;

        Vector3 playerPos = playerPosition;
        playerPos.y = 0f;

        foreach (var enemy in activeEnemies)
        {
            if (enemy == null || !enemy.isActiveAndEnabled)
                continue;

            // 최근에 플레이어를 본 적이 없다면 스킵
            if (now - enemy.LastSeenPlayerTime > combatRecentSeenTime)
                continue;

            // 가로 거리 계산
            Vector3 enemyPos = enemy.transform.position;
            enemyPos.y = 0f;

            float sqrDist = (enemyPos - playerPos).sqrMagnitude;
            if (sqrDist <= maxSqrDist)
            {
                // 전투적으로 의미 있는 적이 최소 1마리 존재
                return true;
            }
        }

        return false;
    }
}
