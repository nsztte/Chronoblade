using System.Linq;
using UnityEditor.Rendering.Universal;
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
