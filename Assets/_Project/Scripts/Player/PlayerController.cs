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

    private void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        InputManager.Instance.OnMoveInput += OnMoveInput;
        InputManager.Instance.OnJumpPressed += OnJumpPressed;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnMoveInput -= OnMoveInput;
        InputManager.Instance.OnJumpPressed -= OnJumpPressed;
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

    private void Move()
    {
        Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;
        Vector3 move = moveDirection * moveSpeed + Vector3.up * velocity.y;
        controller.Move(move * Time.deltaTime);

        // 애니메이션 처리
        Vector3 horizontalVelocity = new Vector3(controller.velocity.x, 0, controller.velocity.z);
        float speed = horizontalVelocity.magnitude / moveSpeed;
        animator.SetFloat("Speed", speed);
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