using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class ScreenTabController : MonoBehaviour, IOptionsTab
{
    [Header("드롭다운")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown windowModeDropdown;
    [SerializeField] private TMP_Dropdown refreshRateDropdown;
    [SerializeField] private TMP_Dropdown vSyncDropdown;

    private Resolution[] availableResolutions;
    private int selectedResolutionIndex;
    private RefreshRate selectedRefreshRate;
    private FullScreenMode selectedScreenMode;
    private int selectedVSyncCount;

    private const string PREF_RESOLUTION = "ResolutionIndex";
    private const string PREF_SCREENMODE = "ScreenMode";
    private const string PREF_REFRESHRATE = "RefreshRate";
    private const string PREF_VSYNC = "VSyncCount";

    private void Awake()
    {
        InitResolutionDropdown();
        InitWindowModeDropdown();
        InitRefreshRateDropdown();
        InitVSyncDropdown();

        LoadAndApplyPlayerPrefs();
    }

    public void OnOpen(OptionOpenMode from)
    {
        gameObject.SetActive(true);
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
    }

    #region 초기화
    private void InitResolutionDropdown()
    {
        availableResolutions = Screen.resolutions;
        var options = availableResolutions
            .Select(r => $"{r.width} x {r.height}")
            .Distinct()
            .ToList();

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
    }

    private void InitWindowModeDropdown()
    {
        var modes = new List<string> { "전체화면", "경계선 없음", "창 모드" };
        windowModeDropdown.ClearOptions();
        windowModeDropdown.AddOptions(modes);
        windowModeDropdown.onValueChanged.AddListener(OnWindowModeChanged);
    }

    private void InitRefreshRateDropdown()
    {
        var rates = availableResolutions
            .Where(r => r.width == availableResolutions[selectedResolutionIndex].width &&
                        r.height == availableResolutions[selectedResolutionIndex].height)
            .Select(r => r.refreshRateRatio)
            .Distinct()
            .ToList();

        var options = rates.Select(r => $"{r}Hz").ToList();
        refreshRateDropdown.ClearOptions();
        refreshRateDropdown.AddOptions(options);
        refreshRateDropdown.onValueChanged.AddListener(OnRefreshRateChanged);

        // 기본값 설정
        selectedRefreshRate = rates[0];
        refreshRateDropdown.value = 0;
    }

    private void InitVSyncDropdown()
    {
        var vsyncOptions = new List<string> { "Off", "On" };
        vSyncDropdown.ClearOptions();
        vSyncDropdown.AddOptions(vsyncOptions);
        vSyncDropdown.onValueChanged.AddListener(OnVSyncChanged);
    }
    #endregion

    #region 실행
    private void OnResolutionChanged(int index)
    {
        Debug.Log("OnResolutionChanged 실행");
        selectedResolutionIndex = index;
        InitRefreshRateDropdown();
        SaveScreenSettings();
        ApplyScreenSettings();
    }

    private void OnWindowModeChanged(int index)
    {
        Debug.Log("OnWindowModeChanged 실행");
        selectedScreenMode = (FullScreenMode)index;
        SaveScreenSettings();
        ApplyScreenSettings();
    }

    private void OnRefreshRateChanged(int index)
    {
        Debug.Log("OnRefreshRateChanged 실행");
        var rates = availableResolutions
            .Where(r => r.width == availableResolutions[selectedResolutionIndex].width &&
                        r.height == availableResolutions[selectedResolutionIndex].height)
            .Select(r => r.refreshRateRatio)
            .Distinct()
            .ToList();

        selectedRefreshRate = rates[index];
        SaveScreenSettings();
        ApplyScreenSettings();
    }

    private void OnVSyncChanged(int index)
    {
        Debug.Log("OnVSyncChanged 실행");
        selectedVSyncCount = (index == 1) ? 1 : 0;
        QualitySettings.vSyncCount = selectedVSyncCount;
        PlayerPrefs.SetInt(PREF_VSYNC, selectedVSyncCount);
    }

    private void ApplyScreenSettings()
    {
        var resolution = availableResolutions
            .FirstOrDefault(r => r.width == availableResolutions[selectedResolutionIndex].width &&
                                 r.height == availableResolutions[selectedResolutionIndex].height &&
                                 r.refreshRateRatio.value == selectedRefreshRate.value);

        Screen.SetResolution(resolution.width, resolution.height, selectedScreenMode, selectedRefreshRate);
    }

    private void SaveScreenSettings()
    {
        PlayerPrefs.SetInt(PREF_RESOLUTION, selectedResolutionIndex);
        PlayerPrefs.SetInt(PREF_SCREENMODE, (int)selectedScreenMode);
        PlayerPrefs.SetInt(PREF_REFRESHRATE, Mathf.RoundToInt((float)selectedRefreshRate.value));
        PlayerPrefs.SetInt(PREF_VSYNC, selectedVSyncCount);
        PlayerPrefs.Save();
    }

    private void LoadAndApplyPlayerPrefs()
    {
        selectedResolutionIndex = PlayerPrefs.GetInt(PREF_RESOLUTION, 0);
        selectedScreenMode = (FullScreenMode)PlayerPrefs.GetInt(PREF_SCREENMODE, (int)FullScreenMode.FullScreenWindow);

        // 저장된 주사율 Hz를 다시 RefreshRate로 복원
        int savedHz = PlayerPrefs.GetInt(PREF_REFRESHRATE, Mathf.RoundToInt((float)availableResolutions[selectedResolutionIndex].refreshRateRatio.value));
        selectedRefreshRate = new RefreshRate { numerator = (uint)savedHz, denominator = 1u };

        selectedVSyncCount = PlayerPrefs.GetInt(PREF_VSYNC, 1);

        // 드롭다운 UI에 값 반영 (이벤트 트리거 방지)
        resolutionDropdown.SetValueWithoutNotify(selectedResolutionIndex);
        windowModeDropdown.SetValueWithoutNotify((int)selectedScreenMode);
        InitRefreshRateDropdown(); // dropdown 초기화는 반드시 resolution 이후
        refreshRateDropdown.SetValueWithoutNotify(0); // selectedRefreshRate를 기준으로 인덱스를 찾아도 됨
        vSyncDropdown.SetValueWithoutNotify(selectedVSyncCount == 1 ? 1 : 0);

        ApplyScreenSettings();
        QualitySettings.vSyncCount = selectedVSyncCount;
    }
    #endregion
}
