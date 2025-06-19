using UnityEngine;
using System.Collections;

public abstract class WeaponController : MonoBehaviour
{
    public WeaponData weaponData;
    protected float coolTime = 0.5f;
    protected bool isAttacking = false;

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

    protected void OnAttackInput()
    {
        if(!gameObject.activeInHierarchy || isAttacking) return;
        isAttacking = true;
        StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        Attack();
        yield return new WaitForSeconds(coolTime);
        isAttacking = false;
    }

    protected abstract void Attack();
}
