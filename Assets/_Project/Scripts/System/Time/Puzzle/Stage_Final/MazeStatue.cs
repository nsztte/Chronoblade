using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class MazeStatue : MonoBehaviour
{
    [SerializeField] private Transform wayPoint;
    [SerializeField] private float moveInterval = 2f;
    [SerializeField] private float moveSpeed = 2f;
    
    private List<Transform> wayPoints;
    private int currentIndex = 0;
    private int direction = 1;
    private bool isMoving = false;

    private void Awake()
    {
        if(wayPoint != null)
            wayPoints = wayPoint.GetComponentsInChildren<Transform>().Skip(1).ToList();
    }

    private void Start()
    {
        if(wayPoint != null && wayPoints.Count > 0)
        {
            transform.position = wayPoints[0].position;
            StartCoroutine(MoveRoutine());
        }
    }

    private IEnumerator MoveRoutine()
    {
        while(true)
        {
            if(!isMoving)
                StartCoroutine(MoveToNextPoint());

            yield return new WaitForSeconds(moveInterval);
        }
    }

    private IEnumerator MoveToNextPoint()
    {
        isMoving = true;

        Vector3 start = transform.position;

        currentIndex += direction;

        if (currentIndex >= wayPoints.Count)
        {
            currentIndex = wayPoints.Count - 2;
            direction = -1;
        }
        else if (currentIndex < 0)
        {
            currentIndex = 1;
            direction = 1;
        }

        Vector3 target = wayPoints[currentIndex].position;

        Vector3 directionVec = (target - start).normalized;

        float angle = 0f;
        float threshold = 5f;

        if (Vector3.Angle(directionVec, Vector3.forward) < threshold) angle = 0f;
        else if (Vector3.Angle(directionVec, Vector3.right) < threshold) angle = 90f;
        else if (Vector3.Angle(directionVec, Vector3.back) < threshold) angle = 180f;
        else if (Vector3.Angle(directionVec, Vector3.left) < threshold) angle = 270f;

        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        float t = 0f;
        while(t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(start, target, t);

            yield return null;
        }

        transform.position = target;
        isMoving = false;
    }
        
}
