using UnityEngine;

public class GunWeaponController : WeaponController
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask hitLayer;
    [SerializeField] private int currentAmmo;

    protected override void Start()
    {
        base.Start();
        currentAmmo = weaponData.magazineSize;
        coolTime = 1f / weaponData.fireRate;
    }

    protected override void Attack()
    {
        if(currentAmmo <= 0)
        {
            Debug.Log("탄약 없음");
            return;
        }

        currentAmmo--;
        Debug.Log($"탄약 사용: {currentAmmo}");

        Ray ray = new Ray(firePoint.position, firePoint.forward);
        if(Physics.Raycast(ray, out RaycastHit hit, weaponData.range, hitLayer))
        {
            if(hit.collider.TryGetComponent(out IDamageable target))
            {
                target.TakeDamage(weaponData.damage);
                Debug.Log($"[총기 타격] 대상: {hit.collider.name}, 데미지: {weaponData.damage}");
            }
        }

        Debug.DrawRay(firePoint.position, firePoint.forward * weaponData.range, Color.yellow, 0.5f);
    }

    /// <summary>
    /// TODO: 탄약 소비 로직 추가
    /// </summary>
    public void Reload()
    {
        currentAmmo = weaponData.magazineSize;
    }
}
