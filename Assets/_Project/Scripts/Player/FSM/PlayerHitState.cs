using UnityEngine;

public class PlayerHitState : PlayerBaseState
{
    private float hitDuration = 0.3f;
    private float timer = 0f;

    public PlayerHitState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        // 피격 효과 적용
        ApplyHitEffects();
        
        // 타이머 초기화
        timer = 0f;
    }

    public override void Exit()
    {
        Debug.Log("PlayerHitState Exit");
    }

    public override void Update()
    {
        timer += Time.deltaTime;
        if (timer >= hitDuration)
        {
            // 로코모션 상태로 복귀
            stateMachine.ChangeState(new PlayerLocomotionState(stateMachine));
        }
    }

    private void ApplyHitEffects()
    {
        // 화면 가장자리 빨간색 효과
        // ScreenEffectManager.Instance?.StartHitEffect();
        
        // 화면 흔들림
        // CameraController.Instance?.ShakeCamera(0.3f, 0.5f);
        
        // 피격 효과음
        // AudioManager.Instance?.PlaySound("PlayerHit");
    }
}
