using UnityEngine;

public abstract class GameBaseState : ScriptableObject
{
    protected GameManager gameManager;
    private bool initialized;

    public virtual void Init(GameManager gm)
    {
        if (initialized && gameManager == gm) return;
        gameManager = gm;
        initialized = true;
    }

    public abstract void Enter();
    public abstract void Exit();
}
