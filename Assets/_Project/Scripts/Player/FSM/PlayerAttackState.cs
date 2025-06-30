using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private PlayerController playerController;
    private float attackStartTime;
    private const float ATTACK_DURATION = 0.5f; // 기본 공격 지속 시간
    private const float MAX_ATTACK_DURATION = 3f; // 최대 공격 지속 시간 (안전장치)

    public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        playerController = stateMachine.GetComponent<PlayerController>();
    }

    public override void Enter()
    {
        // 공격 시작
        attackStartTime = Time.time;
        
        // 공격 입력 이벤트 등록
        InputManager.Instance.OnAttackPressed += OnAttackPressed;
        
        // 공격 실행 (애니메이션 + 무기 공격)
        playerController.PerformAttack();
        
        Debug.Log("PlayerAttackState 진입");
    }

    public override void Exit()
    {
        // 공격 입력 이벤트 해제
        InputManager.Instance.OnAttackPressed -= OnAttackPressed;
        
        Debug.Log("PlayerAttackState 종료");
    }

    public override void Update()
    {
        // 공격 중 물리 업데이트 (이동은 제한되지만 중력은 적용)
        playerController.LocomotionUpdate();
        
        // 공격 완료 체크 (기본 지속 시간)
        if (Time.time - attackStartTime >= ATTACK_DURATION)
        {
            // LocomotionState로 전환
            stateMachine.ChangeState(new PlayerLocomotionState(stateMachine));
            return;
        }
        
        // 최대 공격 시간 체크 (안전장치)
        if (Time.time - attackStartTime > MAX_ATTACK_DURATION)
        {
            Debug.LogWarning("공격 시간 초과, LocomotionState로 강제 전환");
            stateMachine.ChangeState(new PlayerLocomotionState(stateMachine));
            return;
        }
    }

    private void OnAttackPressed()
    {
        // 공격 중 추가 공격 입력 처리 (콤보 시스템)
        Debug.Log("공격 중 콤보 입력 감지");
        HandleComboAttack();
    }

    private void HandleComboAttack()
    {
        // 콤보 입력 감지 → PlayerComboState로 전환
        Debug.Log("콤보 입력 감지 - PlayerComboState로 전환");
        stateMachine.ChangeState(new PlayerComboState(stateMachine));
    }
}
