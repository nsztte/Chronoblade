using UnityEngine;

[RequireComponent(typeof(GameStateMachine))]
public class GameManager : MonoBehaviour
{
    [Header("스테이트 프리팹")]
    public GameBaseState mainMenuState;
    public GameBaseState loadingState;
    public GameBaseState explorationState;
    public GameBaseState combatState;
    public GameBaseState puzzleState;
    public GameBaseState cutsceneState;
    public GameBaseState pausedState;
    public GameBaseState gameOverState;

    private GameStateMachine gameStateMachine;
    
    public GameBaseState CurrentGameState { get; private set; }
    public GameBaseState PreviousGameState { get; private set; }


    #region 싱글톤, 초기화
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        gameStateMachine = GetComponent<GameStateMachine>();
    }
    #endregion
    private void Start()
    {
        ChangeState(mainMenuState);
    }

    public void ChangeState(GameBaseState newState)
    {
        if(CurrentGameState == newState) return;

        PreviousGameState = CurrentGameState;
        newState.Init(this);
        gameStateMachine.ChangeState(newState);
        CurrentGameState = newState;
    }
}
