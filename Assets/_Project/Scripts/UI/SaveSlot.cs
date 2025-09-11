using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [SerializeField] private int slotIndex;
    [SerializeField] private GameObject frameRoot;
    [SerializeField] private TMP_Text questAndTypeText;
    [SerializeField] private TMP_Text playtimeText;
    [SerializeField] private TMP_Text sceneNameText;
    [SerializeField] private TMP_Text savedAtText;  // "Quick / Auto / Manual" 작은 라벨
    [SerializeField] private GameObject selectedHighlight;

    private Button button;
    private SaveManager.SaveMeta meta;

    public event Action<int, SaveManager.SaveMeta> OnClicked;

    public int SlotIndex => slotIndex;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Init(int index, SaveManager.SaveMeta loadedMeta)
    {
        Clear();
        
        slotIndex = index;
        SetMeta(loadedMeta);
        bool hasMeta = meta != null;

        frameRoot.SetActive(hasMeta);
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OnClicked?.Invoke(slotIndex, meta));
    }

    public void SetMeta(SaveManager.SaveMeta newMeta)
    {
        meta = newMeta;

        if (meta != null)
        {
            if (questAndTypeText) questAndTypeText.text = "퀘스트 이름" + (string.IsNullOrEmpty(meta.savedAt) ? "-" : " - " + meta.saveType);  // 퀘스트 도입 후 교체
            if (playtimeText) playtimeText.text = SaveManager.SaveMeta.FormatPlaytime(meta.playtimeSeconds);
            if (sceneNameText) sceneNameText.text = string.IsNullOrEmpty(meta.scene) ? "-" : meta.scene;
            if (savedAtText)  savedAtText.text  = string.IsNullOrEmpty(meta.savedAt) ? "-" : meta.savedAt;
        }
        else
        {
            if (questAndTypeText) questAndTypeText.text = "-";
            if (playtimeText) playtimeText.text = "-";
            if (sceneNameText) sceneNameText.text = "-";
            if (savedAtText)  savedAtText.text  = "-";
        }
    }

    public void Clear()
    {
        SetSelected(false);
        SetInteractable(false);
        OnClicked = null;
    }
    
    public void SetSlotIndex(int index)
    {
        slotIndex = index;
    }

    public void SetSelected(bool selected)
    {
        if (selectedHighlight)
            selectedHighlight.SetActive(selected);
    }

    public void SetInteractable(bool value) => button.interactable = value;
}
