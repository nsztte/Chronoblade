using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int damage);
    void ApplyKnockback(float force);
}
