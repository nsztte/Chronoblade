using System;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [Header("패널")]
    [SerializeField] private GameObject gameOverGroup;
    [SerializeField] private GameObject loadPanel;

    [Header("버튼")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button loadPanelCloseButton;

    [Header("컨트롤러")]
    [SerializeField] private SaveTabController saveTabController;

    private bool isShown;

    private void Awake()
    {
        if (restartButton != null) restartButton.onClick.AddListener(HandleRestart);
        if (loadButton != null) loadButton.onClick.AddListener(OpenLoadPanel);
        if (mainMenuButton != null) mainMenuButton.onClick.AddListener(HandleMainMenu);

        if (loadPanelCloseButton != null) loadPanelCloseButton.onClick.AddListener(CloseLoadPanel);

        // 초기 상태
        if (gameOverGroup != null) gameOverGroup.SetActive(false);
        if (loadPanel != null) loadPanel.SetActive(false);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// GameOverState 진입 시 호출
    /// </summary>
    public void Show()
    {
        isShown = true;
        gameObject.SetActive(true);
        RefreshRestartButtonState();

        ShowGameOverGroup();

        // 첫 버튼 포커스
        if (restartButton != null) restartButton.Select();
    }

    /// <summary>
    /// GameOverState 종료 시 호출
    /// </summary>
    public void Hide()
    {
        isShown = false;

        if (loadPanel != null) loadPanel.SetActive(false);
        if (gameOverGroup != null) gameOverGroup.SetActive(false);

        gameObject.SetActive(false);
    }

    private void RefreshRestartButtonState()
    {
        if (restartButton == null) return;

        bool hasAny = SaveManager.Instance?.HasAnySave() ?? false;
        restartButton.interactable = hasAny;
    }

    private void ShowGameOverGroup()
    {
        if (gameOverGroup != null) gameOverGroup.SetActive(true);
        if (loadPanel != null) loadPanel.SetActive(false);
    }

    private void OpenLoadPanel()
    {
        if (!isShown) return;

        if (gameOverGroup != null) gameOverGroup.SetActive(false);
        if (loadPanel != null) loadPanel.SetActive(true);

        if (saveTabController != null)
        {
            saveTabController.OpenPanel(SaveTabController.SaveUIMode.LoadOnly);
        }

        // 닫기 버튼 포커스
        if (loadPanelCloseButton != null) loadPanelCloseButton.Select();
    }

    private void CloseLoadPanel()
    {
        if (!isShown) return;
        ShowGameOverGroup();

        if (restartButton != null) restartButton.Select();
    }

    private void HandleMainMenu()
    {
        GameManager.Instance.ReturnToTitle();
    }

    private void HandleRestart()
    {
        if (SaveManager.Instance == null)
        {
            Debug.LogWarning("[GameOverUI] SaveManager.Instance가 없음");
            return;
        }

        var all = SaveManager.Instance.GetAllMeta();

        // savedAt = "yyyy-MM-dd HH:mm:ss" 로 저장됨
        var latest = all
            .Where(p => p.meta != null && !string.IsNullOrEmpty(p.meta.savedAt))
            .Select(p => (p.slotIndex, dt: TryParseSavedAt(p.meta.savedAt)))
            .Where(p => p.dt.HasValue)
            .OrderByDescending(p => p.dt.Value)
            .FirstOrDefault();

        if (latest.slotIndex <= 0)
        {
            OpenLoadPanel();
            return;
        }

        LoadingState.NextLoadingMode = LoadingMode.LoadSave;
        LoadingState.NextSlotToLoad = latest.slotIndex;
        GameManager.Instance.EnterLoading();
    }

    private DateTime? TryParseSavedAt(string savedAt)
    {
        // 1) 정확 포맷 우선
        if (DateTime.TryParseExact(
                savedAt,
                "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var dt))
            return dt;

        // 2) 예외 포맷 대비 안전망
        if (DateTime.TryParse(savedAt, out dt))
            return dt;

        return null;
    }
}
