using System;
using UnityEngine;

public class TeleportPortal : MonoBehaviour
{
    [SerializeField] private Transform teleportPosition;
    private Collider col;

    private void Awake()
    {
        col = GetComponent<Collider>();

        if(col != null)
            col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            // 순간이동 연출 추가
            other.transform.position = teleportPosition.position;
            other.transform.rotation = teleportPosition.rotation;
        }
    }
}
