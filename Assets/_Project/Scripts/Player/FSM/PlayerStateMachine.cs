using UnityEngine;
using System.Collections.Generic;

public enum PlayerState
{
    Locomotion,
    Jump,
    Attack,
    Combo,
    Hit,
    Death,
    Rewind
}
public class PlayerStateMachine : MonoBehaviour
{
    public PlayerBaseState currentState;
    public PlayerState currentStateType;
    private readonly Dictionary<System.Type, PlayerState> stateTypeMap = new Dictionary<System.Type, PlayerState>();

    private void Awake()
    {
        InitializeStateTypeMap();
    }

    private void Start()
    {
        ChangeState(new PlayerLocomotionState(this));
    }

    private void Update()
    {
        currentState?.Update();
    }

    private void InitializeStateTypeMap()
    {
        stateTypeMap[typeof(PlayerLocomotionState)] = PlayerState.Locomotion;
        stateTypeMap[typeof(PlayerJumpState)] = PlayerState.Jump;
        stateTypeMap[typeof(PlayerAttackState)] = PlayerState.Attack;
        stateTypeMap[typeof(PlayerComboState)] = PlayerState.Combo;
        // 필요시 추가: stateTypeMap[typeof(PlayerHitState)] = PlayerState.Hit; 등
    }

    public void ChangeState(PlayerBaseState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
        currentStateType = stateTypeMap.GetValueOrDefault(newState.GetType(), currentStateType);
    }
}
