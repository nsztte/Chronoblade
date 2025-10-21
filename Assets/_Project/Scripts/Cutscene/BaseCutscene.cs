using UnityEngine;
using System.Collections;

public abstract class BaseCutscene : MonoBehaviour
{
    protected GameManager gm => GameManager.Instance;
    protected UIManager ui => UIManager.Instance;
    protected CutsceneCameraManager cam => CutsceneCameraManager.Instance;
    protected FadeUI fadeUI => UIManager.Instance.FadeUI;
    protected SubtitleUI subtitleUI => UIManager.Instance.SubtitleUI;

    public virtual IEnumerator Play()
    {
        gm.EnterCutscene();
        yield return null; // 한 프레임 안정화

        yield return RunSequence(); // 자식 클래스에서 구현

        gm.EnterExploration();
    }

    protected abstract IEnumerator RunSequence();
}
