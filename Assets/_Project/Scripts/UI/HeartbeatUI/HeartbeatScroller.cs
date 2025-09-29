using UnityEngine;
using System.Collections.Generic;

public class HeartbeatScroller : MonoBehaviour
{
    [Header("구성 요소")]
    [SerializeField] private RectTransform panel; // HeartbeatUI_Panel
    [SerializeField] private RectTransform heartbeatImagesParent; // HeartbeatImages
    [SerializeField] private HeartbeatLinePool linePool;

    [Header("이동 관련")]
    [SerializeField] private int lineCount = 3;
    private List<HeartbeatLineImage> activeLines = new List<HeartbeatLineImage>();
    private float scrollSpeed; // px/sec
    private float panelWidth;
    private float panelCenterX;
    private float imageWidth;
    private bool isScrolling = false;

    private void OnEnable()
    {
        panelWidth = panel.rect.width;
        panelCenterX = panelWidth * 0.5f;

        float beatInterval = TimingComboManager.Instance.BeatInterval;

        for (int i = 0; i < lineCount; i++)
        {
            var lineRT = linePool.Get();
            lineRT.SetParent(heartbeatImagesParent, false);
            var line = lineRT.GetComponent<HeartbeatLineImage>();

            if (i == 0)
            {
                imageWidth = line.RectTransform.rect.width;
                float distance = panelCenterX + imageWidth * 0.5f;
                scrollSpeed = distance / beatInterval;
            }

            // 이미지 자체 중심 기준으로 간격 배치
            float startX = i * imageWidth;
            line.RectTransform.anchoredPosition = new Vector2(startX, 0f);

            activeLines.Add(line);
        }
        
        if (TimingComboManager.Instance != null)
            TimingComboManager.Instance.OnBeat += OnBeat;
    }

    private void OnDisable()
    {
        foreach (var line in activeLines)
        {
            if (line != null)
                linePool.Release(line.RectTransform);
        }
        activeLines.Clear();

        if (TimingComboManager.Instance != null)
            TimingComboManager.Instance.OnBeat -= OnBeat;
    }

    private void Update()
    {
        if (!isScrolling) return;

        foreach (var line in activeLines)
        {
            var rt = line.RectTransform;
            rt.anchoredPosition += new Vector2(scrollSpeed * Time.deltaTime, 0f);

            // 이미지 중심이 패널 오른쪽을 넘으면 왼쪽으로 되돌림
            if (rt.anchoredPosition.x > panelCenterX + panelWidth * 0.5f)
            {
                float leftMostX = GetLeftMostImageX();
                rt.anchoredPosition = new Vector2(leftMostX - imageWidth, 0f);
            }
        }
    }

    private float GetLeftMostImageX()
    {
        float min = float.MaxValue;
        foreach (var line in activeLines)
        {
            float x = line.RectTransform.anchoredPosition.x;
            if (x < min)
                min = x;
        }
        return min;
    }

    private void OnBeat()
    {
        // 가장 중앙에 가까운 이미지 찾기
        HeartbeatLineImage closest = null;
        float closestDistance = float.MaxValue;

        foreach (var line in activeLines)
        {
            float dist = Mathf.Abs(line.RectTransform.anchoredPosition.x - panelCenterX);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closest = line;
            }
        }

        if (closest != null)
        {
            closest.Flash(1f, 0.3f, 0.2f); // alpha=1 → 0.3으로 0.2초간 페이드
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        isScrolling = true;
    }

    public void Hide()
    {
        isScrolling = false;
        gameObject.SetActive(false);
    }
}
