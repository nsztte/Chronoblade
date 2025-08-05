using UnityEngine;

public interface IRewindable
{
    void StartRewind();
    void StopRewind();
    // void ApplySnapshot(RewindSnapshot snapshot);
    // void RecordSnapshot();
}
