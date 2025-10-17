using UnityEngine;

public class VFXPool : Pool<Transform>
{
    protected override void OnBeforeRelease(Transform item)
    {
        if (item == null) return;

        // ParticleSystem 정리
        var psList = item.GetComponentsInChildren<ParticleSystem>(true);
        foreach (var ps in psList)
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}