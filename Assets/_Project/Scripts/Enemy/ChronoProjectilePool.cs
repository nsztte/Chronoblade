
public class ChronoProjectilePool : Pool<ChronoProjectile>
{
    public static ChronoProjectilePool Instance { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    protected override void OnBeforeRelease(ChronoProjectile projectile)
    {
        base.OnBeforeRelease(projectile);
    }
}
