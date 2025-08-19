using UnityEngine;

public class PuzzleGate : MonoBehaviour
{
    [SerializeField] private int roomId;

    private void Start()
    {
        bool open = PuzzleProgressManager.Instance.IsUnlocked(roomId);
        gameObject.SetActive(!open);

        PuzzleProgressManager.Instance.OnRoomUnlocked += HandleUnlocked;
    }

    private void OnDestroy()
    {
        if (PuzzleProgressManager.Instance != null)
            PuzzleProgressManager.Instance.OnRoomUnlocked -= HandleUnlocked;
    }

    private void HandleUnlocked(int id)
    {
        if (id == roomId) gameObject.SetActive(false);
    }
}
