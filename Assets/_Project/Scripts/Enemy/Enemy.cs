using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public void TakeDamage(int damage)
    {
        Debug.Log("공격 당함");
    }
}
