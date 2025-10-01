using UnityEngine;

public class EnemyPool : Pool<Enemy>
{
    public static EnemyPool Instance { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    protected override void OnBeforeRelease(Enemy enemy)
    {
        base.OnBeforeRelease(enemy);
    }
}
