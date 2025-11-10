using System.Collections;
using UnityEngine;

public class GuideLightMover : MonoBehaviour
{
    [Header("설정")]
    [SerializeField] private int roomId = 0;

    [Header("경로")]
    [SerializeField] private Transform[] waypoints;

    [Header("이동")]
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float startDelay = 0.5f;

    private Coroutine co;
    private bool isActive;
    private bool didRepeat = false;
    public int RoomId => roomId;

    private void Awake()
    {
        SetVisible(false);
    }

    private void OnEnable()
    {
        PuzzleProgressManager.Instance.OnKeyInserted += HandleKeyInserted;
    }

    private void OnDisable()
    {
        PuzzleProgressManager.Instance.OnKeyInserted -= HandleKeyInserted;
        StopGuide();
    }

    public void StartGuide()
    {
        StopGuide();
        if (co != null) StopCoroutine(co);
        co = StartCoroutine(CoRunGuide());
    }

    public void StopGuide()
    {
        isActive = false;
        if (co != null)
        {
            StopCoroutine(co);
            co = null;
        }
        SetVisible(false);
    }

    private void HandleKeyInserted(int nextRoomId)
    {
        if (nextRoomId == roomId)
            StartGuide();
    }

    private IEnumerator CoRunGuide()
    {
        if (!ValidatePath()) yield break;
        yield return new WaitForSeconds(startDelay);
        isActive = true;
        SetVisible(true);
        yield return MoveAlongPathOnce();

        if (roomId == 3 && !didRepeat)
        {
            didRepeat = true;

            // 1회 더 재실행
            yield return new WaitForSeconds(startDelay);
            if (!ValidatePath()) { SetVisible(false); isActive = false; co = null; yield break; }

            isActive = true;
            SetVisible(true);
            yield return MoveAlongPathOnce();
        }

        SetVisible(false);
        isActive = false;
        co = null;
    }

    private IEnumerator MoveAlongPathOnce()
    {
        transform.position = waypoints[0].position;

        for (int i = 1; i < waypoints.Length; i++)
        {
            Vector3 a = transform.position;
            Vector3 b = waypoints[i].position;
            float dist = Vector3.Distance(a, b);
            float dur = Mathf.Max(0.0001f, dist / Mathf.Max(0.01f, speed));
            float t = 0f;

            while (t < 1f && isActive)
            {
                t += Time.deltaTime / dur;
                transform.position = Vector3.Lerp(a, b, t);
                yield return null;
            }

            transform.position = b;
            if (!isActive) yield break;
        }
    }

    private bool ValidatePath()
    {
        if (waypoints == null || waypoints.Length < 2)
        {
            Debug.LogWarning($"[GuideLightMover] waypoints 미설정/2개 미만: {name}");
            return false;
        }
        return true;
    }

    private void SetVisible(bool on)
    {
        foreach (var r in GetComponentsInChildren<Renderer>(true))
            r.enabled = on;

        foreach (var l in GetComponentsInChildren<Light>(true))
            l.enabled = on;
    }
}
