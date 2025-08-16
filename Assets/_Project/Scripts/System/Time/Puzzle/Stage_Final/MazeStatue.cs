using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeStatue : MonoBehaviour
{
    [SerializeField] private List<Transform> wayPoints;
    [SerializeField] private float moveInterval = 2f;
    [SerializeField] private float moveSpeed = 2f;
    
    private int currentIndex = 0;
    private int direction = 1;
    private bool isMoving = false;

    private void Start()
    {
        if(wayPoints.Count > 0)
        {
            Vector3 target = wayPoints[0].position;
            target.y = transform.position.y;
            transform.position = target;
            StartCoroutine(MoveRoutine());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerMazeController playerStatue))
        {
            // 플레이어 빙의 해제

            playerStatue.Reset();
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
        target.y = transform.position.y;

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
