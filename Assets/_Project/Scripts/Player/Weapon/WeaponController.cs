using UnityEngine;
using System.Collections;

public abstract class WeaponController : MonoBehaviour
{
    public WeaponData weaponData;
    protected float coolTime = 0.5f;
    [SerializeField] protected bool isAttacking = false;

    protected virtual void Start() => RegisterInput();
    protected virtual void OnDisable() => UnregisterInput();

    protected virtual void RegisterInput()
    
    {
        if(weaponData.weaponType == WeaponType.Rifle)
        {
            InputManager.Instance.OnAttackHeld += OnAttackInput;
        }
        else
        {
            InputManager.Instance.OnAttackPressed += OnAttackInput;
        }
    }

    protected virtual void UnregisterInput()
    {
        if(weaponData.weaponType == WeaponType.Rifle)
        {
            InputManager.Instance.OnAttackHeld -= OnAttackInput;
        }
        else
        {
            InputManager.Instance.OnAttackPressed -= OnAttackInput;
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

    protected abstract void OnAttackInput();

    protected abstract void Attack();

    // 애니메이션 이벤트 메서드들 (기본 구현은 빈 메서드)
    public virtual void OnMeleeAttackHit() { }
    public virtual void OnMeleeAttackEnd() { }
}
