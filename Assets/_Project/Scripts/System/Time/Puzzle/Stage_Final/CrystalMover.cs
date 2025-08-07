using UnityEngine;
using System.Collections.Generic;

public class CrystalMover : MonoBehaviour, ITimeControllable, IRewindable
{
    [Header("움직임 설정")]
    [SerializeField] private bool isBouncing = false;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Vector3 initialDirection = Vector3.right;
    private Vector3 moveDirection;

    [Header("되감기 설정")]
    [SerializeField] private float recordInterval = 0.1f;
    [SerializeField] private int maxHistoryCount = 100;
    private List<Vector3> positionHistory = new List<Vector3>();
    private float recordTimer;
    private bool isRewinding = false;
    private int rewindIndex = -1;
    private float timeScale = 1f;

    [SerializeField] private ParticleSystem crystalParticles;

    private void Start()
    {
        moveDirection = initialDirection.normalized;

        if (isBouncing)
        {
            // 랜덤 회전 각도 적용
            float randomAngle = Random.Range(-45f, 45f);
            moveDirection = Quaternion.Euler(0, randomAngle, 0) * initialDirection.normalized;
        }

        TimeManager.Instance.RegisterControllable(this);
        TimeManager.Instance.RegisterRewindable(this);
    }
    
    private void OnDisable()
    {
        TimeManager.Instance.UnregisterControllable(this);
        TimeManager.Instance.UnregisterRewindable(this);
    }

    void Update()
    {
        if (isRewinding)
        {
            if (rewindIndex <= 0 || rewindIndex >= positionHistory.Count) return;

            Vector3 targetPos = positionHistory[rewindIndex];
            float step = moveSpeed * timeScale * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

            if (Vector3.Distance(transform.position, targetPos) < 0.01f)
            {
                rewindIndex--;
            }
            return;
        }

        // 이동
        transform.position += moveDirection * moveSpeed * timeScale * Time.deltaTime;

        // 저장
        recordTimer += Time.deltaTime;
        if (recordTimer >= recordInterval)
        {
            positionHistory.Add(transform.position);
            if (positionHistory.Count > maxHistoryCount)
                positionHistory.RemoveAt(0);

            rewindIndex = positionHistory.Count - 1;
            recordTimer = 0f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 normal = collision.contacts[0].normal;

        Vector3 reflectDir = Vector3.Reflect(moveDirection, normal).normalized;

        // 반사 방향에 약간의 랜덤 노이즈 추가
        Vector3 randomOffset = isBouncing ? new Vector3(
            Random.Range(-0.2f, 0.2f),
            0f,
            Random.Range(-0.2f, 0.2f)
        )
        : Vector3.zero;

        moveDirection = (reflectDir + randomOffset).normalized;
    }

    public float GetTimeScale()
    {
        return timeScale;
    }

    public void SetTimeScale(float timeScale)
    {
        this.timeScale = timeScale;
        if (crystalParticles != null)
        {
            var main = crystalParticles.main;
            main.simulationSpeed = timeScale;
        }
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
