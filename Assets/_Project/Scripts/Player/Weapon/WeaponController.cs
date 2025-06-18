using UnityEngine;
using System.Collections;

public abstract class WeaponController : MonoBehaviour
{
    public WeaponData weaponData;
    protected float coolTime = 0.5f;
    protected bool isAttacking = false;

    protected virtual void Start()
    {
        InputManager.Instance.OnAttackPressed += OnAttackInput;
    }

    protected virtual void OnDisable()
    {
        InputManager.Instance.OnAttackPressed -= OnAttackInput;
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
