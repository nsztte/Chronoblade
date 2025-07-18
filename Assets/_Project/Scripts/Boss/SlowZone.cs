using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SlowZone : MonoBehaviour
{
    [SerializeField] private float duration = 5f;

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void Start()
    {
        Destroy(gameObject, duration);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent(out IStatusEffectable effectable))
        {
            effectable.ApplyStatus(StatusEffectType.Slow);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out IStatusEffectable effectable))
        {
            effectable.RemoveStatus(StatusEffectType.Slow);
        }
    }
}
