using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PuzzleClockManager : MonoBehaviour
{
    [Header("퍼즐 바늘 참조")]
    [SerializeField] private PuzzleHand hourHand;
    [SerializeField] private PuzzleHand minuteHand;
    [SerializeField] private PuzzleHand secondHand;

    [Header("퍼즐 시간 설정")]
    [SerializeField] private float puzzleTimeLimit = 25f;

    private Animator animator;
    private float remainingTime;
    private bool isPuzzleActive = false;
    private bool isPuzzleCleared = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(!isPuzzleActive || isPuzzleCleared) return;

        // 애니메이션 속도 조절
        UpdateAnimationSpeed();

        remainingTime -= Time.deltaTime;

        // TODO: 잔여 시간에 따른 화면 연출 (화면 흔들림 트리거)

        if(remainingTime <= 0f)
        {
            PuzzleFail();
            return;
        }

        if(IsPuzzleCleared())
        {
            PuzzleSuccess();
        }
    }

    public void StartPuzzle()
    {
        GameManager.Instance.EnterPuzzle();

        isPuzzleActive = true;
        isPuzzleCleared = false;
        remainingTime = puzzleTimeLimit;

        Debug.Log("[PuzzleClockManager] 퍼즐 시작");
    }

    private bool IsPuzzleCleared()
    {
        return hourHand.IsAligned() && minuteHand.IsAligned() && secondHand.IsAligned()
            && TimeManager.Instance.CurrentTimeState == TimeState.Stop;
    }

    private void PuzzleSuccess()
    {
        isPuzzleCleared = true;
        isPuzzleActive = false;

        Debug.Log("[PuzzleClockManager] 퍼즐 성공");
        TimeManager.Instance.InitializeTimeState();

        // TODO: 퍼즐 클리어 연출

        GameManager.Instance.EnterCombat();
    }

    private void PuzzleFail()
    {
        isPuzzleCleared = true;
        isPuzzleActive = false;

        Debug.Log("[PuzzleClockManager] 퍼즐 실패");
        TimeManager.Instance.InitializeTimeState();

        // TODO: 퍼즐 실패 연출, 플레이어 패널티 or 보스 체력 회복

        GameManager.Instance.EnterCombat();
    }

    private void UpdateAnimationSpeed()
    {
        float speed = TimeManager.Instance.CurrentTimeState switch
        {
            TimeState.Normal => 1f,
            TimeState.Slow => TimeManager.Instance.SlowFactor,
            TimeState.Stop => 0f,
            TimeState.Rewind => -1f,
            TimeState.FastForward => TimeManager.Instance.FastForwardFactor,
            _ => 1f
        };

        animator.SetFloat("Speed", speed);
    }
}
