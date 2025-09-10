using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text sceneText;
    [SerializeField] private TMP_Text savedAtText;
    [SerializeField] private TMP_Text playtimeText;
    [SerializeField] private TMP_Text typeBadgeText;  // "Quick / Auto / Manual" 작은 라벨
    [SerializeField] private GameObject selectedHighlight;

    private Button button;
    private int slotIndex;
    private SaveManager.SaveMeta meta;

    public event Action<int, SaveManager.SaveMeta> OnClicked;

    public int SlotIndex => slotIndex;
    public bool HasMeta => meta != null;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Init(int index, SaveManager.SaveMeta loadedMeta)
    {
        slotIndex = index;
        SetMeta(loadedMeta);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OnClicked?.Invoke(slotIndex, meta));
    }

    public void SetMeta(SaveManager.SaveMeta newMeta)
    {
        meta = newMeta;

        if (meta != null)
        {
            if (sceneText)    sceneText.text    = string.IsNullOrEmpty(meta.scene) ? "-" : meta.scene;
            if (savedAtText)  savedAtText.text  = string.IsNullOrEmpty(meta.savedAt) ? "-" : meta.savedAt;
            if (playtimeText) playtimeText.text = SaveManager.SaveMeta.FormatPlaytime(meta.playtimeSeconds);
            if (typeBadgeText) typeBadgeText.text = string.IsNullOrEmpty(meta.saveType) ? "" : meta.saveType;
        }
        else
        {
            if (sceneText)    sceneText.text    = "-";
            if (savedAtText)  savedAtText.text  = "-";
            if (playtimeText) playtimeText.text = "-";
            if (typeBadgeText) typeBadgeText.text = "";
        }
    }

    public void SetSelected(bool selected)
    {
        if (selectedHighlight)
            selectedHighlight.SetActive(selected);
    }

    public void SetInteractable(bool value) => button.interactable = value;
}
