using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToastController : MonoBehaviour
{
    public static ToastController Instance { get; private set; }

    [SerializeField] private GameObject toastPrefab;
    [SerializeField] private Transform toastParent;
    [SerializeField] private float displayTime = 2.5f;
    [SerializeField] private float fadeDuration = 0.5f;

    private void Awake()
    {
        Instance = this;
    }

    public void Show(string message)
    {
        GameObject toast = Instantiate(toastPrefab, toastParent);
        TMP_Text text = toast.GetComponentInChildren<TMP_Text>();
        CanvasGroup cg = toast.GetComponent<CanvasGroup>();

        text.text = message;
        cg.alpha = 0f;

        LayoutRebuilder.ForceRebuildLayoutImmediate(toast.GetComponent<RectTransform>());

        StartCoroutine(AnimateToast(cg, toast));
    }

    private IEnumerator AnimateToast(CanvasGroup cg, GameObject go)
    {
        // Fade In
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            cg.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }

        cg.alpha = 1f;
        yield return new WaitForSeconds(displayTime);

        // Fade Out
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            cg.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }

        Destroy(go);
    }
}
