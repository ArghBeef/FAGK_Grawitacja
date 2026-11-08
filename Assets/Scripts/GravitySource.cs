using UnityEngine;

public class GravitySource : MonoBehaviour
{
    public bool enabledGravity = false;
    [Min(0)] public float strength = 50f;
    [Min(0.01f)] public float minimumDistance = 1f;
    [Min(0)] public float range = 30f;

    public Vector3 AccelerationAt(Vector3 position)
    {
        if (!enabledGravity) return Vector3.zero;
        Vector3 offset = transform.position - position;
        float distance = offset.magnitude;
        if (distance < 0.001f || (range > 0 && distance > range)) return Vector3.zero;
        float d = Mathf.Max(minimumDistance, distance);
        return offset.normalized * (strength / (d * d));
    }
}
