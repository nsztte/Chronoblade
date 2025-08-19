using UnityEngine;

public class BossAltar : MonoBehaviour
{
    [SerializeField] private Transform[] keySockets;
    [SerializeField] private GameObject bossGateToOpen;

    private int inserted = 0;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var held = PlayerManager.Instance?.CurrentHeldObject;
        if (held == null) return;

        if(held.TryGetComponent(out BossKeyPickup key) && !key.IsInserted)
        {
            key.CanInsert = true;
            key.insert = () => InsertKey(key);
        }
    }

    private void InsertKey(BossKeyPickup key)
    {
        int i = Mathf.Clamp(key.SlotIndex, 0, keySockets.Length - 1);
        var socket = keySockets[i];

        if (key.IsInserted) return;
        if(socket.childCount > 0) return;

        key.InsertToSocket(socket);

        inserted++;

        PuzzleProgressManager.Instance.ReportKeyInserted(inserted);

        if (inserted >= keySockets.Length && bossGateToOpen)
        {
            // 문 열림 연출
            bossGateToOpen.SetActive(true);
        }
    }
}
