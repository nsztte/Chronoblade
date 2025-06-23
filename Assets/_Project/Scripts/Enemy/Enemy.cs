using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHP = 100;
    [SerializeField] private int damage = 10;
    [SerializeField] private float detectionRange = 10f;

    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 1.5f;
    
    private int currentHP;
    private EnemyStateMachine fsm;

    public int Damage => damage;
    public float DetectionRange => detectionRange;
    public float AttackRange => attackRange;
    public float AttackCooldown => attackCooldown;

    private void Awake()
    {
        currentHP = maxHP;
        fsm = GetComponent<EnemyStateMachine>();
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log($"Enemy HP: {currentHP}");

        if(fsm != null)
        {
            if(currentHP <= 0)
            {
                fsm.TransitionToState(fsm.DeadState);
            }
            else
            {
                fsm.TransitionToState(fsm.HitState);
            }
        }
    }

    public void Die()
    {
        Collider collider = GetComponent<Collider>();
        if(collider != null) collider.enabled = false;
        
        this.enabled = false;
        Destroy(this.gameObject, 3f);
    }
}
