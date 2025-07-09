using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class FinalComboController : MonoBehaviour, IStatusEffectable
{
    private Animator animator;
    private NavMeshAgent agent;
    private Enemy enemy;
    private EnemyStateMachine stateMachine;

    [Header("상태 이상")]
    private StatusEffectType currentStatus;

    [Header("상태 이상 효과")]
    [SerializeField] private float slowSpeed = 0.3f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<Enemy>();
        stateMachine = GetComponent<EnemyStateMachine>();
    }

    public virtual void ApplyStatus(ComboAttackData attackData)
    {
        if(currentStatus != StatusEffectType.None) return;

        currentStatus = attackData.statusEffectType;
        switch(currentStatus)
        {
            case StatusEffectType.Freeze:
                StartCoroutine(HandleFreeze(attackData.statusDuration));
                break;
            case StatusEffectType.Slow:
                StartCoroutine(HandleSlow(attackData.statusDuration));
                break;
        }
    }

    private IEnumerator HandleFreeze(float duration)
    {
        Debug.Log($"Enemy {transform.name} 경직 적용");
        
        stateMachine.enabled = false;
        agent.isStopped = true;
        animator.speed = 0f;

        yield return new WaitForSeconds(duration);

        stateMachine.enabled = true;
        agent.isStopped = false;
        animator.speed = 1f;

        currentStatus = StatusEffectType.None;
    }

    private IEnumerator HandleSlow(float duration)
    {
        Debug.Log($"Enemy {transform.name} 슬로우 상태 적용");

        float originalSpeed = agent.speed;
        float originalAnimSpeed = animator.speed;

        agent.speed = originalSpeed * slowSpeed;
        animator.speed = originalAnimSpeed * slowSpeed;

        yield return new WaitForSeconds(duration);

        agent.speed = originalSpeed;
        animator.speed = originalAnimSpeed;
        currentStatus = StatusEffectType.None;
    }
}
