using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GameStateMachine))]
public class GameManager : MonoBehaviour
{
    [Header("스테이트 스크립터블오브젝트")]
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

    private const string TITLESCENE = "Title";


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
        var sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == TITLESCENE)
            EnterMainMenu();
        else
            EnterExploration();
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

    public void EnterFinalChapter()
    {
        LoadingState.NextLoadingMode = LoadingMode.SceneTransition;
        LoadingState.NextSceneName = LoadingState.FINALSCENE;
        EnterLoading();
    }

    public void EnterEnding()
    {
        LoadingState.NextLoadingMode = LoadingMode.Ending;
        EnterLoading();
    }

    public void EnterPreviousState()
    {
        var prev = PreviousGameState;

        // 유효성 검사: null 이거나 복귀하기 부적합한 상태면 탐색 상태로 대체
        if (prev == null || prev is MainMenuState || prev is LoadingState || prev is GameOverState || prev is PausedState || prev is CutsceneState)
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

        // 1. 입력 잠금 해제
        InputManager.Instance?.SetInputEnabled(true);

        // 2. HUD 및 UI 복구
        UIManager.Instance?.UpdatePlayerHud(true);
        UIManager.Instance?.SetCursorLockState(CursorLockMode.Locked);

        // 3. 카메라 위치 초기화
        CameraController.Instance?.ResetToPlayer();
    }
}
