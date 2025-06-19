using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float clampAngle = 80f;
    [SerializeField] private float normalFOV = 60f;
    // [SerializeField] private float zoomedFOV = 30f;
    [SerializeField] private float zoomSpeed = 5f;

    private float rotX = 0f;
    private Camera playerCamera;
    private float targetFOV;
    private bool isZoomed = false;

    // Recoil
    private float recoilX = 0f;
    private float recoilY = 0f;
    private float recoilRecoverySpeed = 10f;

    #region Singleton
    public static CameraController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private void Start()
    {
        InputManager.Instance.OnLookInput += OnLookInput;
        InputManager.Instance.OnAimStarted += OnAimStarted;
        InputManager.Instance.OnAimCanceled += OnAimCanceled;
        
        playerCamera = GetComponent<Camera>();
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
        
        targetFOV = normalFOV;
        playerCamera.fieldOfView = normalFOV;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        UpdateRecoilRecoverySpeed();
    }

    private void OnDisable()
    {
        InputManager.Instance.OnLookInput -= OnLookInput;
        InputManager.Instance.OnAimStarted -= OnAimStarted;
        InputManager.Instance.OnAimCanceled -= OnAimCanceled;
    }

    private void Update()
    {
        // FOV 부드러운 전환
        // if (playerCamera.fieldOfView != targetFOV)
        if (!Mathf.Approximately(playerCamera.fieldOfView, targetFOV))  // 두 값이 근사치인지 확인
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, zoomSpeed * Time.deltaTime);
        }

        // 복구 전 Clamp 제한
        recoilX = Mathf.Clamp(recoilX, -10f, 10f);
        recoilY = Mathf.Clamp(recoilY, -5f, 5f);

        // Recoil 복구
        recoilX = Mathf.Lerp(recoilX, 0f, recoilRecoverySpeed * Time.deltaTime);
        recoilY = Mathf.Lerp(recoilY, 0f, recoilRecoverySpeed * Time.deltaTime);
    }

    private void OnLookInput(Vector2 input)
    {
        float mouseX = input.x * mouseSensitivity * Time.deltaTime;
        float mouseY = input.y * mouseSensitivity * Time.deltaTime;

        rotX -= mouseY;
        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        ApplyLookRotation(mouseX);
    }

    private void ApplyLookRotation(float mouseX)
    {
        // 반동 적용
        float recoilRotX = rotX + recoilX;
        float recoilRotY = recoilY;
        transform.localRotation = Quaternion.Euler(recoilRotX, 0, 0);
        player.Rotate(Vector3.up * (mouseX + recoilRotY));
    }

    private void OnAimStarted()
    {
        var currentWeapon = WeaponManager.Instance.CurrentWeapon;
        if (currentWeapon!= null)
        {
            var weaponData = currentWeapon.weaponData;
            if (weaponData.weaponType == WeaponType.Pistol || weaponData.weaponType == WeaponType.Shotgun || weaponData.weaponType == WeaponType.Rifle)
            {
                isZoomed = true;
                targetFOV = weaponData.aimFOV;
                UpdateRecoilRecoverySpeed();
                return;
            }
        }
        
        // 근접 무기 등은 줌인 불가
        OnAimCanceled();
    }

    private void OnAimCanceled()
    {
        isZoomed = false;
        targetFOV = normalFOV;
        UpdateRecoilRecoverySpeed();
    }

    public void CancelAim()
    {
        OnAimCanceled();
    }

    public void ApplyRecoil(float addRecoilX, float addRecoilY)
    {
        recoilX += addRecoilX;
        recoilY += addRecoilY;
    }

    public void UpdateRecoilRecoverySpeed()
    {
        var currentWeapon = WeaponManager.Instance.CurrentWeapon;
        if (currentWeapon != null && currentWeapon.weaponData != null)
        {
            recoilRecoverySpeed = currentWeapon.weaponData.recoilRecoverySpeed;
        }
        else
        {
            recoilRecoverySpeed = 10f;
        }
    }
}
