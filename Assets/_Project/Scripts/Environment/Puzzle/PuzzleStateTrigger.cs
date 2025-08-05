using UnityEngine;

public class PuzzleStateTrigger : MonoBehaviour
{
    [SerializeField] private bool isActive = false;
    [SerializeField] private bool isCleared = false;
    public bool IsCleared { get => isCleared; set => isCleared = value; }

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
            }
            else if(GameManager.Instance.CurrentGameState is PuzzleState && isActive)
            {
                Debug.Log("퍼즐 방 나가기");
                GameManager.Instance.EnterExploration();
                isActive = false;
            }
        }
    }
}
