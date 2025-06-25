using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float fallMultiplier = 2.5f;

    [Header("Crouch")]
    [SerializeField] private float crouchCameraYOffset = -0.5f;
    [SerializeField] private float crouchingMultiplier = 0.6f;

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 velocity;

    private bool isRunning = false;
    private bool isCrouching = false;

    // 웅크리기 관련 변수
    private float originalControllerHeight;
    private Vector3 originalControllerCenter;
    private float crouchControllerHeight;
    private Vector3 crouchControllerCenter;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        // 원래 컨트롤러 값 저장
        originalControllerHeight = controller.height;
        originalControllerCenter = controller.center;
        crouchControllerHeight = originalControllerHeight * crouchingMultiplier;
        crouchControllerCenter = new Vector3(originalControllerCenter.x, originalControllerCenter.y * crouchingMultiplier, originalControllerCenter.z);
    }

    private void Start()
    {
        InputManager.Instance.OnMoveInput += OnMoveInput;
        InputManager.Instance.OnJumpPressed += OnJumpPressed;
        InputManager.Instance.OnRunStarted += OnRunStarted;
        InputManager.Instance.OnRunCanceled += OnRunCanceled;
        InputManager.Instance.OnCrouchPressed += OnCrouchPressed;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnMoveInput -= OnMoveInput;
        InputManager.Instance.OnJumpPressed -= OnJumpPressed;
        InputManager.Instance.OnRunStarted -= OnRunStarted;
        InputManager.Instance.OnRunCanceled -= OnRunCanceled;
        InputManager.Instance.OnCrouchPressed -= OnCrouchPressed;
    }

    private void Update()
    {
        Move();
        ApplyGravity();
    }

    private void OnMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    private void OnJumpPressed()
    {
        if(controller.isGrounded)
        {
            //velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            velocity.y = jumpForce;
            PlayerManager.Instance.SetAnimatorTrigger("IsJumping");
        }
    }

    private void OnRunStarted()
    {
        isRunning = true;
    }

    private void OnRunCanceled()
    {
        isRunning = false;
    }
    
    private void OnCrouchPressed()
    {
        isCrouching = !isCrouching;
        if (isCrouching)
        {
            controller.height = crouchControllerHeight;
            controller.center = crouchControllerCenter;
            
            // 카메라 위치 내리기
            float targetY = CameraController.Instance.GetDefaultCameraLocalY() + crouchCameraYOffset;
            CameraController.Instance.SetCameraHeight(targetY, 10f);
        }
        else
        {
            controller.height = originalControllerHeight;
            controller.center = originalControllerCenter;
            // 카메라 원래 위치로
            float targetY = CameraController.Instance.GetDefaultCameraLocalY();
            CameraController.Instance.SetCameraHeight(targetY, 10f);
        }
    }

    private void Move()
    {
        // 입력 벡터를 정규화하여 대각선 이동 시 속도가 증가하지 않도록 함
        Vector2 normalizedInput = moveInput.normalized;
        Vector3 moveDirection = transform.right * normalizedInput.x + transform.forward * normalizedInput.y;

        float currentSpeed = moveSpeed;

        if(isCrouching)
            currentSpeed *= 0.5f;
        else if(isRunning && normalizedInput.y > 0)
            currentSpeed *= 1.5f;
        else if(normalizedInput.y < 0)
            currentSpeed *= 0.8f;

        // 수평 속도 벡터 + 중력 적용
        Vector3 horizontalMove = moveDirection * currentSpeed;
        Vector3 finalMove = horizontalMove + Vector3.up * velocity.y;

        controller.Move(finalMove * Time.deltaTime);

        // 애니메이션 블렌드 파라미터 업데이트
        Vector3 horizontalVelocity = new Vector3(controller.velocity.x, 0, controller.velocity.z);
        float normalizedSpeed = horizontalVelocity.magnitude / (moveSpeed * 1.5f); // 최대 속도 기준 정규화

        PlayerManager.Instance.SetAnimatorFloat("Speed", normalizedSpeed, 0.1f, Time.deltaTime);
        PlayerManager.Instance.SetAnimatorFloat("DirectionX", normalizedInput.x, 0.1f, Time.deltaTime);
        PlayerManager.Instance.SetAnimatorFloat("DirectionY", normalizedInput.y, 0.1f, Time.deltaTime);
        PlayerManager.Instance.SetAnimatorBool("IsCrouching", isCrouching);
    }

    private void ApplyGravity()
    {
        if(controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        else
        {
            if(velocity.y < 0)
            {
                velocity.y += fallMultiplier * gravity * Time.deltaTime;
            }
            else
            {
                velocity.y += gravity * Time.deltaTime;
            }
        }
    }
}