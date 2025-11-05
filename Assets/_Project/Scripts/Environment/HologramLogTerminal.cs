using UnityEngine;

public class HologramLogTerminal : MonoBehaviour, IInteractable
{
    [SerializeField] [TextArea] private string[] logText;
    public string GetPrompt()
    {
        return "로그 확인하기";
    }

    public void Interact()
    {
        // TODO: 효과음, 효과 등

        UIManager.Instance.ShowSubtitleHold(logText, TMPro.TextAlignmentOptions.Left);
    }
}
