using System.Collections;
using UnityEngine;

public class StartCutscene : BaseCutscene
{
    [SerializeField] private GameObject cinemachineCam;
    [SerializeField] private Animator camAnimator;

    static readonly int SitAndScanHash = Animator.StringToHash("SitAndScan");
    static readonly int TurnLeftAndStandHash = Animator.StringToHash("TurnLeftAndStand");

    private void Awake()
    {
        ForceUnscaledAnimators(camAnimator);
    }

    private void Start()
    {
        fadeUI.ShowBlackScreen();
        cm.StartCutscene(cinemachineCam);
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
        });

        yield return new WaitUntil(() => !subtitleUI.IsPlaying);

        camAnimator.SetTrigger(SitAndScanHash);
        yield return null; // 상태 전환 프레임
        yield return WaitAnimDone(camAnimator, SitAndScanHash);

        ui.ShowSubtitleHold(new[]{ "……여긴… 어디지?" });
        yield return new WaitUntil(() => !subtitleUI.IsPlaying);

        camAnimator.SetTrigger(TurnLeftAndStandHash);
        yield return null;
        yield return WaitAnimDone(camAnimator, TurnLeftAndStandHash);
        
        CutsceneCameraManager.Instance.EndCutscene(cinemachineCam, OnComplete);
    }

    private IEnumerator Blink()
    {
        yield return fadeUI.Show();
        yield return fadeUI.Hide();
    }

    private void OnComplete()
    {
        PlayerManager.Instance.ShowPlayerBody(true);
    }
    
    protected override void OnBeforePlay()
    {
        PlayerManager.Instance.ShowPlayerBody(false);
    }

    protected override void OnAfterPlay(){}
}
