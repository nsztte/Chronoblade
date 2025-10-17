using UnityEngine;

public class CoreBootstrap : MonoBehaviour
{
    private static bool isCreated;

    private void Awake()
    {
        if (isCreated) { Destroy(gameObject); return; }
        isCreated = true;

        DontDestroyOnLoad(gameObject);

        // 인풋 시스템
        GetComponentInChildren<InputManager>(true)?.Initialize();

        // 저장 시스템
        GetComponentInChildren<SaveManager>(true)?.Initialize();
        GetComponentInChildren<SaveGuard>(true)?.Initialize();

        // 게임시스템
        GetComponentInChildren<GameManager>(true)?.Initialize();
        GetComponentInChildren<TimeManager>(true)?.Initialize();
        GetComponentInChildren<TimeInputHandler>(true)?.Initialize();
        GetComponentInChildren<AudioManager>(true)?.Initialize();

        // 게임플레이
        GetComponentInChildren<ItemManager>(true)?.Initialize();
        GetComponentInChildren<InventoryManager>(true)?.Initialize();
        GetComponentInChildren<WeaponManager>(true)?.Initialize();
        GetComponentInChildren<EnemyManager>(true)?.Initialize();
        GetComponentInChildren<ShopManager>(true)?.Initialize();
        GetComponentInChildren<TimingComboManager>(true)?.Initialize();
        GetComponentInChildren<ComboEvaluator>(true)?.Initialize();

        // 프레젠테이션
        GetComponentInChildren<QuickSlotManager>(true)?.Initialize();
        GetComponentInChildren<CutsceneCameraManager>(true)?.Initialize();
        GetComponentInChildren<VFXManager>(true)?.Initialize();

        // 플레이어
        GetComponentInChildren<PlayerManager>(true)?.Initialize();
    }
}
