using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private PlayerController playerController;
    private float jumpStartTime;
    private const float MAX_JUMP_DURATION = 2f; // 최대 점프 지속 시간

    public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        playerController = stateMachine.GetComponent<PlayerController>();
    }

    public override void Enter()
    {
        // 점프 시작
        jumpStartTime = Time.time;
        
        // 점프 힘 적용
        playerController.ApplyJumpForce();
        
        // 점프 애니메이션 설정
        playerController.SetJumpAnimation();
        
        // 점프 입력 이벤트 등록 (공중에서 추가 점프 가능)
        InputManager.Instance.OnJumpPressed += OnJumpPressed;
        
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
        playerController.LocomotionUpdate();
        
        // 착지 체크
        if (playerController.IsGrounded())
        {
            // 착지 시 LocomotionState로 전환
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
}
