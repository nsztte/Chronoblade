using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    [SerializeField] GameObject SaveLoadGroup;
    [SerializeField] SaveUI saveUI;
    [SerializeField] Button saveButton;
    [SerializeField] Button loadButton;

    private void OnEnable()
    {
        saveButton.onClick.AddListener(() => Open(SaveUI.SaveUIMode.SaveOnly));
        loadButton.onClick.AddListener(() => Open(SaveUI.SaveUIMode.LoadOnly));
    }

    private void Open(SaveUI.SaveUIMode mode)
    {
        SaveLoadGroup.SetActive(false);
        saveUI.Open(mode);
    }
}
