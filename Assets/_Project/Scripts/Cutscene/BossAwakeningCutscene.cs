using System.Collections;
using UnityEngine;

public class BossAwakeningCutscene : BaseCutscene
{
    [Header("Cameras")]
    [SerializeField] GameObject cinemachineCam;

    [Header("Animators")]
    [SerializeField] Animator heartAnimator;
    [SerializeField] Animator bossAnimator;

    static readonly int MoveToBossHash = Animator.StringToHash("MoveToBoss");

    public void StartPlay()
    {
        StartCoroutine(Play());
    }

    protected override IEnumerator RunSequence()
    {
        yield return new WaitUntil(() => !cm.IsBlending());

        heartAnimator.Play(MoveToBossHash);

        yield return WaitAnimDone(heartAnimator, MoveToBossHash);

        // yield return new WaitForSecondsRealtime(1.5f);

        bossAnimator.SetTrigger("Idle");

        yield return new WaitForSecondsRealtime(0.5f);

        CutsceneCameraManager.Instance.EndCutscene(cinemachineCam, OnComplete);
    }

    protected override void OnAfterPlay()
    {
        RestoreAnimators(heartAnimator);
        RestoreAnimators(bossAnimator);
    }

    protected override void OnBeforePlay()
    {
        ForceUnscaledAnimators(heartAnimator);
        ForceUnscaledAnimators(bossAnimator);

        PlayerManager.Instance.ShowPlayerBody(false);
        CutsceneCameraManager.Instance.StartCutscene(cinemachineCam);
    }

    private void OnComplete()
    {
        PlayerManager.Instance.ShowPlayerBody(true);
    }
}
