using UnityEngine;

public class FakeClonePool : Pool<FakeClone>
{
    public static FakeClonePool Instance { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    protected override void OnBeforeRelease(FakeClone clone)
    {
        base.OnBeforeRelease(clone);
    }
}
