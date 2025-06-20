using UnityEngine;

public class GunWeaponController : WeaponController
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask hitLayer;
    [SerializeField] private int currentAmmo;
    private float nextFireTime = 0f;
    private bool isAiming = false;
    public bool IsAiming => isAiming;

    protected override void Start()
    {
        base.Start();
        currentAmmo = weaponData.magazineSize;

        // 초기 탄약 UI 업데이트
        int totalAmmo = InventoryManager.Instance.GetAmmoCount(weaponData.ammoType);
        UIManager.Instance?.UpdateAmmo(currentAmmo, totalAmmo);
    }

    protected override void RegisterInput()
    {
        base.RegisterInput();
        InputManager.Instance.OnAimStarted += OnAimStarted;
        InputManager.Instance.OnAimCanceled += OnAimCanceled;
        InputManager.Instance.OnReloadPressed += OnReload;
    }

    protected override void UnregisterInput()
    {
        base.UnregisterInput();
        InputManager.Instance.OnAimStarted -= OnAimStarted;
        InputManager.Instance.OnAimCanceled -= OnAimCanceled;
        InputManager.Instance.OnReloadPressed -= OnReload;
    }

    protected override void Attack()
    {
        if(Time.time < nextFireTime) return;

        if(currentAmmo <= 0)
        {
            Debug.Log("탄약 없음");
            //TODO: 탄약 없음 사운드 재생
            return;
        }

        // 총기 쿨타임 설정
        nextFireTime = Time.time + weaponData.fireRate;

        // 탄약 사용
        currentAmmo = Mathf.Max(0, currentAmmo - 1);
        Debug.Log($"탄약 사용: {currentAmmo}");        

        if(weaponData.weaponType == WeaponType.Shotgun)
        {
            FireShotgun();
        }
        else
        {
            FireSingle();
        }

        // 탄약 UI 업데이트
        int totalAmmo = InventoryManager.Instance.GetAmmoCount(weaponData.ammoType);
        UIManager.Instance?.UpdateAmmo(currentAmmo, totalAmmo);
    }

    private void FireSingle()
    {
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
        ApplyWeaponRecoil();
    }

    private void FireShotgun()
    {
        for (int i = 0; i < weaponData.pelletCount; i++)
        {
            // 퍼짐 각도 계산 (예: 5도)
            float spreadAngle = weaponData.spreadAngle;
            // 랜덤한 각도 생성
            float randomYaw = Random.Range(-spreadAngle, spreadAngle);
            float randomPitch = Random.Range(-spreadAngle, spreadAngle);
            // 회전 적용
            Quaternion spreadRotation = Quaternion.Euler(randomPitch, randomYaw, 0);
            Vector3 spreadDirection = spreadRotation * firePoint.forward;

            Ray ray = new Ray(firePoint.position, spreadDirection);
            if (Physics.Raycast(ray, out RaycastHit hit, weaponData.range, hitLayer))
            {
                if (hit.collider.TryGetComponent(out IDamageable target))
                {
                    target.TakeDamage(weaponData.damage);
                    Debug.Log($"[샷건 타격] 대상: {hit.collider.name}, 데미지: {weaponData.damage}");
                }
            }
            Debug.DrawRay(firePoint.position, spreadDirection * weaponData.range, Color.red, 0.5f);
        }
        ApplyWeaponRecoil();
    }

    private void ApplyWeaponRecoil()
    {
        float recoilX = weaponData.recoilX;
        float recoilY = weaponData.recoilY;
        if (isAiming)
        {
            recoilX *= weaponData.aimRecoilMultiplier;
            recoilY *= weaponData.aimRecoilMultiplier;
        }
        CameraController.Instance?.ApplyRecoil(recoilX, Random.Range(-recoilY, recoilY));
    }

    /// <summary>
    /// TODO: 탄약 소비 로직 추가
    /// </summary>
    private void Reload()
    {
        if (currentAmmo >= weaponData.magazineSize)
        {
            return; // 이미 탄창이 가득 찬 경우
        }

        int ammoNeeded = weaponData.magazineSize - currentAmmo;
        int ammoAvailable = InventoryManager.Instance.GetAmmoCount(weaponData.ammoType);

        if (ammoAvailable <= 0)
        {
            Debug.Log("재장전할 탄약이 없습니다.");
            return;
        }

        int ammoToReload = Mathf.Min(ammoNeeded, ammoAvailable);

        // 인벤토리에서 탄약 소비 시도
        if (InventoryManager.Instance.UseAmmo(weaponData.ammoType, ammoToReload))
        {
            currentAmmo += ammoToReload;
            Debug.Log($"탄약 재장전: {ammoToReload}발. 현재 탄약: {currentAmmo}");

            // 탄약 UI 업데이트
            int totalAmmo = InventoryManager.Instance.GetAmmoCount(weaponData.ammoType);
            UIManager.Instance?.UpdateAmmo(currentAmmo, totalAmmo);
        }
        else
        {
            Debug.LogWarning("탄약 소비에 실패했습니다. (InventoryManager.UseAmmo false 반환)");
        }

        // 탄약 재장전 애니메이션 재생
        // 탄약 재장전 사운드 재생
    }

    private void OnReload()
    {
        if(WeaponManager.Instance.CurrentWeapon == this)
        {
            Reload();
        }
    }

    private void OnAimStarted()
    {
        Debug.Log("Aim Started");
        isAiming = true;
    }

    private void OnAimCanceled()
    {
        Debug.Log("Aim Canceled");
        isAiming = false;
    }
}
