using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour, IStatusEffectable
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float fallMultiplier = 2.5f;

    [Header("Crouch")]
    [SerializeField] private float crouchCameraYOffset = -0.5f;
    [SerializeField] private float crouchingMultiplier = 0.6f;

    [Header("스태미너 소모")]
    [SerializeField] private float runStaminaCostPerSecond = 15f;

    [Header("상태 이상 효과")]
    [SerializeField] private float slowedMultiplier = 0.5f;
    private Coroutine statusCoroutine;

    private float originalMoveSpeed;
    private float originalAnimSpeed;
    private bool isSlowed = false;
    private bool isFrozen = false;
    public bool IsFrozen => isFrozen;
    [SerializeField] private bool isParalyzed = false;
    public bool IsParalyzed => isParalyzed;

    private CharacterController controller;
    private Animator animator;
    private Vector2 moveInput;
    private Vector3 velocity;

    // 대쉬 관련 변수
    private Vector2 lastMoveInput;
    private float lastMoveTime;
    public Vector2 LastMoveInput => Time.time - lastMoveTime > 0.2f ? Vector2.zero : lastMoveInput;

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
        animator = GetComponent<Animator>();

        // 원래 컨트롤러 값 저장
        originalControllerHeight = controller.height;
        originalControllerCenter = controller.center;
        crouchControllerHeight = originalControllerHeight * crouchingMultiplier;
        crouchControllerCenter = new Vector3(originalControllerCenter.x, originalControllerCenter.y * crouchingMultiplier, originalControllerCenter.z);
    }
    
    #region FSM에서 호출할 메서드들
    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;
        if(input != Vector2.zero)
        {
            lastMoveInput = input;
            lastMoveTime = Time.time;
        }
    }

    public void MoveDirectly(Vector3 move)
    {
        controller.Move(move * Time.deltaTime);
    }

    public bool IsGrounded()
    {
        return controller.isGrounded;
    }

    private void ApplyJumpForce()
    {
        velocity.y = jumpForce;
    }

    private void SetJumpAnimation()
    {
        PlayerManager.Instance.SetAnimatorTrigger("IsJumping");
    }

    public void PerformJump()
    {
        ApplyJumpForce();
        SetJumpAnimation();
    }

    public void PerformWeaponAttack()
    {
        WeaponManager.Instance.CurrentWeapon?.ExecuteWeaponAttack();
    }

    // 약공격
    public void PerformLightAttack()
    {
        WeaponManager.Instance.CurrentWeapon?.ExecuteLightAttack();
    }

    // 강공격
    public void PerformHeavyAttack()
    {
        WeaponManager.Instance.CurrentWeapon?.ExecuteHeavyAttack();
    }
    
    public void SetRunning(bool running)
    {
        isRunning = running;
    }

    public void ToggleCrouch()
    {
        isCrouching = !isCrouching;
        if (isCrouching)
        {
            controller.height = crouchControllerHeight;
            controller.center = crouchControllerCenter;
            float targetY = CameraController.Instance.GetDefaultCameraLocalY() + crouchCameraYOffset;
            CameraController.Instance.SetCameraHeight(targetY, 10f);
        }
        else
        {
            controller.height = originalControllerHeight;
            controller.center = originalControllerCenter;
            float targetY = CameraController.Instance.GetDefaultCameraLocalY();
            CameraController.Instance.SetCameraHeight(targetY, 10f);
        }
    }
    #endregion

    #region FSM LocomotionState에서 호출할 이동 관련 Update
    public void LocomotionUpdate()
    {
        if(IsFrozen) return;

        // 달리기 중 스태미너 소모
        if (isRunning && moveInput.y > 0)
        {
            if (!PlayerManager.Instance.UseStaminaIfAvailable(runStaminaCostPerSecond * Time.deltaTime))
            {
                isRunning = false;
            }
        }
        Move();
        ApplyGravity();
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

    // TODO: 각 스테이트 카메라 효과 추가
    // 향후 필요하다면 duration이 0이 아닌 경우 remove 코루틴이 재생되도록 전반적인 수정 고려
    public void ApplyStatus(StatusEffectType effect, float duration = 0f)
    {
        switch(effect)
        {
            case StatusEffectType.Slow:
                if(!isSlowed)
                {
                    Debug.Log("PlayerController: ApplyStatus Slow");
                    
                    isSlowed = true;
                    originalMoveSpeed = moveSpeed;
                    originalAnimSpeed = animator.speed;
                    moveSpeed *= slowedMultiplier;
                    animator.speed = originalAnimSpeed * slowedMultiplier;

                    if(statusCoroutine != null)
                        StopCoroutine(statusCoroutine);
                    statusCoroutine = StartCoroutine(RemoveStatusAfter(StatusEffectType.Slow, duration));
                }
                break;
            case StatusEffectType.Freeze:
                if(!isFrozen)
                {
                    Debug.Log("PlayerController: ApplyStatus Freeze");
                    isFrozen = true;
                    originalAnimSpeed = animator.speed;
                    animator.speed = 0f;

                    if(statusCoroutine != null)
                        StopCoroutine(statusCoroutine);
                    statusCoroutine = StartCoroutine(RemoveStatusAfter(StatusEffectType.Freeze, duration));
                }
                break;
            case StatusEffectType.Paralysis:
                if(!isParalyzed)
                {
                    Debug.Log("PlayerController: ApplyStatus Paralysis");
                    isParalyzed = true;

                    // TODO: 전기 파티클, 카메라 흔들림 등 연출 삽입 위치
                    if(statusCoroutine != null)
                        StopCoroutine(statusCoroutine);
                    statusCoroutine = StartCoroutine(RemoveStatusAfter(StatusEffectType.Paralysis, duration));
                }
                break;
        }
    }

    public void RemoveStatus(StatusEffectType effect)
    {
        switch(effect)
        {
            case StatusEffectType.Slow:
                if(isSlowed)
                {
                    isSlowed = false;
                    moveSpeed = originalMoveSpeed;
                    animator.speed = originalAnimSpeed;
                }
                break;
            case StatusEffectType.Freeze:
                if(isFrozen)
                {
                    isFrozen = false;
                    animator.speed = originalAnimSpeed;
                }
                break;
            case StatusEffectType.Paralysis:
                if(isParalyzed)
                {
                    isParalyzed = false;
                }
                break;
        }
    }

    private IEnumerator RemoveStatusAfter(StatusEffectType effect, float duration)
    {
        if(duration <= 0f) yield break;

        yield return new WaitForSeconds(duration);
        RemoveStatus(effect);
        statusCoroutine = null;
    }

    public void ApplyStatus(ComboAttackData attackData)
    {
    }
    #endregion
}