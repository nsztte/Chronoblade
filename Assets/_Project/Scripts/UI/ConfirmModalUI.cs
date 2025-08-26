using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConfirmModalUI : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;

    public void Show(string title, string message, Action onConfirm, Action onCancel)
    {
        UIManager.Instance.ShowOverlayBackground();
        gameObject.SetActive(true);
        titleText.text = title;
        messageText.text = message;
        confirmButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();

        confirmButton.onClick.AddListener(() => {
            onConfirm?.Invoke();
            Hide();
        });

        cancelButton.onClick.AddListener(() => {
            onCancel?.Invoke();
            Hide();
        });
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        UIManager.Instance.HideOverlayBackground();
    }
}