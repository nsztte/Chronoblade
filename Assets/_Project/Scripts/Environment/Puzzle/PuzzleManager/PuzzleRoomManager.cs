using UnityEngine;
using System.Collections.Generic;

public abstract class PuzzleRoomManager : MonoBehaviour
{    
    [Header("기본 설정")]
    [SerializeField] protected int roomId;
    [SerializeField] protected GameObject clearedPortal;
    [SerializeField] protected GameObject clearedReward;

    [Header("로드 설정")]
    [SerializeField] private bool forceEntranceOnLoad = true;
    [SerializeField] private Transform entrance;

    private Dictionary<Transform, TransformSnapshot> initialStates = new();

    protected bool isCleared = false;
    public bool IsCleared => isCleared;
    protected bool isActivated = false;
    public bool IsActivated => isActivated;
    
    [System.Serializable]
    private struct TransformSnapshot
    {
        public Vector3 localPosition;
        public Quaternion localRotation;
        public bool isActive;

        public TransformSnapshot(Transform tf)
        {
            localPosition = tf.localPosition;
            localRotation = tf.localRotation;
            isActive = tf.gameObject.activeSelf;
        }

        public void ApplyTo(Transform tf)
        {
            tf.localPosition = localPosition;
            tf.localRotation = localRotation;
            tf.gameObject.SetActive(isActive);
        }
    }

    protected abstract void CheckPuzzle();

    protected void OnPuzzleSolved()
    {
        if (clearedPortal) clearedPortal.SetActive(true);
        if (clearedReward) clearedReward.SetActive(true);
        
        PuzzleProgressManager.Instance.MarkCleared(roomId);
        isCleared = true;

        SaveGuard.Instance?.Unblock(SaveBlockTag.Puzzle);
        SaveManager.Instance?.Save(SaveManager.Instance.NextAutoSlot(), SaveIntent.Auto);
    }

    public void ChangeState(bool isActive)
    {
        isActivated = isActive;
    }

    public void ResetToInitialIfUncleared()
    {
        if (IsCleared) return;

        RestoreInitialStates();
        
        if (forceEntranceOnLoad && entrance != null)
        {
            Vector3 position = entrance.position;
            Quaternion rotation = entrance.rotation;

            PlayerManager.Instance?.PlayerController?.SetPositionAndRotaion(position, rotation);
        }
    }

    public void CacheInitialStates()
    {
        initialStates.Clear();
        foreach (var tf in GetComponentsInChildren<Transform>(true))
        {
            initialStates[tf] = new TransformSnapshot(tf);
        }
    }

    private void RestoreInitialStates()
    {
        foreach (var pair in initialStates)
        {
            pair.Value.ApplyTo(pair.Key);
        }
    }
}
