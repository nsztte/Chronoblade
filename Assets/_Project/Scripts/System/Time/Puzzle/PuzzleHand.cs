using System.Collections.Generic;
using UnityEngine;

public class PuzzleHand : MonoBehaviour, IRewindable, ITimeControllable
{
    [SerializeField] private bool isRight = true;
    [SerializeField] private float rotationSpeed = 60f;

    private float timeScale = 1f;
    [SerializeField] private bool isRewinding = false;

    // 테스트 이후에는 OnEnable으로 변경
    private void Start()
    {
        TimeManager.Instance.RegisterControllable(this);
        TimeManager.Instance.RegisterRewindable(this);
    }

    private void OnDisable()
    {
        TimeManager.Instance.UnregisterControllable(this);
        TimeManager.Instance.UnregisterRewindable(this);
    }

    private void Update()
    {
        // 테스트 이후에는 삭제
        float distance = Vector3.Distance(transform.position, PlayerManager.Instance.transform.position);

        if(distance <= 100f)
        {
            GameManager.Instance.ChangeState(GameManager.Instance.puzzleState);
        }

        // 방향 설정
        float direction = isRight ? 1 : -1;
        if(isRewinding) direction *= -1;

        float rotationThisFrame = rotationSpeed * Time.deltaTime * timeScale;
        transform.Rotate(0, 0, rotationThisFrame * direction);
    }

    public void StartRewind()
    {
        isRewinding = true;
    }

    public void StopRewind()
    {
        isRewinding = false;
    }

    public void SetTimeScale(float timeScale)
    {
        this.timeScale = timeScale;
    }

    public float GetTimeScale()
    {
        return timeScale;
    }

    public void ApplySnapshot(RewindSnapshot snapshot)
    {
    }

    public void RecordSnapshot()
    {
    }
}
