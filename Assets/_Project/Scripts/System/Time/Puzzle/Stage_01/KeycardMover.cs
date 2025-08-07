using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class KeycardMover : MonoBehaviour, ITimeControllable, IRewindable
{
    [SerializeField] private List<Transform> targetPoints;
    [SerializeField] private float interval = 1f;
    [SerializeField] private PuzzleStateTrigger puzzleStateTrigger;
    
    private int currentTargetIndex = -1;
    private float timeScale = 1f;
    private Animator animator;

    // 리와인드 설정
    private List<int> rewindIndices = new();
    private bool isRewinding = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()    // 상황에 따라서 OnEnable로 변경 가능성 있음
    {
        TimeManager.Instance.RegisterControllable(this);
        TimeManager.Instance.RegisterRewindable(this);

        StartCoroutine(MoveRoutine());
    }

    private void OnDisable()
    {
        TimeManager.Instance.UnregisterControllable(this);
        TimeManager.Instance.UnregisterRewindable(this);

        puzzleStateTrigger.IsCleared = true;
    }

    public float GetTimeScale()
    {
        return timeScale;
    }

    public void SetTimeScale(float timeScale)
    {
        this.timeScale = timeScale;
        animator.speed = timeScale;
    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            MoveToNextPoint();

            if (timeScale <= 0f)
            {
                // 시간 정지 상태일 때는 한 프레임씩 기다리며 timeScale 회복 대기
                yield return new WaitUntil(() => timeScale > 0f);
            }

            yield return new WaitForSeconds(interval / timeScale);
        }
    }

    private void MoveToNextPoint()
    {
        if(currentTargetIndex != -1 && !isRewinding)
        {
            rewindIndices.Add(currentTargetIndex);
            if(rewindIndices.Count > 30)    // 최대 저장 갯수 조절할것
                rewindIndices.RemoveAt(0);
        }
        
        int nextIndex = 0;

        if(isRewinding && rewindIndices.Count != 0)
        {
            nextIndex = rewindIndices[rewindIndices.Count - 1];
            rewindIndices.RemoveAt(rewindIndices.Count - 1);
        }
        else
        {
            do{
                nextIndex = Random.Range(0, targetPoints.Count);
            } while (nextIndex == currentTargetIndex);
        }

        currentTargetIndex = nextIndex;
        transform.position = targetPoints[currentTargetIndex].position;
    }

    public void StartRewind()
    {
        isRewinding = true;
    }

    public void StopRewind()
    {
        isRewinding = false;
    }
}
