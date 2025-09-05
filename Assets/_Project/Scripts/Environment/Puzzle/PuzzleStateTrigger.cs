using UnityEngine;

public class PuzzleStateTrigger : MonoBehaviour
{
    [SerializeField] private GameObject puzzleRoomDoor;
    [SerializeField] private GameObject puzzleObjects;
    [SerializeField] private PuzzleRoomManager puzzleRoomManager;

    private bool subscribed = false;
    private bool isActive = false;
    private bool isCleared = false;
    public bool IsCleared { get => puzzleRoomManager ? puzzleRoomManager.IsCleared : isCleared; set => isCleared = value; }

    private void Update()
    {
        if(IsCleared)
        {
            GameManager.Instance.EnterExploration();
            if (puzzleRoomDoor) puzzleRoomDoor.SetActive(false);
            if (puzzleObjects) puzzleObjects.SetActive(false);
            gameObject.SetActive(false);

            if(subscribed && SaveManager.Instance != null)
            {
                SaveManager.Instance.OnAfterLoad -= OnAfterLoadCommon;
                subscribed = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(GameManager.Instance.CurrentGameState is ExplorationState && !IsCleared && !isActive)
            {
                GameManager.Instance.EnterPuzzle();
                isActive = true;
                puzzleRoomManager?.ChangeState(true);

                // 로드 이벤트 구독: 미클리어 상태 동안만 유지
                if (!subscribed && SaveManager.Instance != null)
                {
                    SaveManager.Instance.OnAfterLoad += OnAfterLoadCommon;
                    subscribed = true;
                }

                Invoke(nameof(PushPlayerForward), 0.1f);
                Invoke(nameof(ActivePuzzleRoomDoor), 0.5f);
                Invoke(nameof(ActivePuzzleObjects), 0.5f);
                Invoke(nameof(AutoSave), 0.5f);
            }
            else if(GameManager.Instance.CurrentGameState is PuzzleState && isActive)
            {
                GameManager.Instance.EnterExploration();
                isActive = false;
                puzzleRoomManager?.ChangeState(false);
            }
        }
    }

    private void OnAfterLoadCommon()
    {
        // 퍼즐이 아직 미클리어면 방 초기화
        if (!IsCleared)
            puzzleRoomManager?.ResetToInitialIfUncleared();
    }

    private void AutoSave()
    {
        SaveManager.Instance?.Save(SaveManager.Instance.NextAutoSlot(), SaveIntent.Auto);
        puzzleRoomManager?.CacheInitialStates();
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
