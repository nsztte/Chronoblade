using System;
using UnityEngine;

[RequireComponent(typeof(SaveId))]
public class BossIntroTrigger : SaveableBehaviour
{
    [SerializeField] BossIntroCutscene bossIntroCutscene;

    [Serializable]
    private class Data
    {
        public bool isActivated;
    }

    public override string CaptureStateJson()
    {
        var data = new Data
        {
            isActivated = !gameObject.activeSelf
        };
        return JsonUtility.ToJson(data);
    }

    public override void RestoreStateJson(string json)
    {
        if (string.IsNullOrEmpty(json)) return;

        var data = JsonUtility.FromJson<Data>(json);
        if (data == null) return;

        // 트리거가 이미 활성화되었다면 비활성화 상태로 복원
        if (data.isActivated)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            bossIntroCutscene.StartPlay();
            
            gameObject.SetActive(false);
        }
    }
}
