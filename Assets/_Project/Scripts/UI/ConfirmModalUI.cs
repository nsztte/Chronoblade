using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConfirmModalUI : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;

    public void Show(string title, string message, Action onConfirm, Action onCancel)
    {
        root.SetActive(true);
        UIManager.Instance.SetCursorLockState(CursorLockMode.None);
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
        UIManager.Instance.SetCursorLockState(CursorLockMode.Locked);
        root.SetActive(false);
    }
}