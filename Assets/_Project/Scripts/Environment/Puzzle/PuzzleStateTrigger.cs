using System.Collections;
using UnityEngine;

public class PuzzleStateTrigger : MonoBehaviour
{
    [SerializeField] private GameObject puzzleRoomDoor;
    [SerializeField] private GameObject puzzleObjects;
    [SerializeField] private PuzzleRoomManager puzzleRoomManager;

    private bool hasCachedInitialStates = false;
    private bool isActive = false;
    private bool isCleared = false;
    private Coroutine waitSubscribeCo;

    private Collider col;

    public bool IsActive
    {
        get => puzzleRoomManager ? puzzleRoomManager.IsActivated : isActive;
        set
        {
            if (puzzleRoomManager)
                puzzleRoomManager.ChangeState(value);
            else
                isActive = value;
        }
    }

    public bool IsCleared
    { 
        get => puzzleRoomManager ? puzzleRoomManager.IsCleared : isCleared;
        set => isCleared = value; 
    }


    private void Start()
    {
        col = GetComponent<Collider>();

        // 최초 진입 시 1회 캐싱
        if (!hasCachedInitialStates)
        {
            puzzleRoomManager?.CacheInitialStates();
            hasCachedInitialStates = true;
        }

        // 씬 초기 상태에 맞춰 비주얼 정렬
        OnAfterLoadCommon();
    }

    private void OnEnable()
    {
        TrySubscribeOrWait();
    }

    private void OnDisable()
    {
        // 구독 해제
        if (SaveManager.Instance != null)
            SaveManager.Instance.OnAfterLoad -= OnAfterLoadCommon;

        // 지연 구독 코루틴 정리
        if (waitSubscribeCo != null)
        {
            StopCoroutine(waitSubscribeCo);
            waitSubscribeCo = null;
        }
    }

    private void Update()
    {
        if(IsCleared)
        {
            GameManager.Instance.EnterExploration();
            if (puzzleRoomDoor) puzzleRoomDoor.SetActive(false);
            if (puzzleObjects) puzzleObjects.SetActive(false);
            gameObject.SetActive(false);

            // if(subscribed && SaveManager.Instance != null)
            // {
            //     SaveManager.Instance.OnAfterLoad -= OnAfterLoadCommon;
            //     subscribed = false;
            // }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(GameManager.Instance.CurrentGameState is ExplorationState && !IsCleared && !isActive)
            {
                GameManager.Instance.EnterPuzzle();
                IsActive = true;

                // // 로드 이벤트 구독: 미클리어 상태 동안만 유지
                // if (!subscribed && SaveManager.Instance != null)
                // {
                //     SaveManager.Instance.OnAfterLoad += OnAfterLoadCommon;
                //     subscribed = true;
                // }

                StartCoroutine(EnterPuzzleRoutine());
            }
            else if(GameManager.Instance.CurrentGameState is PuzzleState && isActive)
            {
                GameManager.Instance.EnterExploration();
                IsActive = false;
            }
        }
    }

    private void OnAfterLoadCommon()
    {
        if (puzzleRoomManager == null) return;

        if (IsCleared)
        {
            if(col) col.enabled = false;
            return;
        }

        if (IsActive)
        {
            ActivePuzzleRoomDoor();
            ActivePuzzleObjects();
            if (col) col.enabled = false;
        }
        else
        {
            // 비활성 상태 → 초기 스냅샷으로만 복원
            puzzleRoomManager.ResetToInitialIfUncleared();
            if (col) col.enabled = true;
        }
    }

    private void TrySubscribeOrWait()
    {
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.OnAfterLoad += OnAfterLoadCommon;
        }
        else
        {
            if (waitSubscribeCo == null)
                waitSubscribeCo = StartCoroutine(WaitAndSubscribe());
        }
    }

    private IEnumerator WaitAndSubscribe()
    {
        const float timeout = 5f;
        float t = 0f;
        while (SaveManager.Instance == null && t < timeout)
        {
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        waitSubscribeCo = null;

        if (SaveManager.Instance != null)
            SaveManager.Instance.OnAfterLoad += OnAfterLoadCommon;
    }

    private IEnumerator EnterPuzzleRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        PushPlayerForward();

        yield return new WaitForSeconds(0.4f);
        ActivePuzzleRoomDoor();
        ActivePuzzleObjects();

        // 활성화가 끝난 다음 프레임에 스냅샷
        yield return null;
        // puzzleRoomManager?.CacheInitialStates();

        // 입장 스냅샷 확보용 오토세이브
        SaveManager.Instance?.AutoSave("퍼즐 시작");
    }

    private void ActivePuzzleRoomDoor()
    {
        if(puzzleRoomDoor != null)
        {
            puzzleRoomDoor.SetActive(true);
            transform.GetComponent<Collider>().enabled = false;
        }
    }

    private void ActivePuzzleObjects()
    {
        if(puzzleObjects == null) return;

        Transform[] objs = puzzleObjects.GetComponentsInChildren<Transform>(true);

        if(objs.Length > 0)
        {
            foreach(var obj in objs)
            {
                obj.gameObject.SetActive(true);
            }
        }
    }

    private void PushPlayerForward()
    {
        var player = PlayerManager.Instance;
        if (player == null) return;

        var controller = player.GetComponent<CharacterController>();
        if (controller == null) return;

        // 플레이어 기준 앞으로 밀기
        Vector3 forward = player.transform.forward;
        forward.y = 0f;
        forward.Normalize();

        controller.Move(forward * 0.5f);
    }
}
