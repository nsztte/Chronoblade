using UnityEngine;

public class TitleUIManager : MonoBehaviour
{
    public static TitleUIManager Instance;
    [SerializeField] private GameObject titleUI;
    [SerializeField] private GameObject mainMenuUI;

    void Awake()
    {
        Instance = this;
    }

    public void ShowTitle() => titleUI.SetActive(true);
    // public void HideTitle() => titleUI.SetActive(false);
    // public void HideMainMenu() => mainMenuUI.SetActive(false);
}