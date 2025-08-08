using UnityEngine;

public class RewindableObjects : MonoBehaviour, ITimeControllable, IRewindable
{
    [Header("기본 설정")]
    [SerializeField] private PuzzleRoomManager puzzleRoomManager;
    [SerializeField] private float restorationThreshold = 0.05f;

    private Collider rootCollider;
    private Rigidbody[] childrenRb;
    private Vector3[] originalPositions;
    private Quaternion[] originalRotations;

    private float timeScale = 1f;
    private bool isRewinding = false;
    private bool isStarted = false;

    private void Awake()
    {
        // 부모 콜라이더 끄기
        rootCollider = GetComponent<Collider>();
        if(rootCollider != null)
            rootCollider.enabled = false;

        // 자식 조각들 정보 캐싱
        childrenRb = GetComponentsInChildren<Rigidbody>();

        originalPositions = new Vector3[childrenRb.Length];
        originalRotations = new Quaternion[childrenRb.Length];

        for(int i = 0; i < childrenRb.Length; i++)
        {
            originalPositions[i] = childrenRb[i].transform.position;
            originalRotations[i] = childrenRb[i].transform.rotation;
        }

        SetChildrenPhysics(false);
    }

    private void Start()
    {
        TimeManager.Instance?.RegisterControllable(this);
        TimeManager.Instance?.RegisterRewindable(this);
    }

    private void OnDisable()
    {
        TimeManager.Instance?.UnregisterControllable(this);
        TimeManager.Instance?.UnregisterRewindable(this);
    }

    private void Update()
    {
        if(isRewinding)
        {
            rootCollider.enabled = false;
            SetChildrenPhysics(false);
        }
        else
        {
            if(isStarted)
            {
                Restoring();
            }
        }

        if(puzzleRoomManager.IsActivated && !isStarted)
        {
            isStarted = true;
        }
    }

    private void SetChildrenPhysics(bool enabled)
    {
        for(int i = 0; i < childrenRb.Length; i++)
        {
            childrenRb[i].isKinematic = enabled;
        }
    }

    private void Restoring()
    {
        bool allRestored = true;

        for (int i = 0; i < childrenRb.Length; i++)
        {
            childrenRb[i].isKinematic = true;

            childrenRb[i].transform.position = Vector3.Lerp(
                childrenRb[i].transform.position, originalPositions[i], Time.deltaTime * 5f * timeScale);

            childrenRb[i].transform.rotation = Quaternion.Slerp(
                childrenRb[i].transform.rotation, originalRotations[i], Time.deltaTime * 5f * timeScale);

            if (Vector3.Distance(childrenRb[i].transform.position, originalPositions[i]) > restorationThreshold)
            {
                allRestored = false;
            }
        }

        if(allRestored)
        {
            isStarted = false;
            rootCollider.enabled = true;
        }
    }

    public void StartRewind()
    {
        isRewinding = true;
    }

    public void StopRewind()
    {
        isRewinding = false;
        isStarted = true;
    }

    public void SetTimeScale(float timeScale)
    {
        this.timeScale = timeScale;
    }

    public float GetTimeScale()
    {
        return timeScale;
    }
}
