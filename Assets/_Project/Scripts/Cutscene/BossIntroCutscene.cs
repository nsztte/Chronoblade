using System.Collections;
using UnityEngine;

public class BossIntroCutscene : BaseCutscene
{
    [Header("카메라")]
    [SerializeField] private GameObject cinemachineCam;

    [Header("보스")]
    [SerializeField] private BossController bossController;   // StartIntroState 호출
    
    private Animator bossAnimator;
    
    static readonly int IntroHash = Animator.StringToHash("Intro");

    private void Awake()
    {
        if(bossController != null) bossAnimator = bossController.Animator;
    }

    protected override IEnumerator RunSequence()
    {
        yield return new WaitUntil(() => !cm.IsBlending());

        bossController.StartIntroState();

        yield return WaitAnimDone(bossAnimator, IntroHash);

        CutsceneCameraManager.Instance.EndCutscene(cinemachineCam);
    }

    protected override void OnBeforePlay()
    {
        if (bossAnimator != null)
            ForceUnscaledAnimators(bossController.Animator);

        PlayerManager.Instance.ShowPlayerBody(false);
        CutsceneCameraManager.Instance.StartCutscene(cinemachineCam);
    }

    protected override void OnAfterPlay()
    {
        if (bossController != null && bossController.Animator != null)
            RestoreAnimators(bossController.Animator);

        PlayerManager.Instance.ShowPlayerBody(true);
    }
}
