using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private List<Enemy> activeEnemies = new List<Enemy>();

    #region Singleton
    public static EnemyManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
