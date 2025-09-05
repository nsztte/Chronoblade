using UnityEngine;

public class PuzzleStateTrigger : MonoBehaviour
{
    [SerializeField] private GameObject puzzleRoomDoor;
    [SerializeField] private GameObject puzzleObjects;
    [SerializeField] private PuzzleRoomManager puzzleRoomManager;
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

                // 퍼즐 입장 시 저장 차단
                SaveGuard.Instance?.Block(SaveBlockTag.Puzzle);

                Invoke(nameof(ActivePuzzleRoomDoor), 0.5f);
                Invoke(nameof(ActivePuzzleObjects), 0.5f);
                Invoke(nameof(PushPlayerForward), 0.1f);
            }
            else if(GameManager.Instance.CurrentGameState is PuzzleState && isActive)
            {
                GameManager.Instance.EnterExploration();
                isActive = false;
                puzzleRoomManager?.ChangeState(false);

                // 퍼즐 상태에서 이탈할 경우, 저장 복구
                SaveGuard.Instance?.ClearTag(SaveBlockTag.Puzzle);
            }
        }
    }

    private void ActivePuzzleRoomDoor()
    {
        if(puzzleRoomDoor != null)
            puzzleRoomDoor.SetActive(true);
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
