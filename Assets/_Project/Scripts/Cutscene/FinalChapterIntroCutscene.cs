using System.Collections;
using UnityEngine;

public class FinalChapterIntroCutscene : BaseCutscene
{
    [SerializeField] private GameObject cinemachineCam;
    [SerializeField] private Animator camAnimator;
    [SerializeField] private Transform sceneStartPoint;

    static readonly int Final_IntroHash = Animator.StringToHash("Final_Intro");

    private void Awake()
    {
        ForceUnscaledAnimators(camAnimator);
    }

    public void StartPlay()
    {
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

        camAnimator.Play(Final_IntroHash, 0, 0);

        yield return WaitAnimDone(camAnimator, Final_IntroHash);

        // 대사 출력
        ui.ShowSubtitleHold(new []{
            "…여긴 대체 뭐지?",
            "공기가… 멈춰 있는 것 같아.",
            "일단, 안으로 들어가보자."
        });

        yield return new WaitUntil(() => !subtitleUI.IsPlaying);

        CutsceneCameraManager.Instance.EndCutscene(cinemachineCam, autoSave: true);
    }

    private IEnumerator Blink()
    {
        yield return fadeUI.Show();
        yield return fadeUI.Hide();
    }

    protected override void OnBeforePlay()
    {
        var pc = PlayerManager.Instance?.PlayerController;
        if (pc && sceneStartPoint)
            pc.SetPositionAndRotation(sceneStartPoint.position, sceneStartPoint.rotation);

        fadeUI.ShowBlackScreen();
        cm.StartCutscene(cinemachineCam);
    }

    protected override void OnAfterPlay(){}
}
