using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private List<Enemy> activeEnemies = new List<Enemy>();

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
}
