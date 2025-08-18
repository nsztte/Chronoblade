using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeStatue : MonoBehaviour, ITimeControllable, IRewindable
{
    [SerializeField] private List<Transform> wayPoints;
    [SerializeField] private float moveInterval = 2f;
    [SerializeField] private float moveSpeed = 2f;
    
    private float timeScale = 1f;
    private int currentIndex = 0;
    private int direction = 1;
    private bool isRewinding = false;

    private Coroutine moveRoutine;
    

    private void Start()
    {
        if(wayPoints.Count > 0)
        {
            Vector3 target = wayPoints[0].position;
            target.y = transform.position.y;
            transform.position = target;
            moveRoutine = StartCoroutine(MoveRoutine());
        }
    }

    private void OnEnable()
    {
        TimeManager.Instance.RegisterControllable(this);
        TimeManager.Instance.RegisterRewindable(this);
    }

    private void OnDisable()
    {
        TimeManager.Instance.UnregisterControllable(this);
        TimeManager.Instance.UnregisterRewindable(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerMazeController playerStatue))
        {
            playerStatue.SetPossessed(false);   // 플레이어 빙의 해제
            playerStatue.Reset();               // 플레이어 조각상 위치 초기화
        }
    }

    private IEnumerator MoveRoutine()
    {
        while(true)
        {
            if(wayPoints.Count == 0 || timeScale == 0f)
            {
                yield return null;
                continue;
            }

            yield return MoveToNextPoint();

            float elapsed = 0f;

            while(elapsed < moveInterval)
            {
                elapsed += Time.deltaTime * timeScale;
                yield return null;
            }
        }
    }

    private IEnumerator MoveToNextPoint()
    {
        Vector3 start = transform.position;

        currentIndex += direction;

        if (currentIndex >= wayPoints.Count)
        {
            currentIndex = wayPoints.Count - 2;
            direction = -1;
        }
        else if (currentIndex < 0)
        {
            currentIndex = 1;
            direction = 1;
        }

        Vector3 target = wayPoints[currentIndex].position;
        target.y = transform.position.y;

        Vector3 directionVec = (target - start).normalized;

        // float angle = 0f;
        // float threshold = 5f;

        // if (Vector3.Angle(directionVec, Vector3.forward) < threshold) angle = 0f;
        // else if (Vector3.Angle(directionVec, Vector3.right) < threshold) angle = 90f;
        // else if (Vector3.Angle(directionVec, Vector3.back) < threshold) angle = 180f;
        // else if (Vector3.Angle(directionVec, Vector3.left) < threshold) angle = 270f;

        Vector3[] directions = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };
        float[] angles = directions.Select(dir => Vector3.Angle(directionVec, dir)).ToArray();
        int closest = System.Array.IndexOf(angles, angles.Min());
        float angle = closest * 90f;

        if(!isRewinding)
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        
        float t = 0f;
        while(t < 1f)
        {
            t += Time.deltaTime * moveSpeed * timeScale;
            transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }

        transform.position = target;
    }

    private void RestartRoutine()
    {
        direction *= -1;

        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(MoveRoutine());
    }

    public void SetTimeScale(float timeScale)
    {
        this.timeScale = timeScale;
    }

    public float GetTimeScale()
    {
        return timeScale;
    }

    public void StartRewind()
    {
        if (isRewinding) return;
        isRewinding = true;
        RestartRoutine();
    }

    public void StopRewind()
    {
        if (!isRewinding) return;
        isRewinding = false;
        RestartRoutine();
    }
}
