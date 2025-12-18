using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    private PlayerController playerController;
    private Vector3 dashDirection;
    private float dashSpeed = 20f;
    private float dashDuration = 0.25f;
    private float shakeIntensity = 0.3f;

    private float timer;

    protected override float MovementFactor => 0f;

    public PlayerDashState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        playerController = stateMachine.PlayerController;
    }

    public override void Enter()
    {
        timer = 0f;

        // 플레이어 무적 시간 부여
        PlayerManager.Instance.SetInvincible(true, dashDuration);

        Vector2 input = playerController.LastMoveInput;
        dashDirection = (playerController.transform.forward * input.y + playerController.transform.right * input.x).normalized;
        if(dashDirection == Vector3.zero)
        {
            dashDirection = playerController.transform.forward;
        }

        // 대쉬 카메라 흔들림 추가
        CameraController.Instance.PlayImpactShake(shakeIntensity, dashDuration);

        // 대쉬 사운드
        AudioManager.Instance.Play3dSfxFromCache(
            "Player_Dash",
            playerController.transform.position,
            0.85f,
            Random.Range(1f, 1.05f)
        );

        // 대쉬 볼륨스냅샷 추가
        var snapshot = VolumeSnapshotController.Current;
        if (snapshot != null)
        {
            snapshot.PlayDashPulse(1f, dashDuration);
        }
    }

    public override void Update()
    {
        timer += Time.deltaTime;
        playerController.MoveDirectly(dashDirection * dashSpeed);

        if(timer >= dashDuration)
        {
            stateMachine.ChangeState(new PlayerLocomotionState(stateMachine));
        }
    }

    public override void Exit()
    {
        // Debug.Log("DashState 종료");
    }
}
