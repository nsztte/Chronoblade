using UnityEngine;

public class PuzzlePlate : MonoBehaviour, IInteractable
{
    [SerializeField] private float rotationStep = 90f;
    [SerializeField] private float rotationSpeed = 10f;

    private Quaternion targetRotation;
    private Quaternion correctRotation;

    private void Awake()
    {
        correctRotation = transform.localRotation;

        int randomTurns = Random.Range(0, 4);
        transform.Rotate(Vector3.up, randomTurns * rotationStep);
        targetRotation = transform.localRotation;
    }

    private void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void Rotate()
    {
        targetRotation *= Quaternion.Euler(0f, rotationStep, 0f);
    }

    public bool IsCorrect()
    {
        float angle = Quaternion.Angle(transform.rotation, correctRotation);
        return angle < 5f;
    }

    public void Interact()
    {
        Rotate();
    }
}
