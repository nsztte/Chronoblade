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

    public void Initialize()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning($"{GetType().Name} 인스턴스 감지됨, 초기화 스킵");
            return;
        }
        Instance = this;
    }
    #endregion

    private void OnDisable()
    {
        SaveGuard.Instance?.ClearTag(SaveBlockTag.Cutscene);
    }

    public void StartCutscene(GameObject targetCamera)
    {
        // 컷씬 시작: 저장 차단
        // SaveGuard.Instance?.Block(SaveBlockTag.Cutscene);
        // GameManager.Instance.EnterCutscene();

        playerCinemachineCamera.SetActive(true);
        StartCoroutine(EnableCamNextFrame(targetCamera));
    }

    public void EndCutscene(GameObject targetCamera, Action onComplete = null)
    {
        targetCamera.SetActive(false);
        StartCoroutine(DisablePlayerCameraAfterBlend(() =>
        {
            // 컷씬 종료: 저장 해제
            // SaveGuard.Instance?.Unblock(SaveBlockTag.Cutscene);
            GameManager.Instance.EnterPreviousState();
            CameraController.Instance?.ResetToPlayer();

            onComplete?.Invoke();

            // 자동 저장
            SaveManager.Instance?.AutoSave("컷씬 시작");
        }));
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

    public bool IsBlending()
    {
        return Camera.main.TryGetComponent(out CinemachineBrain brain) && brain.IsBlending;
    }
}
