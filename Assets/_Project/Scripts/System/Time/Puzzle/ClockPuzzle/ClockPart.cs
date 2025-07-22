using UnityEngine;
using System.Collections;

public class ClockPart : MonoBehaviour
{
    [Header("설정")]
    [SerializeField] private int damage = 30;
    [SerializeField] private float minLaunchForce = 5f;
    [SerializeField] private float maxLaunchForce = 10f;
    [SerializeField] private float homingDelay = 0.5f;
    [SerializeField] private float homingSpeed = 8f;
    [SerializeField] private float returnSpeed = 4f;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Rigidbody rb;
    private Collider col;
    private Transform player;
    private Transform boss;
    private Transform target;
    private Transform parentClock;

    [SerializeField] private bool isHoming = false;
    [SerializeField] private bool hasArrived = false;
    public bool IsHoming => isHoming;
    public bool HasArrived => hasArrived;

    private void Awake()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;

        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        col.isTrigger = true;

        player = PlayerManager.Instance.PlayerTransform;
        parentClock = transform.parent;
        boss = parentClock.parent;
    }

    private void OnEnable()
    {
        ForceReset();
    }

    private void Update()
    {
        if(isHoming && !hasArrived)
        {
            Vector3 targetPosition = target.position;
            Collider targetCollider = target.GetComponent<Collider>();

            if(targetCollider != null)
            {
                targetPosition.y = targetCollider.bounds.center.y;
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, homingSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isHoming) return;

        if(other.CompareTag("Player") && target == player)
        {
            if(other.GetComponentInParent<IDamageable>() is IDamageable damageable)
            {
                damageable.TakeDamage(damage);
                ClockPartArrived();
            }
        }
        else if(other.CompareTag("Boss") && target == boss)
        {
            ClockPartArrived();
        }
    }

    public void SetTarget(bool isPlayer)
    {
        this.target = isPlayer ? player : boss;
    }

    public void Launch()
    {
        transform.parent = null;
        rb.isKinematic = false;
        col.enabled = true;

        Vector3 randomDirection = (player.position - transform.position).normalized;
        float launchForce = Random.Range(minLaunchForce, maxLaunchForce);
        rb.AddForce(randomDirection * launchForce, ForceMode.Impulse);

        StartCoroutine(StartHoming());
    }

    public void ForceReset()
    {
        isHoming = false;
        hasArrived = false;
        transform.SetParent(parentClock);
        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;

        rb.isKinematic = true;
        col.enabled = false;
    }

    private IEnumerator StartHoming()
    {
        yield return new WaitForSeconds(homingDelay);
        if(rb == null) yield break;

        rb.isKinematic = true;
        isHoming = true;
    }

    private void ClockPartArrived()
    {
        if(hasArrived) return;

        transform.SetParent(parentClock);
        isHoming = false;
        hasArrived = true;
        rb.isKinematic = true;
        col.enabled = false;
    }
}
