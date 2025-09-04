using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class GunWeaponController : WeaponController
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask hitLayer;
    [SerializeField] private int currentAmmo = -1;
    public int CurrentAmmo => currentAmmo;
    [SerializeField] private float reloadDuration = 1f;
    private float nextFireTime = 0f;
    private bool isAiming = false;
    public bool IsAiming => isAiming;
    public bool isReloading = false;
    private Vector3 originPosition;
    [SerializeField] private Vector3 adsPosition = new Vector3(0f, 0f, 0.2f);
    [SerializeField] private float aimMoveSpeed = 10f;
    private Vector3 currentTargetPosition;

    private Animator animator;

    [Header("적 감지")]
    private float checkInterval = 0.1f;
    private float checkTimer = 0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // currentAmmo = weaponData.magazineSize;
        originPosition = transform.localPosition;
        currentTargetPosition = originPosition;

        int totalAmmo = InventoryManager.Instance.GetAmmoCount(weaponData.ammoType);
        UIManager.Instance?.UpdateAmmo(currentAmmo, totalAmmo);
    }

    private void OnEnable()
    {
        InputManager.Instance.OnAimStarted += OnAimStarted;
        InputManager.Instance.OnAimCanceled += OnAimCanceled;
        InputManager.Instance.OnReloadPressed += OnReload;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnAimStarted -= OnAimStarted;
        InputManager.Instance.OnAimCanceled -= OnAimCanceled;
        InputManager.Instance.OnReloadPressed -= OnReload;
    }

    private void Update()
    {
        UpdateWeaponPosition();

        checkTimer += Time.deltaTime;
        if (checkTimer >= checkInterval)
        {
            checkTimer = 0f;
            CheckEnemyInSight();
        }
    }

    public void UpdateAmmoCount()
    {
        int totalAmmo = InventoryManager.Instance.GetAmmoCount(weaponData.ammoType);
        UIManager.Instance?.UpdateAmmo(currentAmmo, totalAmmo);
    }

    public int GetCurrentAmmoCount()
    {
        return currentAmmo;
    }

    // 세이브/로드 연동용
    public void SetCurrentAmmo(int value)
    {
        int max = weaponData.magazineSize;
        currentAmmo = Mathf.Clamp(value, 0, max);
    }

    public override void ExecuteWeaponAttack()
    {
        if(!gameObject.activeInHierarchy) return;
        if(isReloading)
        {
            Debug.Log("재장전 중");
            return;
        }
        if(Time.time < nextFireTime)
        {
            Debug.Log("총기 쿨타임 중");
            return;
        }
        if(currentAmmo <= 0)
        {
            Debug.Log("탄약 없음");
            //TODO: 탄약 없음 사운드 재생
            return;
        }
        
        // 공격 실행 후에 isAttacking 설정
        nextFireTime = Time.time + coolTime;
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

        // 크로스헤어
        UIManager.Instance?.TriggerCrosshairFireEffect();

        // 탄약 UI 업데이트
        UpdateAmmoCount();
        
        // 공격 실행 후에 isAttacking 설정
        isAttacking = true;
        
        // 쿨타임 후 isAttacking 해제
        StartCoroutine(ResetIsAttackingAfterDelay(coolTime));
    }

    private IEnumerator ResetIsAttackingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false;
    }

    private void FireSingle()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if(Physics.Raycast(ray, out RaycastHit hit, weaponData.range, hitLayer))
        {
            if(hit.collider.TryGetComponent(out IDamageable target))
            {
                target.TakeDamage(weaponData.damage);
                Debug.Log($"[총기 타격] 대상: {hit.collider.name}, 데미지: {weaponData.damage}");
            }
        }

        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * weaponData.range, Color.yellow, 0.5f);
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
            Vector3 spreadDirection = spreadRotation * Camera.main.transform.forward;

            Ray ray = new Ray(Camera.main.transform.position, spreadDirection);
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

    private void CheckEnemyInSight()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, weaponData.range, hitLayer))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                UIManager.Instance?.SetCrosshairColor(Color.red);
                return;
            }
        }

        UIManager.Instance?.SetCrosshairColor(Color.white);
    }

    /// <summary>
    /// TODO: 탄약 소비 이펙트 추가
    /// </summary>
    private IEnumerator Reload(float duration)
    {
        if (currentAmmo >= weaponData.magazineSize)
        {
            Debug.Log("이미 탄창이 가득 찬 경우");
            yield break; // 이미 탄창이 가득 찬 경우
        }

        int ammoNeeded = weaponData.magazineSize - currentAmmo;
        int ammoAvailable = InventoryManager.Instance.GetAmmoCount(weaponData.ammoType);

        if (ammoAvailable <= 0)
        {
            Debug.Log("재장전할 탄약이 없습니다.");
            yield break;
        }

        int ammoToReload = Mathf.Min(ammoNeeded, ammoAvailable);

        // 인벤토리에서 탄약 소비 시도
        if (InventoryManager.Instance.UseAmmo(weaponData.ammoType, ammoToReload))
        {
            isReloading = true;
            currentAmmo += ammoToReload;
            Debug.Log($"탄약 재장전: {ammoToReload}발. 현재 탄약: {currentAmmo}");

            // 탄약 UI 업데이트
            int totalAmmo = InventoryManager.Instance.GetAmmoCount(weaponData.ammoType);
            UIManager.Instance?.UpdateAmmo(currentAmmo, totalAmmo);

            // 탄약 재장전 애니메이션 재생
            animator.SetTrigger("IsReloading");
            yield return new WaitForSeconds(duration);
            isReloading = false;
        }
        else
        {
            Debug.LogWarning("탄약 소비에 실패했습니다. (InventoryManager.UseAmmo false 반환)");
        }

        // 탄약 재장전 사운드 재생
    }

    private void OnReload()
    {
        if(WeaponManager.Instance.CurrentWeapon == this)
        {
            Debug.Log("Reload");
            StartCoroutine(Reload(reloadDuration));
        }
    }

    private void OnAimStarted()
    {
        Debug.Log("Aim Started");
        isAiming = true;
        currentTargetPosition = originPosition + adsPosition;
    }

    private void OnAimCanceled()
    {
        Debug.Log("Aim Canceled");
        isAiming = false;
        currentTargetPosition = originPosition;
    }

    private void UpdateWeaponPosition()
    {
        // currentTargetPosition = isAiming ? originPosition + adsPosition : originPosition;
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            currentTargetPosition,
            Time.deltaTime * aimMoveSpeed
        );
    }

    public void OnReloadAnimationEnd()
    {
        isReloading = false;
    }
}
