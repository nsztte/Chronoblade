using UnityEngine;

public class ComboResultPool : Pool<ComboResultEntry>
{
    protected override void OnBeforeRelease(ComboResultEntry item)
    {
        // 연출 중 DOTween 제거
        item.CleanupBeforeReturn();
    }
}
