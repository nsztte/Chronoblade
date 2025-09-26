using UnityEngine;

public abstract class PoolBase : MonoBehaviour
{
    [Header("풀 공통 설정")]
    [Tooltip("풀링할 프리팹")]
    [SerializeField] protected GameObject prefab;

    [Tooltip("초기 생성 개수")]
    [SerializeField] protected int initialSize = 12;

    [Tooltip("풀 확장 최대치 (0 = 무제한)")]
    [SerializeField] protected int maxSize = 32;

    [Tooltip("풀에서 항목이 없을 때 자동으로 생성할지 여부")]
    [SerializeField] protected bool expandIfEmpty = true;

    [Tooltip("풀 인스턴스의 부모 (디폴트: 이 오브젝트)")]
    [SerializeField] protected Transform poolParent;

    public GameObject Prefab => prefab;

    protected virtual void Reset()
    {
        poolParent = this.transform;
    }

    protected virtual void Awake()
    {
        if (poolParent == null) poolParent = transform;
    }

    /// <summary>초기화</summary>
    public abstract void InitPool();

    /// <summary>모든 사용중 항목을 즉시 반환</summary>
    public abstract void ReturnAll();
}
