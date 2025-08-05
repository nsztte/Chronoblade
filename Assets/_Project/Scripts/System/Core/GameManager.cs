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
        // ChangeState(mainMenuState);
        EnterExploration();  // 메인메뉴 구현 이후에는 수정할것

        Enemy.OnCombatStarted += OnCombatDetected;
        InputManager.Instance.OnPause += OnPausePressed;
    }

    private void OnDestroy()
    {
        Enemy.OnCombatStarted -= OnCombatDetected;
        InputManager.Instance.OnPause -= OnPausePressed;
    }

    public void ChangeState(GameBaseState newState)
    {
        if(CurrentGameState == newState) return;

        PreviousGameState = CurrentGameState;
        newState.Init(this);
        gameStateMachine.ChangeState(newState);
        CurrentGameState = newState;
    }

    public void EnterMainMenu()
    {
        ChangeState(mainMenuState);
    }

    public void EnterLoading()
    {
        ChangeState(loadingState);
    }

    public void EnterExploration()
    {
        ChangeState(explorationState);
    }

    public void EnterCombat()
    {
        ChangeState(combatState);
    }

    public void EnterPuzzle()
    {
        ChangeState(puzzleState);
    }

    public void EnterCutscene()
    {
        ChangeState(cutsceneState);
    }

    public void EnterPaused()
    {
        ChangeState(pausedState);
    }

    public void EnterGameOver()
    {
        ChangeState(gameOverState);
    }

    private void OnCombatDetected()
    {
        if(CurrentGameState is ExplorationState || CurrentGameState is PuzzleState)
        {
            EnterCombat();
        }
    }

    private void OnPausePressed()
    {
        if(CurrentGameState is MainMenuState || CurrentGameState is LoadingState || CurrentGameState is GameOverState) return;

        if(CurrentGameState is PausedState)
        {
            ChangeState(PreviousGameState);
        }
        else
        {
            EnterPaused();
        }
    }
}
