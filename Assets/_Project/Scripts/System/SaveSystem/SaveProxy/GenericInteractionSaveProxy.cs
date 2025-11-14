using System;
using UnityEngine;

public class GenericInteractionSaveProxy : SaveableBehaviour
{
    [Serializable]
    class Data
    {
        public bool activated;
        public bool held;
        public bool hasPose;
        public Vector3 pos;
        public Quaternion rot;
    }

    [SerializeField] private MonoBehaviour target;
    private IInteractableSavable sav;

    void Awake()
    {
        // 에디터에서 수동/자동 세팅된 target 기준으로 한 번 더 보정
        if (target == null || target is not IInteractableSavable)
        {
            AutoAssignTarget();
        }

        sav = target as IInteractableSavable;
        if (sav == null)
        {
            Debug.LogWarning($"[GenericInteractionSaveProxy] {name} 에서 IInteractableSavable 대상(target)을 찾지 못했습니다.");
        }
    }

    public override string CaptureStateJson()
    {
        if (sav == null) return null;

        var d = new Data {
            activated = sav.IsActivated(),
            held = sav.IsHeld()
        };

        if (sav.TryGetWorldPose(out d.pos, out d.rot)) d.hasPose = true;
        return JsonUtility.ToJson(d);
    }

    public override void RestoreStateJson(string json)
    {
        if (sav == null || string.IsNullOrEmpty(json)) return;
        var d = JsonUtility.FromJson<Data>(json);

        // 포즈
        if (d.hasPose)
            sav.ApplyWorldPose(d.pos, d.rot);

        // 상태/홀드
        sav.ApplyActivated(d.activated);
        sav.ApplyHeld(d.held);
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (Application.isPlaying) return;

        // 에디터에서 target이 비었거나 잘못 연결됐으면 자동 할당
        if (target == null || target is not IInteractableSavable)
        {
            AutoAssignTarget();

            // 에디터에 변경사항 반영
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
    #endif

    private void AutoAssignTarget()
    {
        // 자기 자신을 제외한 MonoBehaviour 중 IInteractableSavable 구현체 찾기
        var behaviours = GetComponents<MonoBehaviour>();
        foreach (var b in behaviours)
        {
            if (b == this) continue;
            if (b is IInteractableSavable)
            {
                target = b;
                break;
            }
        }
    }
}
