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

    // 반동
    private float recoilX = 0f;
    private float recoilY = 0f;
    private float recoilRecoverySpeed = 10f;

    // 무기 들었을 때 시야각 제한
    [SerializeField] private float weaponClampAngle = 30f;
    [SerializeField] private float zoomedClampAngle = 10f;

    private float defaultLocalY;
    private float targetLocalY;
    private float cameraLerpSpeed = 10f;

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
        // 카메라 기본 Y 위치 저장
        defaultLocalY = transform.localPosition.y;
        targetLocalY = defaultLocalY;
        
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
        if (!Mathf.Approximately(playerCamera.fieldOfView, targetFOV))
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, zoomSpeed * Time.deltaTime);
        }

        // 카메라 Y 위치 부드럽게 이동
        Vector3 localPos = transform.localPosition;
        localPos.y = Mathf.Lerp(localPos.y, targetLocalY, cameraLerpSpeed * Time.deltaTime);
        transform.localPosition = localPos;

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

        // 평소에도 시야각을 제한하되, 무기를 들었을 때는 더 크게(더 좁게) 제한
        float minAngle = -clampAngle;
        float maxAngle = clampAngle;

        var currentWeapon = WeaponManager.Instance.CurrentWeapon;
        if (currentWeapon != null)
        {
            if(isZoomed)
            {
                minAngle = -zoomedClampAngle;
                maxAngle = zoomedClampAngle;
            }
            else
            {
                minAngle = -weaponClampAngle;
                maxAngle = weaponClampAngle;
            }
        }

        rotX = Mathf.Clamp(rotX, minAngle, maxAngle);

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

    // 웅크리기용 카메라 높이 조정 메서드
    public void SetCameraHeight(float targetY, float lerpSpeed = 10f)
    {
        targetLocalY = targetY;
        cameraLerpSpeed = lerpSpeed;
    }

    public float GetDefaultCameraLocalY() => defaultLocalY;
}
