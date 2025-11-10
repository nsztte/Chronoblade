using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LightPathController : MonoBehaviour
{
    public enum LightPathMode { FanoutOnce, GuideOnUnlock }

    [Header("기본 정보")]
    [SerializeField] private LightPathMode mode = LightPathMode.FanoutOnce;
    [SerializeField] private int roomId;

    [Header("타이밍")]
    [SerializeField] private float fanoutDelay = 0.6f;
    [SerializeField] private float fanoutDuration = 2.0f;

    private LineRenderer lr;
    private Coroutine co;

    // 현재 켜져 있는 가이드(전역 1개만 유지)
    private static LightPathController currentGuide;

    public void Hide() { SetVisible(false); }

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
    }

    private void OnEnable()
    {
        var ppm = PuzzleProgressManager.Instance;
        if (ppm != null) ppm.OnRoomUnlocked += HandleRoomUnlocked;

        if (mode == LightPathMode.FanoutOnce)
        {
            if (co != null) StopCoroutine(co);
            co = StartCoroutine(CoFanout());
        }
    }

    private void OnDisable()
    {
        var ppm = PuzzleProgressManager.Instance;
        if (ppm != null) ppm.OnRoomUnlocked -= HandleRoomUnlocked;

        if (co != null) StopCoroutine(co);
    }

    private void HandleRoomUnlocked(int unlockedRoomId)
    {
        if (mode != LightPathMode.GuideOnUnlock) return;
        if (unlockedRoomId != roomId) return;

        // 새 가이드 시작 → 기존 가이드 끄고 나만 켠다
        if (currentGuide != null && currentGuide != this)
            currentGuide.SetVisible(false);

        SetVisible(true);
        currentGuide = this;
    }

    private IEnumerator CoFanout()
    {
        yield return new WaitForSeconds(fanoutDelay);
        SetVisible(true);
        yield return new WaitForSeconds(fanoutDuration);
        SetVisible(false);
        co = null;
    }

    private void SetVisible(bool on)
    {
        if (lr != null) lr.enabled = on;
    }
}
