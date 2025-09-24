using UnityEngine;

[CreateAssetMenu(menuName="GameState/Loading")]
public class LoadingState : GameBaseState
{
    public static int NextSlotToLoad = -1;

    public override void Enter()
    {
        Debug.Log("[GameState] LoadingState Enter");
        // UIManager.Instance.ShowLoadingScreen();
        UIManager.Instance?.UpdateUI(false);
        InputManager.Instance?.SetInputEnabled(false);
        TimeManager.Instance.SetTimeScale(0f);

        SaveGuard.Instance?.Block(SaveBlockTag.UI);

        // TODO: 실제 로딩 처리
        // TODO: 씬 매니저 연동
        // TODO: 세이브/로드 시스템 연동

        SaveManager.Instance.OnAfterLoad += HandleAfterLoad;

        // 실제 로드 시작
        if (NextSlotToLoad >= 0)
            SaveManager.Instance.DefaultLoad(NextSlotToLoad);
    }

    private void HandleAfterLoad()
    {
        SaveManager.Instance.OnAfterLoad -= HandleAfterLoad;
        GameManager.Instance.EnterExploration();
    }

    public override void Exit()
    {
        Debug.Log("[GameState] LoadingState Exit");
        // UIManager.Instance.HideLoadingScreen();
        UIManager.Instance?.UpdateUI(true);
        InputManager.Instance?.SetInputEnabled(true);
        TimeManager.Instance.SetTimeScale(1f);

        SaveGuard.Instance?.Unblock(SaveBlockTag.UI);

        // GameManager.Instance.EnterExploration();
    }
}
