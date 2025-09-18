using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T> : PoolBase where T : Component
{
    private readonly Queue<T> pool = new Queue<T>();
    private readonly HashSet<T> activeSet = new HashSet<T>();
    private int createdCount = 0;
    protected bool CanCreateMore => (maxSize == 0) || (createdCount < maxSize);
    
    public override void InitPool()
    {
        // 이미 충분히 초기화 됐으면 리턴
        if(createdCount > 0 && pool.Count + activeSet.Count >= initialSize) return;

        for(int i = 0; i < initialSize; i++)
        {
            var inst = CreateInstance();
            if (inst != null) pool.Enqueue(inst);
        }
    }

    protected virtual T CreateInstance()
    {
        if (prefab == null)
        {
            Debug.LogError($"[{name}] 풀 프리팹 null");
            return null;
        }

        var go = Instantiate(prefab, poolParent);
        go.SetActive(false);
        var comp = go.GetComponent<T>();
        if (comp == null)
        {
            Debug.LogError($"[{name}] 프리팹 {typeof(T).Name} 컴포넌트 없음");
            Destroy(go);
            return null;
        }

        createdCount++;
        return comp;
    }

    /// <summary>풀에서 항목 꺼내기(없으면 확장 or null)</summary>
    public virtual T Get()
    {
        T item = null;

        if(pool.Count > 0)
        {
            item = pool.Dequeue();
        }
        else if(expandIfEmpty && CanCreateMore)
        {
            item = CreateInstance();
        }
        else
        {
            // 확장 불가 시 null 반환
            return null;
        }

        if(item = null) return null;

        activeSet.Add(item);
        item.gameObject.SetActive(true);
        return item;
    }

    /// <summary>항목 반환</summary>
    public virtual void Release(T item)
    {
        if(item = null) return;

        // 제거할 수 없으면 (이미 반환되었거나 해당 소속이 아님) -> 비활성화 후 큐에 넣음
        if(!activeSet.Remove(item))
        {
            item.gameObject.SetActive(false);
            if (!pool.Contains(item)) pool.Enqueue(item);
            return;
        }

        // 기본 정리: 비활성화 + 부모 재설정 후 큐에 넣음
        item.gameObject.SetActive(false);
        item.transform.SetParent(poolParent, true);
        pool.Enqueue(item);
    }

    /// <summary>시간 기반 자동 반환 헬퍼</summary>
    public Coroutine ReleaseAfter(T item, float seconds)
    {
        if (item == null) return null;
        return StartCoroutine(ReleaseAfterCoroutine(item, seconds));
    }

    private IEnumerator ReleaseAfterCoroutine(T item, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (item != null) Release(item);
    }

    public override void ReturnAll()
    {
        var list = new List<T>(activeSet);
        foreach (var it in list)
        {
            if (it == null) continue;
            var go = it.gameObject;
            go.SetActive(false);
            it.transform.SetParent(poolParent, true);
            activeSet.Remove(it);
            pool.Enqueue(it);
        }
    }

    // 상태 확인용
    public int AvailableCount => pool.Count;
    public int ActiveCount => activeSet.Count;
    public int TotalCreated => createdCount;
}