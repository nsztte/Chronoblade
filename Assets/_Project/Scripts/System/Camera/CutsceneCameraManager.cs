using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class CutsceneCameraManager : MonoBehaviour
{
    [SerializeField] private GameObject playerCinemachineCamera;
    [SerializeField] private GameObject clockPuzzleCinemachineCamera;

    #region Singleton
    public static CutsceneCameraManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    #endregion

    public void StartPuzzle()
    {
        playerCinemachineCamera.SetActive(true);
        StartCoroutine(EnableClockCamNextFrame());
    }

    public void EndPuzzle()
    {
        clockPuzzleCinemachineCamera.SetActive(false);
        StartCoroutine(DisablePlayerCameraAfterBlend());
    }

    private IEnumerator EnableClockCamNextFrame()
    {
        yield return null;
        clockPuzzleCinemachineCamera.SetActive(true);
    }

    private IEnumerator DisablePlayerCameraAfterBlend()
    {
        yield return null;

        yield return new WaitUntil(() => !IsBlending());

        playerCinemachineCamera.SetActive(false);
    }

    private bool IsBlending()
    {
        return Camera.main.TryGetComponent(out CinemachineBrain brain) && brain.IsBlending;
    }
}
