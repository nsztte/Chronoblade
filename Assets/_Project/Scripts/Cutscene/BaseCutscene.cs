using UnityEngine;
using System.Collections;

public abstract class BaseCutscene : MonoBehaviour
{
    protected GameManager gm => GameManager.Instance;
    protected UIManager ui => UIManager.Instance;
    protected CutsceneCameraManager cm => CutsceneCameraManager.Instance;
    protected FadeUI fadeUI => UIManager.Instance.FadeUI;
    protected SubtitleUI subtitleUI => UIManager.Instance.SubtitleUI;

    public virtual IEnumerator Play()
    {
        OnBeforePlay();

        gm.EnterCutscene();
        yield return null; // 한 프레임 안정화

        try
        {
            yield return RunSequence(); // 자식 클래스에서 구현
        }
        finally
        {
            OnAfterPlay();
        }
    }

    protected abstract IEnumerator RunSequence();
    protected abstract void OnBeforePlay();
    protected abstract void OnAfterPlay();

    protected IEnumerator WaitAnimDone(Animator a, int stateHash)
    {
        // 해당 상태로 진입할 때까지
        yield return new WaitUntil(() => a.GetCurrentAnimatorStateInfo(0).shortNameHash == stateHash);
        // 클립 끝날 때까지
        yield return new WaitUntil(() =>
        {
            var s = a.GetCurrentAnimatorStateInfo(0);
            return s.shortNameHash == stateHash && s.normalizedTime >= 1f && !a.IsInTransition(0);
        });
    }

    protected void ForceUnscaledAnimators(Animator a)
    {
        a.updateMode = AnimatorUpdateMode.UnscaledTime;
        // a.cullingMode = AnimatorCullingMode.AlwaysAnimate;
    }

    protected void RestoreAnimators(Animator a)
    {
        a.updateMode = AnimatorUpdateMode.Normal;
        // a.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
    }
}
