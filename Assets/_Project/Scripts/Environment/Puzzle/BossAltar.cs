using UnityEngine;

public class BossAltar : MonoBehaviour
{
    [SerializeField] private GameObject[] keySockets;
    [SerializeField] private GameObject bossGateToOpen;
    [SerializeField] private GameObject lastPart;
    [SerializeField] private BossAwakeningCutscene cm;
 
    [SerializeField] private int activated = 0;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        
        if(PuzzleProgressManager.Instance != null)
            PuzzleProgressManager.Instance.OnAllCleared += OnAllCleared;
    }

    private void OnDestroy()
    {
        if(PuzzleProgressManager.Instance != null)
            PuzzleProgressManager.Instance.OnAllCleared -= OnAllCleared;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var held = PlayerManager.Instance?.CurrentHeldObject;
        if (held == null) return;

        if(held.TryGetComponent(out BossKeyPickup key) && ((!key.IsActivated())))
        {
            key.CanActive = true;
            key.insert = () => InsertKey(key);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        var held = PlayerManager.Instance?.CurrentHeldObject;
        if (held != null && held.TryGetComponent(out BossKeyPickup key))
        {
            key.CanActive = false;
            key.insert = null;
        }
    }

    public int InsertedKeyCount => activated;
    public int SlotCount => keySockets != null ? keySockets.Length : 0;

    // 현재 슬롯 삽입 상태를 얻기 (소켓 활성 여부 기준)
    public void GetSlotInsertedSnapshot(bool[] dst)
    {
        if (dst == null || keySockets == null) return;
        for (int i = 0; i < dst.Length && i < keySockets.Length; i++)
            dst[i] = keySockets[i] != null && keySockets[i].activeSelf;
    }

    // 복원: 이벤트/컷씬 발행 없이 상태만 맞춤
    public void ApplyStateOnly(int insertedKeyCount, bool[] slotInserted)
    {
        if (keySockets == null) return;
        insertedKeyCount = Mathf.Clamp(insertedKeyCount, 0, keySockets.Length);

        // 슬롯 비주얼/콜라이더 동기화
        int count = 0;
        for (int i = 0; i < keySockets.Length; i++)
        {
            bool on = (slotInserted != null && i < slotInserted.Length) ? slotInserted[i] : false;
            if (keySockets[i] != null)
                keySockets[i].SetActive(on);
            if (on) count++;
        }

        activated = count; // 실제 카운트로 동기화

        // 모든 키가 꽂힌 상태면 보스문 상태만 즉시 동기화
        if (activated >= keySockets.Length)
        {
            if (bossGateToOpen) bossGateToOpen.SetActive(false);    // 게이트/콜라이더 오프
            if (lastPart) lastPart.SetActive(true);                 // 마지막 파트 비주얼 온
            // animator나 cm.StartPlay() 등 연출은 호출하지 않음
        }
    }

    private void InsertKey(BossKeyPickup key)
    {
        int i = Mathf.Clamp(key.SlotIndex, 0, keySockets.Length - 1);
        var socket = keySockets[i];

        if (key.IsActivated()) return;
        if (socket.gameObject.activeSelf) return;

        key.ActivateSocket(socket);

        activated++;

        PuzzleProgressManager.Instance.ReportKeyInserted(activated, keySockets.Length);

        if (activated >= keySockets.Length && bossGateToOpen)
        {
            bossGateToOpen.SetActive(false);    // 콜라이더 제거
            cm.StartPlay();
        }
    }

    private void OnAllCleared()
    {
        if (lastPart) lastPart.SetActive(true);
        if (animator) animator.SetTrigger("Active");

        // 그 외 연출
    }
}
