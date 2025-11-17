using System.Collections.Generic;
using UnityEngine;

public class FixedItemPool : MonoBehaviour
{
    [Header("슬롯으로 만들 ItemPickup 프리팹")]
    [SerializeField] private GameObject itemPrefab;

    [Header("슬롯 개수")]
    [SerializeField] private int count = 10;

    private List<ItemPickup> items = new List<ItemPickup>();
    private readonly Queue<ItemPickup> availableQueue = new Queue<ItemPickup>();
    private readonly HashSet<ItemPickup> activeSet = new HashSet<ItemPickup>();

    private void Awake()
    {
        items.Clear();
        GetComponentsInChildren(true, items);

        InitializePool();
    }

    private void InitializePool()
    {
        availableQueue.Clear();
        activeSet.Clear();

        foreach (var pickup in items)
        {
            if (pickup == null) continue;

            pickup.gameObject.SetActive(false);
            pickup.transform.SetParent(transform, true);
            availableQueue.Enqueue(pickup);
        }
    }

    public ItemPickup Get()
    {
        if (availableQueue.Count == 0)
        {
            Debug.LogWarning("[FixedItemPool] 사용 가능한 아이템 슬롯이 더 이상 없음", this);
            return null;
        }

        var pickup = availableQueue.Dequeue();
        activeSet.Add(pickup);
        pickup.gameObject.SetActive(true);
        return pickup;
    }

    public void Release(ItemPickup pickup)
    {
        if (pickup == null) return;
        if (!activeSet.Contains(pickup)) return;

        activeSet.Remove(pickup);
        pickup.gameObject.SetActive(false);
        pickup.transform.SetParent(transform, true);
        availableQueue.Enqueue(pickup);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (itemPrefab == null) return;
        if (Application.isPlaying) return; // 런타임 동안은 건드리지 말기

        // 자식에 있는 ItemPickup 전부 다시 스캔
        items.Clear();
        GetComponentsInChildren(true, items);

        // 이미 슬롯이 있는 경우에는 "추가 생성"만 허용
        int current = items.Count;

        if (current < count)
        {
            int toCreate = count - current;
            for (int i = 0; i < toCreate; i++)
            {
                var go = Instantiate(itemPrefab, transform);
                go.name = $"{itemPrefab.name}_{current + i}";
                go.SetActive(false);
                
                var id = go.GetComponent<SaveId>();
                id?.Regenerate();
            }

            // 다시 리스트 업데이트
            items.Clear();
            GetComponentsInChildren(true, items);
        }
        else if (current > count)
        {
            // 경고
            Debug.LogWarning(
                $"[FixedItemPool] 현재 슬롯 {current}개 > count {count}개. " +
                $"슬롯 삭제는 기존 세이브와 호환이 깨지니, 의도한 변경인지 확인 필요.",
                this
            );
        }
    }
#endif
}
