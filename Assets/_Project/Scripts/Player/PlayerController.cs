using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float fallMultiplier = 2.5f;

    private Animator animator;
    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 velocity;

    private bool isRunning = false;
    private bool isCrouching = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
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

        // 수평 속도 벡터 + 중력 적용
        Vector3 horizontalMove = moveDirection * currentSpeed;
        Vector3 finalMove = horizontalMove + Vector3.up * velocity.y;

        controller.Move(finalMove * Time.deltaTime);

        // 애니메이션 블렌드 파라미터 업데이트
        Vector3 horizontalVelocity = new Vector3(controller.velocity.x, 0, controller.velocity.z);
        float normalizedSpeed = horizontalVelocity.magnitude / (moveSpeed * 1.5f); // 최대값 기준 정규화
        animator.SetFloat("Speed", normalizedSpeed);
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