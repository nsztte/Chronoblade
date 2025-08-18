using UnityEngine;

public class PossessionPortal : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject mazeCamera;
    [SerializeField] private PlayerMazeController playerMazeController;

    private bool isPlayerInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        isPlayerInside = true;

        CutsceneCameraManager.Instance.StartCutscene(mazeCamera);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") || !isPlayerInside) return;
        
        isPlayerInside = false;

        CutsceneCameraManager.Instance.EndCutscene(mazeCamera);
    }

    public void Interact()
    {
        if (!isPlayerInside) return;

        isPlayerInside = false;

        CutsceneCameraManager.Instance.EndCutscene(mazeCamera, () =>
        {
            playerMazeController?.SetPossessed(true);
        });
    }
}
