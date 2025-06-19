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
        if (playerCamera.fieldOfView != targetFOV)
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, zoomSpeed * Time.deltaTime);
        }
    }

    private void OnLookInput(Vector2 input)
    {
        float mouseX = input.x * mouseSensitivity * Time.deltaTime;
        float mouseY = input.y * mouseSensitivity * Time.deltaTime;

        rotX -= mouseY;
        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        transform.localRotation = Quaternion.Euler(rotX, 0, 0);

        player.Rotate(Vector3.up * mouseX);
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
                return; 
            }
        }
        // 근접 무기 등은 줌인 불가
        isZoomed = false;
        targetFOV = normalFOV;
    }

    private void OnAimCanceled()
    {
        isZoomed = false;
        targetFOV = normalFOV;
    }

    public void CancelAim()
    {
        OnAimCanceled();
    }
}
