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
    }

    private void Start()
    {
        TimeManager.Instance?.RegisterControllable(this);
    }

    private void OnDisable()
    {
        TimeManager.Instance?.UnregisterControllable(this);
    }

    public void SetTimeScale(float timeScale)
    {
        currentTimeScale = timeScale;

        agent.speed = baseSpeed * currentTimeScale;
        if(animator != null) animator.speed = currentTimeScale;
    }

    public void SetSpeed(float speed)
    {
        if(baseSpeed <= 0)
            baseSpeed = speed;
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
