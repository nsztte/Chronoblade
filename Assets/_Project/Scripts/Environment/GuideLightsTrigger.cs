using UnityEngine;

public class GuideLightsTrigger : MonoBehaviour
{
    [SerializeField] private GuideLightMover[] lightMovers;
    private bool isActive = false;

    private void Awake()
    {
        foreach (var l in lightMovers)
        {
            l.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (!isActive)
        {
            foreach (var l in lightMovers)
            {
                l.gameObject.SetActive(true);
                l.StartGuide();
            }
        }

        isActive = true;
    }
}
