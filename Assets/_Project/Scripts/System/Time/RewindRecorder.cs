using System.Collections.Generic;
using UnityEngine;

public class RewindRecorder : MonoBehaviour, IRewindable
{
    [Header("리와인드 기록 설정")]
    [SerializeField] private float recordDuration = 3f;
    [SerializeField] private float recordInterval = 0.05f;

    private List<RewindSnapshot> snapshots = new List<RewindSnapshot>();
    private float timeSinceLastRecord = 0f;
    private bool isRewinding = false;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        TimeManager.Instance?.RegisterRewindable(this);
    }

    private void OnDisable()
    {
        TimeManager.Instance?.UnregisterRewindable(this);
    }

    private void Update()
    {
        if(isRewinding)
        {
            PlayRewind();
        }
        else
        {
            timeSinceLastRecord += Time.deltaTime;
            while (timeSinceLastRecord >= recordInterval)
            {
                RecordSnapshot();
                timeSinceLastRecord = 0f;
            }
        }
    }

    public void StartRewind()
    {
        isRewinding = true;
        if(rb != null)
        {
            rb.isKinematic = false;
        }
    }

    public void StopRewind()
    {
        isRewinding = false;
        if(rb != null)
        {
            rb.isKinematic = false;
        }
    }

    public void ApplySnapshot(RewindSnapshot snapshot)
    {
        transform.position = snapshot.position;
        transform.rotation = snapshot.rotation;
    }

    public void RecordSnapshot()
    {
        if(snapshots.Count >= recordDuration / recordInterval)
        {
            snapshots.RemoveAt(0);  // FIFO
        }

        snapshots.Add(new RewindSnapshot(transform.position, transform.rotation));
    }

    private void PlayRewind()
    {
        if(snapshots.Count > 0)
        {
            RewindSnapshot snapshot = snapshots[^1];
            ApplySnapshot(snapshot);
            snapshots.RemoveAt(snapshots.Count - 1);
        }
        else
        {
            rb.isKinematic = true;
        }
    }
}
