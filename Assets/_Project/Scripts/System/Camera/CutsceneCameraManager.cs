using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
using System;

public class CutsceneCameraManager : MonoBehaviour
{
    [SerializeField] private GameObject playerCinemachineCamera;
    // [SerializeField] private GameObject clockPuzzleCinemachineCamera;

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

    // public void StartPuzzle()
    // {
    //     playerCinemachineCamera.SetActive(true);
    //     StartCoroutine(EnableClockCamNextFrame());
    // }

    // public void EndPuzzle(Action onComplete)
    // {
    //     clockPuzzleCinemachineCamera.SetActive(false);
    //     StartCoroutine(DisablePlayerCameraAfterBlend(onComplete));
    // }

    // private IEnumerator EnableClockCamNextFrame()
    // {
    //     yield return null;
    //     clockPuzzleCinemachineCamera.SetActive(true);
    // }

    public void StartCutscene(GameObject targetCamera)
    {
        playerCinemachineCamera.SetActive(true);
        StartCoroutine(EnableCamNextFrame(targetCamera));
    }

    public void EndCutscene(GameObject targetCamera, Action onComplete = null)
    {
        targetCamera.SetActive(false);
        StartCoroutine(DisablePlayerCameraAfterBlend(onComplete));
    }

    private IEnumerator EnableCamNextFrame(GameObject cam)
    {
        yield return null;
        cam.SetActive(true);
    }

    private IEnumerator DisablePlayerCameraAfterBlend(Action onComplete = null)
    {
        yield return null;

        yield return new WaitUntil(() => !IsBlending());

        playerCinemachineCamera.SetActive(false);
        onComplete?.Invoke();
    }

    private bool IsBlending()
    {
        return Camera.main.TryGetComponent(out CinemachineBrain brain) && brain.IsBlending;
    }
}
