using UnityEngine;
using System.Collections;

public class TitleUI : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private CanvasGroup titleCanvasGroup;
    [SerializeField] private CanvasGroup pressAnyKeyGroup;
    [SerializeField] private float blinkSpeed = 2f;
    [SerializeField] private float changeDuration = 0.5f;

    private bool started = false;
    private Coroutine fadeRoutine;

    private void Awake()
    {
        if (!titleCanvasGroup) titleCanvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        // 입력 비활성화
        InputManager.Instance?.SetInputEnabled(false);

        // 타이틀 진입 상태
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 메인메뉴 비활성 + 알파 0 초기화
        if (mainMenuUI)
        {
            var mmcg = mainMenuUI.GetComponent<CanvasGroup>();
            if (!mmcg) mmcg = mainMenuUI.AddComponent<CanvasGroup>();
            mmcg.alpha = 0f;
            mmcg.interactable = false;
            mmcg.blocksRaycasts = false;
            mainMenuUI.SetActive(false);
        }

        if (InputManager.Instance != null)
            InputManager.Instance.OnPressAnyKey += OnStartPressed;
    }

    private void OnDisable()
    {
        if(InputManager.Instance != null)
            InputManager.Instance.OnPressAnyKey -= OnStartPressed;
    }

    private void Update()
    {
        if (!started && pressAnyKeyGroup != null)
            pressAnyKeyGroup.alpha = (Mathf.Sin(Time.time * blinkSpeed) + 1f) * 0.5f;
    }

    private void OnStartPressed()
    {
        if (started) return;
        started = true;

        // Blink 정지 고정
        if (pressAnyKeyGroup) pressAnyKeyGroup.alpha = 1f;

        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(HideTitleAndShowMain());
    }

    private IEnumerator HideTitleAndShowMain()
    {
        // 타이틀은 곧바로 입력 차단
        titleCanvasGroup.interactable = false;
        titleCanvasGroup.blocksRaycasts = false;

        // 타이틀 페이드아웃
        yield return FadeTo(titleCanvasGroup, 0f, changeDuration, setActiveFalse:true);

        // 메인메뉴 활성화 & 알파 0
        mainMenuUI.SetActive(true);
        var mainMenuCanvasGroup = mainMenuUI.GetComponent<CanvasGroup>();
        mainMenuCanvasGroup.alpha = 0f;
        mainMenuCanvasGroup.interactable = false;
        mainMenuCanvasGroup.blocksRaycasts = false;

        // 메인메뉴 페이드인
        yield return FadeTo(mainMenuCanvasGroup, 1f, changeDuration, setActiveFalse:false);

        // 메인메뉴 입력 허용
        mainMenuCanvasGroup.interactable = true;
        mainMenuCanvasGroup.blocksRaycasts = true;
    }

    private IEnumerator FadeTo(CanvasGroup canvasGroup, float targetAlpha, float duration, bool setActiveFalse)
    {
        float start = canvasGroup.alpha;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / duration;
            canvasGroup.alpha = Mathf.Lerp(start, targetAlpha, t);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;

        if (Mathf.Approximately(targetAlpha, 0f) && setActiveFalse)
            canvasGroup.gameObject.SetActive(false);
    }
}