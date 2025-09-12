using UnityEngine;

[RequireComponent(typeof(GameStateMachine))]
public class GameManager : MonoBehaviour
{
    [SerializeField] private bool isTimeTest = false;

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


    #region 싱글톤 및 초기화
    public static GameManager Instance { get; private set; }

    public void Initialize()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning($"{GetType().Name} 인스턴스 감지됨, 초기화 스킵");
            return;
        }
        Instance = this;

        InputManager.Instance.OnPause += OnPausePressed;
        SaveManager.Instance.OnAfterLoad += HandleAfterLoad;
    }
    #endregion

    private void Awake()
    {
        gameStateMachine = GetComponent<GameStateMachine>();
    }

    private void Start()
    {
        if(isTimeTest)
            EnterPuzzle();
        else
            EnterExploration();  // 메인메뉴 구현 이후에는 수정할것
    }

    private void OnEnable()
    {
        Enemy.OnCombatStarted += OnCombatDetected;

        if(InputManager.Instance != null)
            InputManager.Instance.OnPause += OnPausePressed;

        if (SaveManager.Instance != null)
            SaveManager.Instance.OnAfterLoad += HandleAfterLoad;
    }

    private void OnDisable()
    {
        Enemy.OnCombatStarted -= OnCombatDetected;

        if(InputManager.Instance != null)
            InputManager.Instance.OnPause -= OnPausePressed;

        if (SaveManager.Instance != null)
            SaveManager.Instance.OnAfterLoad -= HandleAfterLoad;
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

    public void EnterPreviousState()
    {
        var prev = PreviousGameState;

        // 유효성 검사: null 이거나 복귀하기 부적합한 상태면 탐색 상태로 대체
        if (prev == null || prev is MainMenuState || prev is LoadingState || prev is GameOverState || prev is PausedState)
            prev = explorationState;

        // 만약 이미 현재 상태와 동일하면 무시
        if (CurrentGameState == prev)
            return;

        ChangeState(prev);
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
            // ChangeState(PreviousGameState);
            EnterPreviousState();
        }
        else
        {            
            EnterPaused();
        }
    }

    private void HandleAfterLoad()
    {
        StartCoroutine(PostLoadRoutine());
    }

    private System.Collections.IEnumerator PostLoadRoutine()
    {
        yield return null;
    }
}
