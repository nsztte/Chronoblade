using UnityEngine;
using DG.Tweening;

public class ComboResultUIController : MonoBehaviour
{
    [Header("콤보 텍스트 풀 및 스택 위치")]
    [SerializeField] private ComboResultPool resultPool;
    [SerializeField] private RectTransform comboStack;

    [Header("스택 이동 간격")]
    [SerializeField] private float floatUpDistance = 40f;
    [SerializeField] private float floatUpDuration = 0.25f;

    [Header("자동 반환 대기 시간")]
    [SerializeField] private float autoReleaseDelay = 1.2f;

    public void ShowResult(string text, Color color)
    {
        var entry = resultPool.Get();
        if (entry == null)
        {
            Debug.LogWarning("[ComboResultUI] Entry를 가져올 수 없습니다.");
            return;
        }

        // 스택에 추가
        entry.transform.SetParent(comboStack, false);
        entry.transform.SetAsLastSibling(); // 위로 쌓이게

        // 텍스트/색상 설정 및 애니메이션 실행
        entry.Play(text, color);

        // 기존 항목 float-up 이동
        foreach (Transform child in comboStack)
        {
            if (child == entry.transform) continue;

            child.DOLocalMoveY(child.localPosition.y + floatUpDistance, floatUpDuration)
                  .SetEase(Ease.OutCubic);
        }

        // 일정 시간 후 자동 반환
        resultPool.ReleaseAfter(entry, autoReleaseDelay);
    }
}
