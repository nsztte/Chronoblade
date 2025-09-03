using UnityEngine;

public class RewindBoxSpawner : MonoBehaviour
{
    [SerializeField] private GameObject boxPrefab;
    // [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private Transform spawnPoint;

    // private float timer = 0f;

    private void Start()
    {
        spawnPoint = transform;

        GameObject box = Instantiate(boxPrefab, spawnPoint.position, Quaternion.identity);
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, PlayerManager.Instance.transform.position);

        if(distance <= 100f)
        {
            GameManager.Instance.ChangeState(GameManager.Instance.puzzleState);
        }
    }
}
