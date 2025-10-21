using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public enum SubtitleMode { Auto, Click }
public class SubtitleUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float fadeIn = 0.2f, fadeOut = 0.2f;
    [SerializeField] private float minSec = 1.2f, secPerChar = 0.045f, maxSec = 3.5f;

    private readonly Queue<(string line, SubtitleMode mode)> q = new();
    private bool waiting;
    private bool clicked;
    private float clickCooldown;
    private Coroutine runCo;

    public bool IsPlaying => runCo != null;

    private void Update()
    {
        if (clickCooldown > 0f) clickCooldown -= Time.unscaledDeltaTime;

        if (waiting && clickCooldown <= 0f && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
            clicked = true;
    }

    public void Open()
    {
        if (!gameObject.activeSelf) gameObject.SetActive(true);
        group.alpha = 0f;
        group.blocksRaycasts = false;
        group.interactable   = false;
    }

    public void Close()
    {
        if (runCo != null) StopCoroutine(runCo);
        q.Clear();
        waiting = clicked = false;
        text.text = "";
        group.alpha = 0f;
        group.blocksRaycasts = false;
        group.interactable   = false;
        if (gameObject.activeSelf) gameObject.SetActive(false);
    }

    public void Clear()
    {
        q.Clear();
        StopAllCoroutines();
        waiting = clicked = false;
        group.alpha = 0;
        group.blocksRaycasts = false;
        group.interactable   = false;
        text.text = "";
    }
    
    public void Enqueue( string line, SubtitleMode mode = SubtitleMode.Auto ) => q.Enqueue((line, mode));

    public void Play()
    {
        if (runCo != null) StopCoroutine(runCo);
        runCo = StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        yield return FadeTo(1f, fadeIn);

        while (q.Count > 0)
        {
            var (line, mode) = q.Dequeue();
            text.text = line;

            if (mode == SubtitleMode.Auto)
            {
                float dur = Mathf.Clamp(Mathf.Max(minSec, line.Length * secPerChar), minSec, maxSec);
                float t = 0f;
                waiting = false;
                clicked = false;
                group.blocksRaycasts = false;
                group.interactable   = false;
                while (t < dur && !clicked)
                {
                    t += Time.unscaledDeltaTime;
                    yield return null;
                }
                clickCooldown = 0.12f;
            }
            else
            {
                waiting = true;
                clicked = false;
                group.blocksRaycasts = true;
                group.interactable   = true;

                while (!clicked) yield return null;

                waiting = false;
                group.blocksRaycasts = false;
                group.interactable   = false;
                clickCooldown = 0.12f;
            }
        }

        yield return FadeTo(0f, fadeOut);
        text.text = "";
        runCo = null;
    }

    private IEnumerator FadeTo(float target, float time)
    {
        float start = group.alpha;
        if (Mathf.Approximately(start, target) || time <= 0f)
        {
            group.alpha = target;
            yield break;
        }
        float t = 0f;

        while (t < time)
        { 
            t += Time.unscaledDeltaTime;
            group.alpha = Mathf.Lerp(start, target, t/time);
            yield return null;
        }

        group.alpha = target;
    }
}
