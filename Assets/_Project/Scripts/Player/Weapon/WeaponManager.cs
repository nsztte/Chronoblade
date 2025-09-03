using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    [Header("무기 슬롯")]
    [SerializeField] private List<WeaponController> weaponSlots;
    private int currentWeaponIndex = -1;
    [SerializeField] private WeaponController currentWeapon;
    public WeaponController CurrentWeapon => currentWeapon;
    private PlayerController playerController;


    #region Singleton
    public static WeaponManager Instance { get; private set; }

    public void Initialize()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning($"{GetType().Name} 인스턴스 감지됨, 초기화 스킵");
            return;
        }
        Instance = this;
    }
    #endregion

    private void Start()
    {
        // InputManager.Instance.OnWeaponSwitch += OnWeaponSwitch;

        playerController = PlayerManager.Instance.PlayerController;
    }

    // private void OnDestroy()
    // {
    //     InputManager.Instance.OnWeaponSwitch -= OnWeaponSwitch;
    // }

    public void EquipWeaponByItem(ItemData item)
    {
        if(!CanSwitchWeapon()) return;

        for(int i = 0; i < weaponSlots.Count; i++)
        {
            if(weaponSlots[i].ItemData == item)
            {
                EquipWeapon(i);
                return;
            }
        }
    }
    
    // private void OnWeaponSwitch(int index)
    // {
    //     // 무기 전환 시도
    //     bool weaponChanged = EquipWeapon(index);
        
    //     // 무기 전환이 실제로 성공했을 때만 조준 취소
    //     if (weaponChanged)
    //     {
    //         CameraController.Instance?.CancelAim();
    //         CameraController.Instance?.UpdateRecoilRecoverySpeed();
    //     }
    // }

    private bool EquipWeapon(int index)
    {
        if(!CanSwitchWeapon()) return false;

        if(index < 0 || index >= weaponSlots.Count) return false;

        var weapon = weaponSlots[index];

        if (!InventoryManager.Instance.IsWeaponObtained(weapon.ItemData))
        {
            Debug.LogWarning($"[WeaponManager] {weapon.ItemData.itemID} 은 아직 획득하지 않았습니다.");
            return false;
        }

        if(currentWeapon != null)
        {
            currentWeapon.gameObject.SetActive(false);
        }

        currentWeapon = weaponSlots[index];
        currentWeaponIndex = index;
        currentWeapon.SetWeaponData(currentWeapon.weaponData);
        currentWeapon.gameObject.SetActive(true);

        if (currentWeapon is GunWeaponController gun)
        {
            gun.UpdateAmmoCount();
        }

        // 인벤토리 연동
        InventoryManager.Instance.Equip(currentWeapon.weaponData);

        // 무기 타입이 Sword인지 판별하여 애니메이터 bool 변경
        bool isSword = currentWeapon.weaponData.weaponType == WeaponType.Sword;
        bool isGun = currentWeapon.weaponData.weaponType != WeaponType.Sword;
        PlayerManager.Instance?.SetAnimatorBool("IsSwordHeld", isSword);
        PlayerManager.Instance?.SetAnimatorBool("IsGunHeld", isGun);

        // UI 활성화
        UIManager.Instance?.SetCrosshairActive(true);
        UIManager.Instance?.UpdateCrosshair(currentWeapon.weaponData.weaponType);
        UIManager.Instance?.ActiveWeaponPanel(currentWeapon.weaponData.iconSprite);
        UIManager.Instance?.ActiveAmmoPanel(isGun);
        
        
        return true;
    }

    public void UnEquipWeapon()
    {
        if(currentWeapon != null)
        {
            currentWeapon.gameObject.SetActive(false);
            
            // 인벤토리 연동
            InventoryManager.Instance.Unequip(currentWeapon.weaponData);
        }
        currentWeapon = null;
        currentWeaponIndex = -1;

        // 무기 해제 시 조준 취소
        CameraController.Instance?.CancelAim();
        // 반동 복구 속도 업데이트
        CameraController.Instance?.UpdateRecoilRecoverySpeed();
        // 애니메이터 상태 해제
        PlayerManager.Instance?.SetAnimatorBool("IsSwordHeld", false);
        PlayerManager.Instance?.SetAnimatorBool("IsGunHeld", false);

        // UI 비활성화
        UIManager.Instance?.SetCrosshairActive(false);
        UIManager.Instance?.ActiveWeaponPanel();
        UIManager.Instance?.ActiveAmmoPanel(false);

        // 조준 상태 초기화 및 반동 복구 갱신
        CameraController.Instance?.CancelAim();
        CameraController.Instance?.UpdateRecoilRecoverySpeed();
    }

    private bool CanSwitchWeapon()
    {
        var playerManager = PlayerManager.Instance;
        if(playerManager != null && playerManager.PlayerStateMachine != null)
        {
            var state = playerManager.PlayerStateMachine.CurrentState;
            if(state != null && (state is PlayerAttackState || state is PlayerComboState))
            {
                return false;
            }
        }

        if(currentWeapon != null && currentWeapon.IsAttacking)
        {
            return false;
        }

        return true;
    }

    public int GetCurrentWeaponIndex()
    {
        return currentWeaponIndex;
    }

    public int GetMaxWeaponCount()
    {
        return weaponSlots.Count;
    }

    public List<WeaponController> GetWeaponSlots()
    {
        return weaponSlots;
    }
}
