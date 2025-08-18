using UnityEngine;

public class UnpossessPortal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMazeController maze))
        {
            maze.IsNearPortal = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerMazeController maze))
        {
            maze.IsNearPortal = false;
        }
    }
}
