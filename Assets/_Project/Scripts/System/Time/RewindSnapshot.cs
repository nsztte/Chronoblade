using System;
using UnityEngine;

[Serializable]
public struct RewindSnapshot
{
    public Vector3 position;
    public Quaternion rotation;

    public RewindSnapshot(Vector3 position, Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }
}
