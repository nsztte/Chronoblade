using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class SlowZone : MonoBehaviour
{
    [SerializeField] private float duration = 5f;
    private readonly HashSet<IStatusEffectable> inside = new();

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
            if (inside.Add(effectable))
                effectable.ApplyStatus(StatusEffectType.Slow);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out IStatusEffectable effectable))
        {
            if (inside.Remove(effectable))
                effectable.RemoveStatus(StatusEffectType.Slow);
        }
    }

    private void OnDisable()
    {
        foreach (var e in inside)
            e.RemoveStatus(StatusEffectType.Slow);
        inside.Clear();
    }
}
