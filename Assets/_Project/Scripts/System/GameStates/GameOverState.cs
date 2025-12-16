using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName="GameState/GameOver")]
public class GameOverState : GameBaseState
{
    private Coroutine fadeOutCo;

    public override void Enter()
    {
        Debug.Log("[GameState] GameOverState Enter");
        TimeManager.Instance.SetTimeScale(0f);
        UIManager.Instance?.UpdatePlayerHud(false);

        SaveGuard.Instance?.Block(SaveBlockTag.GameOver);

        // 페이드아웃 시작
        if (GameManager.Instance != null)
        {
            fadeOutCo = GameManager.Instance.StartCoroutine(FadeOutScreen());
        }
    }

    public override void Exit()
    {
        // 실행 중인 코루틴 중단
        if (fadeOutCo != null && GameManager.Instance != null)
        {
            GameManager.Instance.StopCoroutine(fadeOutCo);
            fadeOutCo = null;
        }

        UIManager.Instance?.GameOverUI.Hide();
        UIManager.Instance?.FadeUI.Hide(0f);

        SaveGuard.Instance?.ClearTag(SaveBlockTag.GameOver);
    }

    private IEnumerator FadeOutScreen()
    {
        yield return UIManager.Instance?.FadeUI.Show(1f);  // 페이드 아웃

        UIManager.Instance?.GameOverUI.Show();
        UIManager.Instance?.FadeUI.Hide(0f);
    }
}
