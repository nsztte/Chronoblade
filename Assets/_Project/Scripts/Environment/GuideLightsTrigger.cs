using UnityEngine;

public class GuideLightsTrigger : MonoBehaviour
{
    [SerializeField] private GuideLightMover[] lightMovers;
    private bool isActive = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (!isActive)
        {
            foreach (var l in lightMovers)
            {
                l.StartGuide();
            }
        }

        isActive = true;
    }
}
