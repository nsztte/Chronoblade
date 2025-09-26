using UnityEngine;

public class HeartbeatLinePool : Pool<RectTransform>
{
    protected override void OnBeforeRelease(RectTransform item)
    {
        // 도트 이펙트 정리, 깜빡임 제거 등
        base.OnBeforeRelease(item);
    }
}
