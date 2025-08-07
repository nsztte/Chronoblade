using UnityEngine;

public class PuzzleStateTrigger : MonoBehaviour
{
    [SerializeField] private GameObject puzzleRoomDoor;
    [SerializeField] private GameObject puzzleObjects;
    private bool isActive = false;
    private bool isCleared = false;
    public bool IsCleared { get => isCleared; set => isCleared = value; }

    private void Start()
    {
        if(puzzleRoomDoor != null)
            puzzleRoomDoor.SetActive(false);
    }

    private void Update()
    {
        if(isCleared)
        {
            GameManager.Instance.EnterExploration();
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(GameManager.Instance.CurrentGameState is ExplorationState && !isCleared && !isActive)
            {
                Debug.Log("퍼즐 방 진입");
                GameManager.Instance.EnterPuzzle();
                isActive = true;

                Invoke(nameof(ActivePuzzleRoomDoor), 0.5f);
                Invoke(nameof(ActivePuzzleObjects), 0.5f);              
            }
            else if(GameManager.Instance.CurrentGameState is PuzzleState && isActive)
            {
                Debug.Log("퍼즐 방 나가기");
                GameManager.Instance.EnterExploration();
                isActive = false;
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
}
