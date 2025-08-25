using System.Collections;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    [SerializeField] private float interactRadius = 2f;
    [SerializeField] private float promptInterval = 0.2f;

    private IInteractable currentTarget;

    private void Start()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnInteract += OnHandleInteract;

        StartCoroutine(PromptUpdateRoutine());
    }

    private void OnDestroy()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnInteract -= OnHandleInteract;
    }

    private IEnumerator PromptUpdateRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(promptInterval);
        while (true)
        {
            UpdatePrompt();
            yield return wait;
        }
    }

    private void UpdatePrompt()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactRadius);
        IInteractable closest = null;
        float minDistance = float.MaxValue;

        foreach (var hit in hits)
        {
            var interactable = hit.GetComponent<IInteractable>();
            if (interactable != null)
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = interactable;
                }
            }
        }

        if (closest != currentTarget)
        {
            currentTarget = closest;

            if (closest != null)
            {
                // PlayerHUD.Instance.ShowPrompt($"[F] {closest.GetPrompt()}");
            }
            else
            {
                // PlayerHUD.Instance.HidePrompt();
            }
        }
    }

    private void OnHandleInteract()
    {
        if(currentTarget != null)
            currentTarget.Interact();
    }
}
