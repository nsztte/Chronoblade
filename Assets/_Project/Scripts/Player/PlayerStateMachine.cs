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
    Rewind,
    Dash
}
public class PlayerStateMachine : MonoBehaviour
{
    private Animator animator;
    private PlayerController playerController;
    private PlayerBaseState currentState;
    [SerializeField] private PlayerState currentStateType;
    private readonly Dictionary<System.Type, PlayerState> stateTypeMap = new Dictionary<System.Type, PlayerState>();

    public Animator Animator => animator;
    public PlayerController PlayerController => playerController;
    public PlayerBaseState CurrentState => currentState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
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
        stateTypeMap[typeof(PlayerHitState)] = PlayerState.Hit;
        stateTypeMap[typeof(PlayerDeathState)] = PlayerState.Death;
        stateTypeMap[typeof(PlayerRewindState)] = PlayerState.Rewind;
    }

    public void ChangeState(PlayerBaseState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
        currentStateType = stateTypeMap.GetValueOrDefault(newState.GetType(), currentStateType);
    }
}
