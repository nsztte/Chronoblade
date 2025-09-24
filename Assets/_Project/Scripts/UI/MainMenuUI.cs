using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("패널")]
    [SerializeField] private MainMenuLoadPanel loadPanel;
    [SerializeField] private GameObject optionPanel;

    [Header("버튼")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button exitButton;

    private const string STARTSCENE = "TestScene01";    // 테스트 이후 수정

    private void Awake()
    {
        // 버튼 이벤트 연결
        if (newGameButton != null)
            newGameButton.onClick.AddListener(OnNewGame);

        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinue);

        if (optionsButton != null)
            optionsButton.onClick.AddListener(OnOptions);

        if (exitButton != null)
            exitButton.onClick.AddListener(OnExit);

        // 패널은 처음에 꺼둠
        if (loadPanel != null) loadPanel.gameObject.SetActive(false);
        if (optionPanel != null) optionPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        // 이벤트 정리 (메모리릭 방지)
        if (newGameButton != null)
            newGameButton.onClick.RemoveListener(OnNewGame);

        if (continueButton != null)
            continueButton.onClick.RemoveListener(OnContinue);

        if (optionsButton != null)
            optionsButton.onClick.RemoveListener(OnOptions);

        if (exitButton != null)
            exitButton.onClick.RemoveListener(OnExit);
    }

    private void OnNewGame()
    {
        Debug.Log("New Game 시작");
        SceneManager.LoadScene(STARTSCENE);
    }

    private void OnContinue()
    {
        Debug.Log("Continue 실행");
        if (loadPanel != null)
        {
            loadPanel.gameObject.SetActive(!loadPanel.gameObject.activeSelf);
            if(loadPanel.gameObject.activeSelf) loadPanel.OpenPanel();
        }
    }

    private void OnOptions()
    {
        Debug.Log("Options 실행");
        if (optionPanel != null)
        {
            optionPanel.SetActive(!optionPanel.activeSelf);
        }
    }

    private void OnExit()
    {
        Debug.Log("게임 종료");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
