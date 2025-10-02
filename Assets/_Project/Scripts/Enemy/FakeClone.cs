using UnityEngine;

public class FakeClone : MonoBehaviour, IDamageable
{
    private MirrorDuelist enemy;
    private float spawnTime;
    private bool isHit = false;
    private bool isReleased = false;
    
    public void Initialize(MirrorDuelist enemy)
    {
        this.enemy = enemy;
        spawnTime = Time.time;
        isHit = false;
        isReleased = false;
    }

    private void Update()
    {
        if(isHit) return;

        if(Time.time - spawnTime > enemy.CloneLifetime)
        {
            ReleaseClone();
            return;
        }
    }

    public void TakeDamage(int damage)
    {
        isHit = true;
        ReleaseClone();
    }

    private void ReleaseClone()
    {
        if (isReleased) return;
        isReleased = true;

        //TODO: 클론 파괴 효과 추가
        enemy.UnregisterClone(this);
        FakeClonePool.Instance?.Release(this);
    }

    public void ApplyKnockback(float force) {}
}
