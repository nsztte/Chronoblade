using UnityEngine;

[CreateAssetMenu(menuName="GameState/MainMenu")]
public class MainMenuState : GameBaseState
{
    public override void Enter()
    {
        Debug.Log("[GameState] MainMenuState Enter");
        // 움직임 불가
        // UIManager.Instance.UpdateUI(true);
        TitleUIManager.Instance.ShowTitle();

        // 입력 비활성화
        InputManager.Instance?.SetInputEnabled(false);

        // 타이틀 진입 상태
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 타임스케일 0
        TimeManager.Instance.SetTimeScale(0f);

        SaveGuard.Instance?.Block(SaveBlockTag.UI);
    }

    public override void Exit()
    {
        Debug.Log("MainMenuState Exit");
        // UIManager.Instance.UpdateUI(false);
        // 움직임 가능
        InputManager.Instance?.SetInputEnabled(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        TimeManager.Instance.SetTimeScale(1f);

        SaveGuard.Instance?.Unblock(SaveBlockTag.UI);
    }
}
