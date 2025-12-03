using UnityEngine;

public class CombatTutorialStarter : MonoBehaviour
{
    [SerializeField] private EnemySpawnPoint spawnPoint;

    public void OnSwordPicked()
    {
        // 튜토리얼 시작
        CombatTutorialManager.Instance?.StartCombatTutorial();

        // 튜토리얼 와쳐가 플레이어를 바로 인식하도록
        if (spawnPoint != null)
        {
            foreach (var te in spawnPoint.ActiveEnemies)
            {
                te.ResetDetection();
                te.DetectPlayer();
            }
        }
    }
}