using UnityEngine;

public class PortalTrigger : MonoBehaviour
{
    [Header("조건")]
    [SerializeField] private EnemySpawnPoint[] enemySpawnPoints;
    [SerializeField] private float checkInterval = 0.25f;

    [Header("포탈 오브젝트")]
    [SerializeField] private GameObject portalVisual;
    [SerializeField] private Collider portalCollider;

    private bool activated;

    private void Start()
    {
        SetPortalActive(false);

        // 일정 주기로 AreAllSpawnPointsCleared()를 호출
        InvokeRepeating(nameof(CheckEnemies), 1f, checkInterval);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!activated || portalVisual == null) return;
        if (!other.CompareTag("Player")) return;

        GameManager.Instance.EnterFinalChapter();
    }

    private void CheckEnemies()
    {
        if (activated) return;

        if (AreAllSpawnPointsCleared())
        {
            activated = true;
            SetPortalActive(true);

            // 정지
            CancelInvoke(nameof(CheckEnemies));
        }
    }

    private bool AreAllSpawnPointsCleared()
    {
        if (enemySpawnPoints == null || enemySpawnPoints.Length == 0) return false;

        foreach (var sp in enemySpawnPoints)
        {
            if (sp == null) continue;

            if (sp.ActiveEnemies != null && sp.ActiveEnemies.Count > 0)
                return false;
        }
        return true;
    }

    private void SetPortalActive(bool on)
    {
        if (portalVisual) portalVisual.SetActive(on);
        if (portalCollider) portalCollider.enabled = on;
    }
}
