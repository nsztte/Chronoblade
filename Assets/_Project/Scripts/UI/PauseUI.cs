using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;
    private CanvasGroup group;

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        resumeButton.onClick.AddListener(() => InputManager.Instance.TriggerPause());
        optionsButton.onClick.AddListener(() => UIManager.Instance.OptionUI.Open(OptionOpenMode.InGame));
        // quitButton.onClick.AddListener(() => UIManager.Instance.QuitToTitle());
    }

    private void OnEnable()
    {
        InputManager.Instance.OnPause += Hide;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnPause -= Hide;
    }

    public void Show()
    {
        UIManager.Instance.ShowOverlayBackground();

        gameObject.SetActive(true);
        group.alpha = 1f;
    }

    public void Hide()
    {
        group.alpha = 0f;
        gameObject.SetActive(false);

        UIManager.Instance.HideOverlayBackground();
    }
}
