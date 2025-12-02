using System;
using UnityEngine;

public class PlayerSaveProxy : SaveableBehaviour
{
    [Serializable]
    private class Data
    {
        public float[] pos;  // [x, y, z]
        public float yaw;    // y축 회전
        public float hp;
        public float mp;
        public int gold;
        public string heldObject;

        // 어빌리티 해금 상태
        public bool canDash;
        public bool unlockedSlow;
        public bool unlockedStop;
        public bool unlockedRewind;
        public bool unlockedFastForward;
    }

    private PlayerManager Player => PlayerManager.Instance;
    private Transform Body => Player.PlayerTransform;

    public override string CaptureStateJson()
    {
        var time = TimeManager.Instance;

        var d = new Data
        {
            pos = new[] { Body.position.x, Body.position.y, Body.position.z },
            yaw = Body.eulerAngles.y,
            hp  = Player.CurrentHP,
            mp  = Player.CurrentMP,
            gold = Player.Gold,
            heldObject = Player.CurrentHeldObject ? Player.CurrentHeldObject.name : "None",

            // 어빌리티 해금 상태
            canDash = Player.CanDash,
            unlockedSlow = time != null && time.UnlockedSlow,
            unlockedStop = time != null && time.UnlockedStop,
            unlockedRewind = time != null && time.UnlockedRewind,
            unlockedFastForward = time != null && time.UnlockedFastForward
        };
        return JsonUtility.ToJson(d);
    }

    public override void RestoreStateJson(string json)
    {
        if (string.IsNullOrEmpty(json)) return;
        var d = JsonUtility.FromJson<Data>(json);

        // 1) 위치/회전 복원
        Vector3 position = new Vector3(d.pos[0], d.pos[1], d.pos[2]);
        Quaternion rotation = Quaternion.Euler(0f, d.yaw, 0f);

        Player.PlayerController.SetPositionAndRotation(position, rotation);

        // 2) HP/MP 복원 (UI 동기화 포함)
        Player.SetHP(d.hp, true);
        Player.SetMP(d.mp, true);
        Player.AddGold(d.gold - Player.Gold);

        // 3) heldObject 복원
        if (d.heldObject != "None")
        {
            var obj = GameObject.Find(d.heldObject);
            if (obj != null)
                Player.SetHeldObject(obj);
        }

        // 4) 어빌리티 해금 상태 복원
        var time = TimeManager.Instance;

        // 기본 리셋
        Player.LockDash();
        if (time != null)
        {
            time.SetUnlockedStates(false, false, false, false);
        }

        // 세이브된 값 적용
        if (d.canDash)
            Player.UnlockDash();

        if (time != null)
        {
            if (d.unlockedSlow)
                time.UnlockTimeSkill(TimeState.Slow);
            if (d.unlockedStop)
                time.UnlockTimeSkill(TimeState.Stop);
            if (d.unlockedRewind)
                time.UnlockTimeSkill(TimeState.Rewind);
            if (d.unlockedFastForward)
                time.UnlockTimeSkill(TimeState.FastForward);
        }
    }
}
