using UnityEngine;
using System.Collections;

public class PlayerMazeController : MonoBehaviour
{
    [SerializeField] private Transform startPosition;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rayDistance = 2f;
    [SerializeField] private float moveDelay = 0.25f;
    [SerializeField] private float rotationDuration = 0.2f;
    [SerializeField] private LayerMask waypointLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private GameObject PlayerStatueCamera;

    [SerializeField] private bool isPossessed = false;
    private Quaternion startRotation;
    private float lastMoveTime = -Mathf.Infinity;
    private bool isMoving = false;
    private bool isRotating = false;

    private void Start()
    {
        startRotation = transform.rotation;

        Reset();
    }

    private void Update()
    {
        if(!isPossessed || isMoving || isRotating) return;

        if (Time.time - lastMoveTime >= moveDelay)
        {
            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

            if (input != Vector3.zero)
            {
                lastMoveTime = Time.time;
                Vector3 localInput = transform.TransformDirection(input);
                TryMove(localInput);
            }
        }

        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            StartCoroutine(SmoothRotate(90f));
        }
    }

    private void TryMove(Vector3 direction)
    {
        Vector3 origin = transform.position;
        Ray ray = new Ray(origin, direction);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, waypointLayer))
        {
            // 벽과의 충돌 여부 확인
            if (!Physics.Raycast(origin, direction, hit.distance, wallLayer))
            {
                StartCoroutine(MoveToPosition(hit.transform.position));
            }
        }
    }

    private IEnumerator MoveToPosition(Vector3 target)
    {
        isMoving = true;

        Vector3 start = transform.position;
        Vector3 end = new Vector3(target.x, start.y, target.z); // 높이 고정

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        transform.position = end;
        isMoving = false;
    }

    private IEnumerator SmoothRotate(float angle)
    {
        isRotating = true;
        Quaternion startRot = transform.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(0, angle, 0);
        float elapsed = 0f;

        while (elapsed < rotationDuration)
        {
            transform.rotation = Quaternion.Slerp(startRot, endRot, elapsed / rotationDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRot;
        isRotating = false;
    }

    public void Reset()
    {        
        StopAllCoroutines();

        isMoving = false;
        isRotating = false;

        Vector3 target = startPosition.position;
        target.y = transform.position.y;
        transform.position = target;

        transform.rotation = startRotation;
    }

    public void SetPossessed(bool value)
    {
        isPossessed = value;
        PlayerStatueCamera.SetActive(value);
    }
}
