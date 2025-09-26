using UnityEngine;

public class HeartbeatLineImage : MonoBehaviour
{
    [SerializeField] private RectTransform pulseOriginPos;
    public RectTransform PulseOriginPos => pulseOriginPos;
    public RectTransform RectTransform => (RectTransform)transform;
}
