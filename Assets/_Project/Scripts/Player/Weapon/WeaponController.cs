using UnityEngine;
using System.Collections;

public abstract class WeaponController : MonoBehaviour
{
    public WeaponData weaponData;
    protected float coolTime = 0.5f;
    [SerializeField] protected bool isAttacking = false;

    // FSM에서 공격을 관리하므로 자동 입력 등록 비활성화
    // protected virtual void Start() => RegisterInput();
    // protected virtual void OnDisable() => UnregisterInput();

    protected virtual void RegisterInput()
    {
        if(weaponData.weaponType == WeaponType.Rifle)
        {
            InputManager.Instance.OnAttackHeld += ExecuteWeaponAttack;
        }
        else
        {
            InputManager.Instance.OnAttackPressed += ExecuteWeaponAttack;
        }
    }

    protected virtual void UnregisterInput()
    {
        if(weaponData.weaponType == WeaponType.Rifle)
        {
            InputManager.Instance.OnAttackHeld -= ExecuteWeaponAttack;
        }
        else
        {
            InputManager.Instance.OnAttackPressed -= ExecuteWeaponAttack;
        }
    }

    public virtual void SetWeaponData(WeaponData data)
    {
        UnregisterInput();
        weaponData = data;

        if(weaponData.weaponType == WeaponType.Sword)
            coolTime = 0.5f;
        else
            coolTime = 1f / weaponData.fireRate;

        RegisterInput();
    }

    // 총기류에서만 사용
    public virtual void ExecuteWeaponAttack() { }

    // 근접무기에서만 사용
    public virtual void ExecuteLightAttack() { }
    public virtual void ExecuteHeavyAttack() { }

    // 애니메이션 이벤트 메서드들 (기본 구현은 빈 메서드)
    public virtual void OnMeleeAttackHit() { }
    public virtual void OnMeleeAttackEnd() { }
}
