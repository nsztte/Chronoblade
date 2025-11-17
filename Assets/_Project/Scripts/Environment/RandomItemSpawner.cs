using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItemSpawner : SaveableBehaviour
{
    [Header("아이템 슬롯 풀")]
    [SerializeField] private FixedItemPool fixedItemPool;

    [Header("스폰 설정")]
    [SerializeField] private int count = 10;
    [SerializeField] private Vector3 areaSize = new Vector3(10, 0, 10);
    [SerializeField] private bool spawnOnStart = true;

    [Header("바닥 맞춤 설정")]
    [SerializeField] private float raycastHeight = 5f;      // 스폰 기준점에서 얼마나 위에서 쏠지
    [SerializeField] private float raycastDistance = 50f;   // 최대 얼마까지 아래로 쏠지
    [SerializeField] private float surfaceOffset = 0.05f;   // 바닥에서 얼마나 띄울지
    [SerializeField] private LayerMask groundMask = ~0;

    [Header("겹침 / 가림 검사")]
    [SerializeField] private LayerMask obstacleMask;   // 위에서 덮거나 옆에서 겹치면 안 되는 레이어
    [SerializeField] private float overlapRadius = 0.3f;   // 아이템 반경 대략 값
    [SerializeField] private float headroomHeight = 0.5f;  // 아이템 위로 필요한 여유 높이

    [Header("아이템 간 최소 거리")]
    [SerializeField] private float minDistanceBetweenItems = 1.5f;


    private const int MAX_ATTEMPTS = 50;
    private readonly List<Vector3> spawnedPositions = new List<Vector3>();

    [SerializeField] private bool hasSpawned;

    private void Start()
    {
        if (!spawnOnStart) return;

        StartCoroutine(SpawnAfterRestore());
    }

    private IEnumerator SpawnAfterRestore()
    {
        // RestoreStateJson이 SaveManager에서 호출될 프레임을 넘겨주기 위해 지연
        yield return null;
        yield return null;

        // 세이브에서 hasSpawned=true로 복원되었다면 다시 스폰하지 않는다
        if (hasSpawned) yield break;

        SpawnItems();
        hasSpawned = true;
    }

    public void SpawnItems()
    {
        if (fixedItemPool == null)
        {
            Debug.LogWarning($"[RandomItemSpawner] {name}: FixedItemPool이 비어 있음", this);
            return;
        }

        spawnedPositions.Clear();

        for (int i = 0; i < count; i++)
        {
            if (!TryGetValidPosition(out Vector3 worldPos))
            {
                Debug.LogWarning($"[RandomItemSpawner] {name}: 유효한 위치를 찾지 못해 스폰 포기 (index {i})", this);
                continue;
            }

            var pickup = fixedItemPool.Get();
            if (pickup == null)
            {
                Debug.LogWarning($"[RandomItemSpawner] {name}: FixedItemPool에서 더 이상 사용할 수 있는 슬롯이 없음 (index {i})", this);
                break;
            }

            // 부모를 이 스포너 밑으로 붙이고 위치/회전 지정
            pickup.transform.SetParent(transform, true);
            pickup.transform.position = worldPos;
            pickup.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
            pickup.gameObject.SetActive(true);

            spawnedPositions.Add(worldPos);
        }
    }

    private bool TryGetValidPosition(out Vector3 worldPos)
    {
        worldPos = Vector3.zero;

        for (int attempt = 0; attempt < MAX_ATTEMPTS; attempt++)
        {
            // XZ 랜덤 샘플
            Vector3 localPos = new Vector3(
                Random.Range(-areaSize.x * 0.5f, areaSize.x * 0.5f),
                0f,
                Random.Range(-areaSize.z * 0.5f, areaSize.z * 0.5f)
            );
            Vector3 projected = transform.TransformPoint(localPos);

            // 위에서 아래로 바닥 찾기
            Vector3 origin = projected + Vector3.up * raycastHeight;

            if (!Physics.Raycast(origin, Vector3.down, out var hit, raycastDistance, groundMask))
                continue; // 바닥 못 찾으면 다음 시도

            Vector3 basePos = hit.point + Vector3.up * surfaceOffset;

            // 겹침 / 가림 검사
            if (!IsPositionFree(basePos))
                continue;

            worldPos = basePos;
            return true;
        }

        return false; // MAX_ATTEMPTS 동안 못 찾음
    }

    private bool IsPositionFree(Vector3 basePos)
    {
        // 1) 주변에 다른 콜라이더가 겹치는지
        Vector3 checkCenter = basePos + Vector3.up * overlapRadius * 0.5f;
        if (Physics.CheckSphere(checkCenter, overlapRadius, obstacleMask, QueryTriggerInteraction.Ignore))
            return false;

        // 2) 위쪽에 뭔가 씌워져 있는지 (턱 아래로 들어가는 상황 방지)
        if (Physics.Raycast(basePos + Vector3.up * 0.01f, Vector3.up, headroomHeight, obstacleMask, QueryTriggerInteraction.Ignore))
            return false;

        // 3) 이미 스폰된 아이템들과의 최소 거리 검사
        for (int i = 0; i < spawnedPositions.Count; i++)
        {
            if (Vector3.Distance(basePos, spawnedPositions[i]) < minDistanceBetweenItems)
                return false;
        }

        return true;
    }

    // SaveableBehaviour 구현부
    [System.Serializable]
    private class Data
    {
        public bool hasSpawned;
    }

    public override string CaptureStateJson()
    {
        var data = new Data
        {
            hasSpawned = this.hasSpawned
        };
        return JsonUtility.ToJson(data);
    }

    public override void RestoreStateJson(string json)
    {
        if (string.IsNullOrEmpty(json)) return;

        var data = JsonUtility.FromJson<Data>(json);
        hasSpawned = data.hasSpawned;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.15f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.up * 0f, areaSize);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector3.up * 0f, areaSize);
    }

    private void OnValidate()
    {
        if (fixedItemPool == null)
        {
            Debug.LogWarning($"[RandomItemSpawner] {name}: FixedItemPool이 비어 있습니다.", this);
        }

        if (count < 0) count = 0;
    }
#endif
}
