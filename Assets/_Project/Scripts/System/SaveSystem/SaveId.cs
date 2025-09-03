using UnityEngine;

[DisallowMultipleComponent]
public class SaveId : MonoBehaviour
{
    [SerializeField] private string guid;
    public string Guid => guid;

    private void Reset()
    {
        if(string.IsNullOrWhiteSpace(guid))
            guid = System.Guid.NewGuid().ToString("N");
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrWhiteSpace(guid))
            guid = System.Guid.NewGuid().ToString("N");
    }

    [ContextMenu("GUID 재생성 (주의)")]
    private void Regenerate() => guid = System.Guid.NewGuid().ToString("N");
#endif
}
