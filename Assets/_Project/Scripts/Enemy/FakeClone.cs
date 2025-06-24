using UnityEngine;

public class FakeClone : MonoBehaviour, IDamageable
{
    private Enemy enemy;
    private float spawnTime;
    private bool isHit = false;
    
    public void Initialize(Enemy enemy)
    {
        this.enemy = enemy;
        this.spawnTime = Time.time;
    }

    private void Update()
    {
        if(isHit) return;

        if(Time.time - spawnTime > enemy.CloneLifetime)
        {
            DestroyClone();
            return;
        }
    }

    public void TakeDamage(int damage)
    {
        isHit = true;
        DestroyClone();
    }

    private void DestroyClone()
    {
        //TODO: 클론 파괴 효과 추가
        Destroy(gameObject);
    }
}
