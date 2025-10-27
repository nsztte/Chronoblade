using UnityEngine;
using UnityEngine.SceneManagement;

public enum LoadingMode { None, NewGame, LoadSave, SceneTransition, Ending }

[CreateAssetMenu(menuName="GameState/Loading")]
public class LoadingState : GameBaseState
{
    public static LoadingMode NextLoadingMode = LoadingMode.None;
    public static int NextSlotToLoad = -1;
    public static string NextSceneName = "";   // 테스트 이후 수정
    private const string STARTSCENE = "Chapter_1";
    private const string TitleScene = "Title";

    public override void Enter()
    {
        Debug.Log("[GameState] LoadingState Enter");

        // UIManager.Instance.ShowLoadingScreen();
        // UIManager.Instance?.UpdateUI(false);

        InputManager.Instance?.SetInputEnabled(false);
        TimeManager.Instance.SetTimeScale(0f);
        SaveGuard.Instance?.Block(SaveBlockTag.UI);

        // TODO: 실제 로딩 처리
        // TODO: 씬 매니저 연동
        // TODO: 세이브/로드 시스템 연동

        switch (NextLoadingMode)
        {
            case LoadingMode.NewGame:
                // 직접 씬 로드 후 Exploration으로 진입
                SceneManager.sceneLoaded += OnSceneLoaded;
                SceneManager.LoadScene(STARTSCENE);
                break;

            case LoadingMode.LoadSave:
                if (NextSlotToLoad >= 0)
                {
                    SaveManager.Instance.OnAfterLoad += HandleAfterLoad;
                    SaveManager.Instance.DefaultLoad(NextSlotToLoad);
                }
                break;

            case LoadingMode.SceneTransition:
                SceneManager.sceneLoaded += OnSceneLoaded;
                SceneManager.LoadScene(NextSceneName);
                break;

            case LoadingMode.Ending:
                SceneManager.sceneLoaded += OnSceneLoaded;
                SceneManager.LoadScene(TitleScene);
                break;
        }
    }

    private void HandleAfterLoad()
    {
        SaveManager.Instance.OnAfterLoad -= HandleAfterLoad;
        GameManager.Instance.EnterExploration();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GameManager.Instance.EnterExploration(); // 탐색 상태 진입
    }

    public override void Exit()
    {
        Debug.Log("[GameState] LoadingState Exit");

        // UIManager.Instance.HideLoadingScreen();
        // UIManager.Instance?.UpdateUI(true);

        InputManager.Instance?.SetInputEnabled(true);
        TimeManager.Instance.SetTimeScale(1f);
        SaveGuard.Instance?.Unblock(SaveBlockTag.UI);

        NextLoadingMode = LoadingMode.None;
        NextSlotToLoad = -1;
        NextSceneName = "";
    }
}
