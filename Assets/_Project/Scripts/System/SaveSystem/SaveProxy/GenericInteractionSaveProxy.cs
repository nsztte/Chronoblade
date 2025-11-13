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
        sav = target as IInteractableSavable;
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
}
