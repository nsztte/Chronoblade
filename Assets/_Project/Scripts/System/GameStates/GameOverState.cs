using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName="GameState/GameOver")]
public class GameOverState : GameBaseState
{
    private Coroutine deathSequenceCoroutine;

    public override void Enter()
    {
        Debug.Log("[GameState] GameOverState Enter");
        UIManager.Instance.ShowGameOverScreen();
        TimeManager.Instance.SetTimeScale(0f);

        SaveGuard.Instance?.Block(SaveBlockTag.GameOver);

        // 페이드아웃과 사망 연출 시작
        if (GameManager.Instance != null)
        {
            deathSequenceCoroutine = GameManager.Instance.StartCoroutine(DeathSequence());
        }
    }

    public override void Exit()
    {
        // 실행 중인 코루틴 중단
        if (deathSequenceCoroutine != null && GameManager.Instance != null)
        {
            GameManager.Instance.StopCoroutine(deathSequenceCoroutine);
            deathSequenceCoroutine = null;
        }

        UIManager.Instance.HideGameOverScreen();

        SaveGuard.Instance?.ClearTag(SaveBlockTag.GameOver);
    }

    private IEnumerator DeathSequence()
    {
        // 1. 화면 페이드아웃 (1초)
        if (GameManager.Instance != null)
        {
            yield return GameManager.Instance.StartCoroutine(FadeOutScreen());
        }
        else
        {
            yield break;
        }
        
        // 2. "You are dead" 메시지 표시 (2초)
        // TODO: UIManager.Instance.ShowDeathMessage("You are dead");
        Debug.Log("You are dead");
        yield return new WaitForSecondsRealtime(2f);
        
        // 3. 즉시 리스폰 (Loading 상태로 전환)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.EnterLoading();
        }
    }

    private IEnumerator FadeOutScreen()
    {
        // TODO: ScreenEffectManager 구현 후 실제 페이드아웃 적용
        // ScreenEffectManager.Instance?.StartDeathFadeOut();
        
        // 임시로 시간만 대기
        yield return new WaitForSecondsRealtime(1f);
    }
}
