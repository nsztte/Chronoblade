using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("패널")]
    [SerializeField] private MainMenuLoadPanel loadPanel;
    [SerializeField] private OptionUI optionUI;

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
        if (optionUI != null) optionUI.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        // 이벤트 정리
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
        SceneManager.LoadScene(STARTSCENE);
    }

    private void OnContinue()
    {
        if (loadPanel != null)
        {
            loadPanel.gameObject.SetActive(!loadPanel.gameObject.activeSelf);
            if(loadPanel.gameObject.activeSelf) loadPanel.OpenPanel();
        }
    }

    private void OnOptions()
    {
        if (optionUI == null) return;

        if (!optionUI.gameObject.activeSelf)
            optionUI.Open(OptionOpenMode.Title);
        else
            optionUI.Close();
    }

    private void OnExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}