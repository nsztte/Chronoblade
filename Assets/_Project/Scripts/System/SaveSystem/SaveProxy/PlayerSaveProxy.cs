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
    }

    private PlayerManager Player => PlayerManager.Instance;
    private Transform Body => Player.PlayerTransform;

    public override string CaptureStateJson()
    {
        var d = new Data
        {
            pos = new[] { Body.position.x, Body.position.y, Body.position.z },
            yaw = Body.eulerAngles.y,
            hp  = Player.CurrentHP,
            mp  = Player.CurrentMP,
            gold = Player.Gold,
            heldObject = Player.CurrentHeldObject ? Player.CurrentHeldObject.name : "None"
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
    }
}
