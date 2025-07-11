using UnityEngine;

public class RewindBoxSpawner : MonoBehaviour
{
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private Transform spawnPoint;

    private float timer = 0f;

    private void Start()
    {
        spawnPoint = transform;

        GameObject box = Instantiate(boxPrefab, spawnPoint.position, Quaternion.identity);
    }

    // private void Update()
    // {
    //     timer += Time.deltaTime;

    //     if(timer >= spawnInterval)
    //     {
    //         timer = 0f;
    //     }
    // }
}
