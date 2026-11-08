using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PhysicsObject : MonoBehaviour
{
    public bool isPhysics = true;
    [Min(0.01f)] public float mass = 1f;
    private Rigidbody body;
    public Rigidbody Body => body;

    void Awake() { ApplySettings(); }

    public void ApplySettings()
    {
        if (isPhysics)
        {
            if (!TryGetComponent(out body)) body = gameObject.AddComponent<Rigidbody>();
            body.mass = Mathf.Max(0.01f, mass);
            body.isKinematic = false;
            body.useGravity = false; // GravityManager controls all gravity.
        }
        else
        {
            if (TryGetComponent(out body))
            {
                body.isKinematic = true;
                body.useGravity = false;
            }
        }
    }
}
