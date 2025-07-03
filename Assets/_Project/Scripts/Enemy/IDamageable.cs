using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int damage);
    void ApplyKnockback(Vector3 direction, float force);
}
