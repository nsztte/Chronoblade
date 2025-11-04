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
    private Collider col;

    // 리와인드 설정
    private List<int> rewindIndices = new();
    private bool isRewinding = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<Collider>();
    }

    private void Start()    // 상황에 따라서 OnEnable로 변경 가능성 있음
    {
        TimeManager.Instance.RegisterControllable(this);
        TimeManager.Instance.RegisterRewindable(this);

        StartCoroutine(MoveRoutine());
    }

    private void Update()
    {
        if (TimeManager.Instance != null && col != null)
        {
            bool isNormal = TimeManager.Instance.CurrentTimeState == TimeState.Normal;
            if (col.enabled == isNormal)
            {
                col.enabled = !isNormal;
            }
        }
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
        float acc = 0f;
        while (true)
        {
            // 정지면 누적을 멈추고 다음 프레임
            if (timeScale <= 0f) { yield return null; continue; }

            acc += Time.deltaTime * timeScale;
            if (acc >= interval)
            {
                MoveToNextPoint();
                acc = 0f;
            }

            yield return null;
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
