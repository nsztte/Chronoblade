using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    [Header("무기 슬롯")]
    [SerializeField] private List<WeaponController> weaponSlots;
    private int currentWeaponIndex = -1;
    private WeaponController currentWeapon;
    public WeaponController CurrentWeapon => currentWeapon;

    #region Singleton
    public static WeaponManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private void Start()
    {
        InputManager.Instance.OnWeaponSwitch += OnWeaponSwitch;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnWeaponSwitch -= OnWeaponSwitch;
    }
    
    private void OnWeaponSwitch(int index)
    {
        // 무기 전환 시 조준 취소
        CameraController.Instance?.CancelAim();
        // 반동 복구 속도 업데이트
        CameraController.Instance?.UpdateRecoilRecoverySpeed();
        // 무기 전환
        EquipWeapon(index);
    }

    private void EquipWeapon(int index)
    {
        if(index < 0 || index >= weaponSlots.Count) return;

        if(currentWeapon != null)
        {
            currentWeapon.gameObject.SetActive(false);
        }

        currentWeapon = weaponSlots[index];
        currentWeaponIndex = index;
        currentWeapon.SetWeaponData(currentWeapon.weaponData);
        currentWeapon.gameObject.SetActive(true);

        // 무기 타입이 Sword인지 판별하여 애니메이터 bool 변경
        bool isSword = currentWeapon.weaponData.weaponType == WeaponType.Sword;
        bool isGun = currentWeapon.weaponData.weaponType != WeaponType.Sword;
        PlayerManager.Instance?.SetAnimatorBool("IsSwordHeld", isSword);
        PlayerManager.Instance?.SetAnimatorBool("IsGunHeld", isGun);
    }

    public void UnEquipWeapon()
    {
        if(currentWeapon != null)
        {
            currentWeapon.gameObject.SetActive(false);
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
