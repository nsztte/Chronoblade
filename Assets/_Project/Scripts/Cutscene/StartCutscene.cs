using System.Collections;
using UnityEngine;

public class StartCutscene : BaseCutscene
{
    private void Start()
    {
        fadeUI.ShowBlackScreen();

        StartCoroutine(Play());
    }

    protected override IEnumerator RunSequence()
    {
        ui.ShowSubtitleHold(new []{
            "..........",
            ".............."
        });

        yield return new WaitUntil(() => !subtitleUI.IsPlaying);

        // 눈 깜빡임
        yield return Blink();
        yield return Blink();

        // 대사 출력
        ui.ShowSubtitleAuto(new []{
            "……신호 수신 완료.",
            "응답 없음. 생체신호 재활성화 중.",
            "……여긴… 어디지?"
        });

        yield return new WaitUntil(() => !subtitleUI.IsPlaying);
    }

    private IEnumerator Blink()
    {
        yield return fadeUI.Show();
        yield return fadeUI.Hide();
    }
}
