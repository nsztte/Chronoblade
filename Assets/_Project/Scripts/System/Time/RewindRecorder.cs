using System.Collections.Generic;
using UnityEngine;

public class RewindRecorder : MonoBehaviour, IRewindable
{
    [Header("리와인드 기록 설정")]
    [SerializeField] private float recordDuration = 3f;
    [SerializeField] private float recordInterval = 0.05f;

    private List<RewindSnapshot> snapshots = new List<RewindSnapshot>();
    private float timeSinceLastRecord = 0f;
    private bool isRewinding = false;

    // 되감기 속도 조절
    private float rewindTimer = 0f;
    private float rewindInterval = 0.1f;         // 초기 재생 간격 (초)
    private float minInterval = 0.015f;          // 가장 빠를 때 간격
    private float intervalLerpSpeed = 2f;        // 속도 증가율

    // 되감기 위치 조절
    private Vector3 currentTargetPosition;
    private Quaternion currentTargetRotation;
    private bool hasTarget = false; // 다음 타겟 위치 설정 여부
    private float lerpSpeed = 10f;
    private float maxLerpSpeed = 50f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        TimeManager.Instance?.RegisterRewindable(this);
    }

    private void OnDisable()
    {
        TimeManager.Instance?.UnregisterRewindable(this);
    }

    private void Update()
    {
        if (isRewinding)
        {
            rewindTimer += Time.deltaTime;

            if (rewindTimer >= rewindInterval)
            {
                rewindTimer = 0f;
                SetNextRewindTarget();
            }

            if (hasTarget)
            {
                transform.position = Vector3.Lerp(transform.position, currentTargetPosition, lerpSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, currentTargetRotation, lerpSpeed * Time.deltaTime);

                // 스냅 조건: 일정 거리 이하일 때 고정
                if (Vector3.Distance(transform.position, currentTargetPosition) < 0.01f)
                {
                    transform.position = currentTargetPosition;
                    transform.rotation = currentTargetRotation;
                    hasTarget = false; // 다음 snapshot 대상으로 넘어감
                }
                else
                {
                    // 점점 더 빠르게
                    lerpSpeed = Mathf.Lerp(lerpSpeed, maxLerpSpeed, Time.deltaTime * 0.5f);
                }
            }

            // 점점 빨라지게
            rewindInterval = Mathf.Lerp(rewindInterval, minInterval, Time.deltaTime * intervalLerpSpeed);
        }
        else
        {
            // 기록 로직
            timeSinceLastRecord += Time.deltaTime;
            while (timeSinceLastRecord >= recordInterval)
            {
                RecordSnapshot();
                timeSinceLastRecord -= recordInterval;
            }

            rewindInterval = 0.1f;
        }
    }


    public void StartRewind()
    {
        isRewinding = true;
        if(rb != null)
        {
            rb.isKinematic = false;
        }
    }

    public void StopRewind()
    {
        isRewinding = false;
        if(rb != null)
        {
            rb.isKinematic = false;
        }
    }

    public void ApplySnapshot(RewindSnapshot snapshot)
    {
        transform.position = snapshot.position;
        transform.rotation = snapshot.rotation;
    }

    public void RecordSnapshot()
    {
        if(snapshots.Count >= recordDuration / recordInterval)
        {
            snapshots.RemoveAt(0);  // FIFO
        }

        snapshots.Add(new RewindSnapshot(transform.position, transform.rotation));
    }

    private void PlayRewind()
    {
        if(snapshots.Count > 0)
        {
            RewindSnapshot snapshot = snapshots[^1];
            ApplySnapshot(snapshot);
            snapshots.RemoveAt(snapshots.Count - 1);
        }
        else
        {
            rb.isKinematic = true;
        }
    }

    private void SetNextRewindTarget()
    {
        if (snapshots.Count > 0)
        {
            var snapshot = snapshots[^1];
            snapshots.RemoveAt(snapshots.Count - 1);

            currentTargetPosition = snapshot.position;
            currentTargetRotation = snapshot.rotation;
            hasTarget = true;
        }
        else
        {
            hasTarget = false;
            rb.isKinematic = true;
        }
    }
}
