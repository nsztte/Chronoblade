using System;
using UnityEngine;

[Serializable]
public struct RewindSnapshot
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;

    public RewindSnapshot(Vector3 position, Quaternion rotation, Vector3 velocity)
    {
        this.position = position;
        this.rotation = rotation;
        this.velocity = velocity;
    }
}
