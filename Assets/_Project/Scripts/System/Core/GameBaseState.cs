using UnityEngine;

public abstract class GameBaseState : MonoBehaviour
{
    protected GameManager gameManager;

    public virtual void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public abstract void Enter();
    public abstract void Exit();
}
