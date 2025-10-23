using System.Collections;
using UnityEngine;

public class BossPhaseTransitionCutscene : BaseCutscene
{
    [Header("카메라")]
    [SerializeField] private GameObject cinemachineCam;

    [Header("보스")]
    [SerializeField] private BossController bossController;

    [Header("VFX")]
    [SerializeField] private ParticleSystem[] vfxOnStart;

    private Animator bossAnimator;
    
    private void Start()
    {
        if(bossController != null) bossAnimator = bossController.Animator;
    }

    public void StartPlay()
    {
        StartCoroutine(Play());
    }

    protected override IEnumerator RunSequence()
    {
        yield return new WaitUntil(() => !cm.IsBlending());

        bossController.StartPhaseTransitionState();

        if (vfxOnStart != null)
            foreach (var v in vfxOnStart) if (v != null) v.Play();

        yield return new WaitForSecondsRealtime(bossController.GetCurrentAnimationLength() + 1f);

        CutsceneCameraManager.Instance.EndCutscene(cinemachineCam);
    }

    protected override void OnBeforePlay()
    {
        ForceUnscaledAnimators(bossController.Animator);

        CutsceneCameraManager.Instance.StartCutscene(cinemachineCam);
    }

    protected override void OnAfterPlay()
    {
        RestoreAnimators(bossController.Animator);

        if (vfxOnStart != null)
            foreach (var v in vfxOnStart) if (v != null) v.Stop();
    }
}
