using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform cameraPosition;
    [SerializeField] private Camera fpCamera;
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float clampAngle = 80f;
    [SerializeField] private float normalFOV = 60f;
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
    [SerializeField] private float defaultLocalY;
    [SerializeField] private float targetLocalY;
    private float cameraLerpSpeed = 10f;

    [Header("이동 쉐이킹")]
    [SerializeField] private float walkBobAmplitude = 0.02f;
    [SerializeField] private float runBobAmplitude = 0.05f;
    [SerializeField] private float bobFrequency = 8f;
    private float bobPhase;
    private Vector3 baseCamLocalPos;

    [Header("공격/콤보용 임팩트 쉐이크")]
    [SerializeField] private float shakeDuration = 0.1f;
    [SerializeField] private float shakeIntensity = 0.05f;
    private float shakeTimer = 0f;
    private Vector3 shakeOffset = Vector3.zero;


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
        if (fpCamera) fpCamera.fieldOfView = normalFOV;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        UpdateRecoilRecoverySpeed();

        // defaultLocalPosition = transform.localPosition;
        // targetLocalPosition = defaultLocalPosition;

        baseCamLocalPos = transform.localPosition;
    }

    private void OnDestroy()
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
            float fov = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, zoomSpeed * Time.deltaTime);
            playerCamera.fieldOfView = fov;
            if (fpCamera) fpCamera.fieldOfView = fov;
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

    private void LateUpdate()
    {
        // 이동시 카메라 좌우 이동
        var player = PlayerManager.Instance?.PlayerController;
        if (player == null) return;

        Vector3 hv = new Vector3(player.GetComponent<CharacterController>().velocity.x, 0, player.GetComponent<CharacterController>().velocity.z);
        float speed = hv.magnitude;
        bool isRunning = player.IsRunning;

        // 1) 보블 오프셋 (속도=0이면 자동으로 0에 수렴)
        float amp = isRunning ? runBobAmplitude : walkBobAmplitude;
        if (isZoomed) amp *= 0.3f;

        if (speed >= 0.1f)
        {
            bobPhase += bobFrequency * Time.deltaTime * (isRunning ? 1.5f : 1f);
        }
        else
        {
            // 정지 시 페이즈만 천천히 감쇠(선택), 오프셋은 0으로 자연 복귀
            bobPhase = Mathf.Lerp(bobPhase, 0f, Time.deltaTime * 5f);
        }

        float bobX = (speed >= 0.1f) ? Mathf.Sin(bobPhase) * amp : 0f;
        float bobY = (speed >= 0.1f) ? Mathf.Cos(bobPhase * 2f) * amp * 0.5f : 0f;
        Vector3 bobOffset = new Vector3(bobX, bobY, 0f);

        // 2) 임팩트 쉐이크 오프셋 (항상 적용 경로에 둔다)
        Vector3 impactOffset = Vector3.zero;
        if (shakeTimer > 0f)
        {
            shakeTimer -= Time.deltaTime;
            float t = Mathf.Clamp01(shakeTimer / shakeDuration); // 페이드아웃
            impactOffset = Random.insideUnitSphere * (shakeIntensity * t);
            // 조준 중 과도한 멀미 방지 (선택)
            if (isZoomed) impactOffset *= 0.5f;
        }

        // 3) 최종 타겟 = 기본위치 + 보블 + 임팩트 (early return 없음)
        Vector3 targetPos = baseCamLocalPos + bobOffset + impactOffset;
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * 10f);
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
                UIManager.Instance?.SetCrosshairZoom(true);
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
        UIManager.Instance?.SetCrosshairZoom(false);
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

    public void ResetToPlayer()
    {
        // 위치 초기화
        transform.localPosition = cameraPosition.localPosition;
        defaultLocalY = transform.localPosition.y;
        targetLocalY = defaultLocalY;

        // 회전 초기화
        transform.localRotation = cameraPosition.localRotation;

        // FOV 복구
        targetFOV = normalFOV;
        playerCamera.fieldOfView = normalFOV;
        if (fpCamera) fpCamera.fieldOfView = normalFOV;

        // 줌 상태 초기화
        isZoomed = false;

        // 커서 락 상태 복구
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void PlayImpactShake(float intensity = -1f, float duration = -1f)
    {
        if (intensity > 0f) shakeIntensity = intensity;
        if (duration  > 0f) shakeDuration  = duration;
        shakeTimer = shakeDuration;

        Debug.Log($"PlayImpactShake: {shakeIntensity}, {shakeDuration}, {shakeDuration}");
    }
}
