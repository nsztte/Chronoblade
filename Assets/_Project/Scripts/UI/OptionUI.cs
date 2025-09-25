using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public enum OptionOpenMode { Title, InGame }

public interface IOptionsTab
{
    void OnOpen(OptionOpenMode from);
    void OnClose();
    // void Refresh();
}

public class OptionUI : MonoBehaviour
{
    [Header("탭 버튼")]
    [SerializeField] private Button saveTabButton;
    [SerializeField] private Button screenTabButton;
    [SerializeField] private Button audioTabButton;
    [SerializeField] private Button controlTabButton;

    [Header("탭 그룹")]
    [SerializeField] private GameObject saveGroup;
    [SerializeField] private GameObject screenGroup;
    [SerializeField] private GameObject audioGroup;
    [SerializeField] private GameObject controlGroup;

    [Header("탭 컨트롤러")]
    [SerializeField] private SaveTabController saveTabController;
    [SerializeField] private ScreenTabController screenTabController;
    [SerializeField] private AudioTabController audioTabController;
    [SerializeField] private ControlTabController controlTabController;

    [Header("닫기 버튼")]
    [SerializeField] private Button closeButton;

    private IOptionsTab[] tabControllers;
    private GameObject[] tabGroups;
    private Button[] tabButtons;

    private int currentTab = -1;
    private OptionOpenMode currentMode;

    // 탭 우선순위
    private const int TAB_SAVE = 0;
    private const int TAB_SCREEN = 1;
    private const int TAB_AUDIO = 2;
    private const int TAB_CONTROL = 3;

    private void Awake()
    {
        tabButtons = new[] { saveTabButton, screenTabButton, audioTabButton, controlTabButton };
        tabGroups = new[] { saveGroup, screenGroup, audioGroup, controlGroup };
        tabControllers = new IOptionsTab[4];

        AssignTabController(TAB_SAVE, saveTabController);
        AssignTabController(TAB_SCREEN, screenTabController);
        AssignTabController(TAB_AUDIO, audioTabController);
        AssignTabController(TAB_CONTROL, controlTabController);

        saveTabButton.onClick.AddListener(() => ShowTab(TAB_SAVE));
        screenTabButton.onClick.AddListener(() => ShowTab(TAB_SCREEN));
        audioTabButton.onClick.AddListener(() => ShowTab(TAB_AUDIO));
        controlTabButton.onClick.AddListener(() => ShowTab(TAB_CONTROL));

        closeButton?.onClick.AddListener(() => Close());

        // gameObject.SetActive(false);
    }

    public void Open(OptionOpenMode mode)
    {
        currentMode = mode;
        gameObject.SetActive(true);

        bool isInGame = mode == OptionOpenMode.InGame;
        saveTabButton.gameObject.SetActive(isInGame);

        int firstTab = isInGame ? TAB_SAVE : TAB_SCREEN;
        ShowTab(firstTab);

        UIManager.Instance?.ShowOverlayBackground();
    }

    public void Close()
    {
        UIManager.Instance?.HideOverlayBackground();

        gameObject.SetActive(false);

        if (currentTab >= 0 && currentTab < tabControllers.Length)
            tabControllers[currentTab]?.OnClose();
    }

    public void SetCloseButtonAction(UnityAction action)
    {
        closeButton.onClick?.RemoveAllListeners();
        closeButton.onClick?.AddListener(action);
    }

    private void ShowTab(int index)
    {
        if (index < 0 || index >= tabGroups.Length)
            return;

        if (currentTab >= 0)
        {
            tabGroups[currentTab].SetActive(false);
            tabControllers[currentTab]?.OnClose();
        }

        currentTab = index;
        tabGroups[index].SetActive(true);
        // tabControllers[index]?.Refresh();
        tabControllers[index]?.OnOpen(currentMode);

        SetInitialFocus(tabGroups[index]);
    }

    private void AssignTabController(int index, MonoBehaviour mb)
    {
        if (mb is IOptionsTab tab)
            tabControllers[index] = tab;
        else
            tabControllers[index] = null;
    }

    private void SetInitialFocus(GameObject group)
    {
        var first = group.GetComponentInChildren<Selectable>();
        if (first != null)
            EventSystem.current.SetSelectedGameObject(first.gameObject);
    }
}
