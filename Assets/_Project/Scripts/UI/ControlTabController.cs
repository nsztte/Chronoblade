using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControlTabController : MonoBehaviour, IOptionsTab
{
    [Header("슬라이더")]
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private TextMeshProUGUI sensitivityValueText;

    [Header("토글")]
    [SerializeField] private UISwitcher.UISwitcher invertYToggle;
    [SerializeField] private UISwitcher.UISwitcher toggleSprintToggle;
    [SerializeField] private UISwitcher.UISwitcher toggleCrouchToggle;

    private const float STEP = 0.05f;

    private void Awake()
    {
        LoadControlSettingsToUI();

        mouseSensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
        invertYToggle.onValueChanged.AddListener(OnInvertYChanged);
        toggleSprintToggle.onValueChanged.AddListener(OnToggleSprintChanged);
        toggleCrouchToggle.onValueChanged.AddListener(OnToggleCrouchChanged);
    }

    public void OnOpen(OptionOpenMode from)
    {
        gameObject.SetActive(true);
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
    }

    private void OnSensitivityChanged(float rawValue)
    {
        var stepped = Mathf.Round(rawValue / STEP) * STEP;
        mouseSensitivitySlider.SetValueWithoutNotify(stepped);
        sensitivityValueText.text = stepped.ToString("0.0");

        InputManager.Instance.SetMouseSensitivity(stepped, stepped);
    }

    private void OnInvertYChanged(bool value)
    {
        InputManager.Instance.SetInvertMouseY(value);
    }

    private void OnToggleSprintChanged(bool value)
    {
        InputManager.Instance.SetToggleSprint(value);
    }

    private void OnToggleCrouchChanged(bool value)
    {
        InputManager.Instance.SetToggleCrouch(value);
    }

    private void LoadControlSettingsToUI()
    {
        var input = InputManager.Instance;
        mouseSensitivitySlider.SetValueWithoutNotify(input.MouseSensitivityX);
        sensitivityValueText.text = input.MouseSensitivityX.ToString("0.0");
        invertYToggle.SetWithoutNotify(input.InvertMouseY);
        toggleSprintToggle.SetWithoutNotify(input.ToggleSprint);
        toggleCrouchToggle.SetWithoutNotify(input.ToggleCrouch);
    }
}
