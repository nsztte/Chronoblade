using System.Collections.Generic;
using UnityEngine;

public class PuzzleHand : MonoBehaviour, IRewindable, ITimeControllable
{
    [Header("퍼즐 바늘 설정")]
    [SerializeField] private bool isRight = true;
    [SerializeField] private float rotationSpeed = 60f;
    [SerializeField] private float targetAngle;
    [SerializeField] private float angleTolerance = 3f;

    private float timeScale = 1f;
    private bool isRewinding = false;

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
        // 방향 설정
        float direction = isRight ? 1 : -1;
        if(isRewinding) direction *= -1;

        float rotationThisFrame = rotationSpeed * Time.deltaTime * timeScale;
        transform.Rotate(0, 0, rotationThisFrame * direction);
    }

    public bool IsAligned()
    {
        float currentAngle = transform.localEulerAngles.z;
        float deltaAngle = Mathf.DeltaAngle(currentAngle, targetAngle);
        return Mathf.Abs(deltaAngle) <= angleTolerance;
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
