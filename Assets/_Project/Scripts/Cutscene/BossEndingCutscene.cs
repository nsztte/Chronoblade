using System.Collections;
using UnityEngine;

public class BossEndingCutscene : BaseCutscene
{
    [Header("카메라")]
    [SerializeField] private GameObject cinemachineCam;

    [Header("보스")]
    [SerializeField] private BossController bossController;
    
    private Animator bossAnimator;
    private Animator heartAnimator;

    static readonly int EndingHash = Animator.StringToHash("Ending");
    static readonly int HeartEndingHash = Animator.StringToHash("HeartEnding");
    static readonly int Ending2Hash = Animator.StringToHash("Ending2");

    private void Start()
    {
        if(bossController != null) 
        {
            bossAnimator = bossController.Animator;
            heartAnimator = bossController.HeartAnimator;
        }
    }

    public void StartPlay()
    {
        StartCoroutine(Play());
    }

    protected override IEnumerator RunSequence()
    {
        yield return new WaitUntil(() => !cm.IsBlending());

        bossAnimator.Play(EndingHash);

        yield return WaitAnimDone(bossAnimator, EndingHash);

        heartAnimator.Play(HeartEndingHash);

        yield return WaitAnimDone(heartAnimator, HeartEndingHash);

        bossAnimator.Play(Ending2Hash);
        yield return WaitAnimDone(bossAnimator, Ending2Hash);

        ui.ShowSubtitleHold(new []{
            "시간의 신전이 멈췄다.",
            "그 속에서 모든 소리가 사라졌다.",
            "남은 것은, 흐름을 거스른 자의 흔적뿐이었다.",
            "나는 이 흐름 속에, 영원히 머물게 될 것을 직감했다."
        });

        yield return new WaitUntil(() => !subtitleUI.IsPlaying);

        fadeUI.ShowBlackScreen();

        yield return new WaitForSecondsRealtime(1.5f);

        GameManager.Instance.EnterEnding();
    }

    protected override void OnBeforePlay()
    {
        ForceUnscaledAnimators(bossController.Animator);
        ForceUnscaledAnimators(heartAnimator);

        CutsceneCameraManager.Instance.StartCutscene(cinemachineCam);
    }

    protected override void OnAfterPlay()
    {
        RestoreAnimators(bossController.Animator);
        RestoreAnimators(heartAnimator);
    }
}
