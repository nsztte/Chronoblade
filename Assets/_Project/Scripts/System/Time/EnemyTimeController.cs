using UnityEngine;
using UnityEngine.AI;

public class EnemyTimeController : MonoBehaviour, ITimeControllable
{
    private float currentTimeScale = 1f;
    private NavMeshAgent agent;
    private Animator animator;
    private float baseSpeed;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        baseSpeed = agent.speed;
    }

    private void OnEnable()
    {
        TimeManager.Instance?.Register(this);
    }

    private void OnDisable()
    {
        TimeManager.Instance?.Unregister(this);
    }

    public void SetTimeScale(float timeScale)
    {
        currentTimeScale = timeScale;

        agent.speed = baseSpeed * currentTimeScale;
        if(animator != null) animator.speed = currentTimeScale;
    }
    public float GetTimeScale()
    {
        return currentTimeScale;
    }

    public float GetAdjustedDeltaTime()
    {
        return Time.deltaTime * currentTimeScale;
    }
}
