using UnityEngine;

[DisallowMultipleComponent]
public class SaveId : MonoBehaviour
{
    [SerializeField] private string guid;
    public string Guid => guid;

    public void Reset()
    {
        if(string.IsNullOrWhiteSpace(guid))
            guid = System.Guid.NewGuid().ToString("N");
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Application.isPlaying) return;

        // 비어 있으면 먼저 하나 생성
        if (string.IsNullOrWhiteSpace(guid))
            guid = System.Guid.NewGuid().ToString("N");

        // 씬 안의 모든 SaveId를 가져와서 중복 체크
        var all = Object.FindObjectsByType<SaveId>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        bool changed;
        int safety = 0;

        // 혹시나 또 겹칠 경우를 대비해서 몇 번까지 재시도
        do
        {
            changed = false;

            foreach (var other in all)
            {
                if (other == this) continue;
                if (other.guid == guid)
                {
                    guid = System.Guid.NewGuid().ToString("N");
                    changed = true;
                    break;
                }
            }

            safety++;
        }
        while (changed && safety < 10);

        UnityEditor.EditorUtility.SetDirty(this);
    }

    [ContextMenu("GUID 재생성 (주의)")]
    public void Regenerate()
    {
        guid = System.Guid.NewGuid().ToString("N");
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif
}
