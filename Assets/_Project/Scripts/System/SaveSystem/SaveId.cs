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
    // ※주의: OnValidate 로직은 의도적으로 비활성화된 상태임.
    /*
        원래 OnValidate에서 GUID 중복 검사 및 자동 재생성을 수행했으나,
        이 방식은 "씬을 열거나 저장하는 것만으로도 GUID가 변경되는" 문제를 유발함.

        세이브/로드 시스템은 GUID를 기준으로 오브젝트 매칭을 수행하므로,
        GUID가 에디터 자동 처리(OnValidate)로 변경되면 저장된 세이브 파일과
        실제 씬 오브젝트 간 매칭이 불가능해져 로드가 깨지는 치명적인 버그가 발생함.

        예)
        - 챕터1에서 저장
        - 파이널 챕터 이동
        - 다시 챕터1 저장 파일 로드 → OnValidate가 GUID를 바꿔버려 로드 실패

        따라서 GUID는 “의도적인 경우에만 수동 Regenerate로 변경”하는 정책으로 전환함.
        OnValidate를 비활성화한 이유는 세이브 ID의 자동 변경을 완전히 차단하기 위함임.
    */
    // private void OnValidate()
    // {
    //     if (Application.isPlaying) return;

    //     // 비어 있으면 먼저 하나 생성
    //     // if (string.IsNullOrWhiteSpace(guid))
    //     //     guid = System.Guid.NewGuid().ToString("N");

    //     // 씬 안의 모든 SaveId를 가져와서 중복 체크
    //     var all = Object.FindObjectsByType<SaveId>(
    //         FindObjectsInactive.Include,
    //         FindObjectsSortMode.None
    //     );

    //     bool changed;
    //     int safety = 0;

    //     // 혹시나 또 겹칠 경우를 대비해서 몇 번까지 재시도
    //     do
    //     {
    //         changed = false;

    //         foreach (var other in all)
    //         {
    //             if (other == this) continue;
    //             if (other.guid == guid)
    //             {
    //                 guid = System.Guid.NewGuid().ToString("N");
    //                 changed = true;
    //                 break;
    //             }
    //         }

    //         safety++;
    //     }
    //     while (changed && safety < 10);

    //     UnityEditor.EditorUtility.SetDirty(this);
    // }

    [ContextMenu("GUID 재생성 (주의)")]
    public void Regenerate()
    {
        guid = System.Guid.NewGuid().ToString("N");
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif
}
