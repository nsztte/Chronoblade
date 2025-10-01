using UnityEngine;

public class EnemyPool : Pool<Enemy>
{
    protected override void OnBeforeRelease(Enemy enemy)
    {
        base.OnBeforeRelease(enemy);
    }
}
