using UnityEngine;

public class PlayerBlockState : PlayerBaseState
{
    private PlayerController playerController;

    protected override float MovementFactor => 0.5f;

    public PlayerBlockState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        playerController = stateMachine.PlayerController;
    }

    public override void Enter()
    {
        PlayerManager.Instance.IsBlocking = true;
        InputManager.Instance.OnBlockCanceled += OnBlockCanceled;

        WeaponManager.Instance?.CurrentWeapon?.SetBlocking(true);

        Debug.Log("BlockState 시작");
    }

    public override void Exit()
    {
        PlayerManager.Instance.IsBlocking = false;
        InputManager.Instance.OnBlockCanceled -= OnBlockCanceled;
        PlayerManager.Instance.LastBlockEndTime = Time.time;

        WeaponManager.Instance?.CurrentWeapon?.SetBlocking(false);

        Debug.Log("BlockState 종료");
    }

    public override void Update()
    {
        UpdateMovement();
    }

    private void OnBlockCanceled()
    {
        stateMachine.ChangeState(new PlayerLocomotionState(stateMachine));
    }
}