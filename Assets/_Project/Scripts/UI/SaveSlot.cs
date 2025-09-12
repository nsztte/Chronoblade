using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class SaveSlot : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private int slotIndex;
    [SerializeField] private GameObject frameRoot;
    [SerializeField] private RawImage preview;
    [SerializeField] private TMP_Text questAndTypeText;
    [SerializeField] private TMP_Text playtimeText;
    [SerializeField] private TMP_Text sceneNameText;
    [SerializeField] private TMP_Text savedAtText;  // "Quick / Auto / Manual" 작은 라벨
    [SerializeField] private GameObject selectedHighlight;

    [Header("프리뷰 컬러 설정")]
    [SerializeField] private Color deactivatePreview;
    [SerializeField] private Color activatePreview;


    private Button button;
    private Texture2D previewTexture;
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
        
            LoadPreview(meta.thumbnail);
        }
        else
        {
            if (questAndTypeText) questAndTypeText.text = "-";
            if (playtimeText) playtimeText.text = "-";
            if (sceneNameText) sceneNameText.text = "-";
            if (savedAtText)  savedAtText.text  = "-";

            UnloadPreview();
        }
    }

    private void LoadPreview(string relativePath)
    {
        UnloadPreview();    // 기존 텍스처 정리

        if (preview == null || string.IsNullOrEmpty(relativePath)) 
        {
            if (preview != null) preview.texture = null;
            return;
        }

        string full = Path.Combine(Application.persistentDataPath, relativePath);
        if (!File.Exists(full))
        {
            // 파일 없으면 플레이스홀더 유지(혹은 null)
            if (preview != null) preview.texture = null;
            return;
        }

        try
        {
            byte[] bytes = File.ReadAllBytes(full);
            var tex = new Texture2D(2, 2, TextureFormat.RGB24, false);
            if (tex.LoadImage(bytes))
            {
                previewTexture = tex;
                preview.texture = previewTexture;
                preview.color = activatePreview;
                // preview.uvRect = new Rect(0,0,1,1);
            }
            else
            {
                Destroy(tex);
                preview.texture = null;
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning($"세이브슬롯 프리뷰 로드 실패: {e}");
            preview.texture = null;
        }
    }

    private void UnloadPreview()
    {
        if (previewTexture != null)
        {
            Destroy(previewTexture);
            previewTexture = null;
        }
        if (preview != null)
        {
            preview.color = deactivatePreview;
            preview.texture = null;
        }
    }

    public void Clear()
    {
        SetSelected(false);
        SetInteractable(false);
        OnClicked = null;
    }

    public void SetSelected(bool selected)
    {
        if (selectedHighlight)
            selectedHighlight.SetActive(selected);
    }

    public void SetInteractable(bool value) => button.interactable = value;
}
