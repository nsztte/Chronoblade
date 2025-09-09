using UnityEngine;

public interface IInteractableSavable
{
    // 상태: ex) 문 열림/닫힘, 락 해제 여부, 레버 on/off
    bool IsActivated();

    // 들고 있는 물체라면 true (없으면 false 반환)
    bool IsHeld();

    // 위치/회전 저장이 필요할 때만 값 제공 (필요 없으면 null 의미로 Vector3.zero/Quaternion.identity + 플래그)
    bool TryGetWorldPose(out Vector3 pos, out Quaternion rot);

    // 복원용
    void ApplyActivated(bool activated);
    void ApplyHeld(bool held);
    void ApplyWorldPose(Vector3 pos, Quaternion rot); // 필요할 때만 호출
}
