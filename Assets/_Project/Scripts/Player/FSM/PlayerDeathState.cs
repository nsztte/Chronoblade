using UnityEngine;

public class PlayerDeathState : PlayerBaseState
{
    private float fadeOutDuration = 1.0f;
    private float timer = 0f;

    public PlayerDeathState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        // 화면 페이드아웃 시작
        // ScreenEffectManager.Instance?.StartDeathFadeOut();
        
        // 사망 메시지 표시
        // UIManager.Instance?.ShowDeathMessage("당신은 죽었습니다");
        
        // 모든 입력 차단
        // InputManager.Instance?.DisableAllInput();
        
        timer = 0f;
    }

    public override void Exit()
    {
        // 리스폰 시 입력 재활성화
        // InputManager.Instance?.EnableAllInput();
    }

    public override void Update()
    {
        timer += Time.deltaTime;
        if (timer >= fadeOutDuration)
        {
            // 게임 오버 처리
            Time.timeScale = 0f;
            Debug.Log("게임 오버");
            //TODO: 연출, 사운드, 애니메이션, 게임 오버 UI 표시
        }
    }
}
