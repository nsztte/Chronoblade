using UnityEngine;

public class WeaknessHitbox : MonoBehaviour, IDamageable
{
    [SerializeField] private BossController boss;
    [SerializeField] private float damageMultiplier = 1.5f;

    public void TakeDamage(int damage)
    {
        if(boss == null)
        {
            Debug.LogError("BossController가 할당되지 않았습니다.");
            return;
        }

        int finalDamage = Mathf.RoundToInt(damage * damageMultiplier);
        boss.TakeDamage(finalDamage);

        Debug.Log($"취약점 히트박스 피격 - 초기 데미지: {damage}, 최종 데미지: {finalDamage}");
    }

    public void ApplyKnockback(float force)
    {
    }
}
