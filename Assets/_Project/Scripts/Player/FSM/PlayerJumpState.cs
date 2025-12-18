using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private PlayerController playerController;
    private float jumpStartTime;
    private bool landSfxPlayed = false;
    private const float MAX_JUMP_DURATION = 2f; // 최대 점프 지속 시간

    protected override float MovementFactor => 0.8f;

    public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        playerController = stateMachine.GetComponent<PlayerController>();
    }

    public override void Enter()
    {
        landSfxPlayed = false;

        // 점프 시작
        jumpStartTime = Time.time;
        
        // 점프 실행 (힘 적용 + 애니메이션)
        playerController.PerformJump();
        
        // 점프 입력 이벤트 등록 (공중에서 추가 점프 가능)
        InputManager.Instance.OnJumpPressed += OnJumpPressed;

        // TimingComboManager.Instance.StopBeatRoutine();
        
        Debug.Log("PlayerJumpState 진입");
    }

    public override void Exit()
    {
        // 점프 입력 이벤트 해제
        InputManager.Instance.OnJumpPressed -= OnJumpPressed;
        
        Debug.Log("PlayerJumpState 종료");
    }

    public override void Update()
    {
        // 점프 중 물리 업데이트
        UpdateMovement();
        
        // 착지 체크
        if (playerController.IsGrounded())
        {
            PlayLandSfx();
            stateMachine.ChangeState(new PlayerLocomotionState(stateMachine));
            return;
        }
        
        // 최대 점프 시간 체크 (안전장치)
        if (Time.time - jumpStartTime > MAX_JUMP_DURATION)
        {
            Debug.LogWarning("점프 시간 초과, LocomotionState로 강제 전환");
            stateMachine.ChangeState(new PlayerLocomotionState(stateMachine));
            return;
        }
    }

    private void OnJumpPressed()
    {
        // 공중에서 추가 점프 (더블 점프 등) 구현 가능
        // 현재는 기본 점프만 구현
        Debug.Log("점프 중 추가 점프 입력 감지");
    }

    private void PlayLandSfx()
    {
        if (landSfxPlayed) return;
        landSfxPlayed = true;

        AudioManager.Instance.Play3dSfxFromCache(
            "Player_Land",
            playerController.transform.position,
            0.9f,
            1f
        );
    }
}
