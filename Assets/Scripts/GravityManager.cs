using UnityEngine;

public class GravityManager : MonoBehaviour
{
    public static GravityManager Instance { get; private set; }
    public Vector3 gravityDirection = Vector3.down;
    [Min(0)] public float gravityStrength = 9.81f;
    public bool globalGravityEnabled = true;

    void Awake()
    {
        Instance = this;
        Physics.gravity = Vector3.zero;
    }

    void FixedUpdate()
    {
        PhysicsObject[] objects = FindObjectsByType<PhysicsObject>(FindObjectsSortMode.None);
        GravitySource[] sources = FindObjectsByType<GravitySource>(FindObjectsSortMode.None);
        foreach (PhysicsObject obj in objects)
        {
            if (!obj.isPhysics || obj.Body == null || obj.Body.isKinematic) continue;
            Vector3 acceleration = globalGravityEnabled ? gravityDirection.normalized * gravityStrength : Vector3.zero;
            foreach (GravitySource source in sources)
                if (source.gameObject != obj.gameObject)
                    acceleration += source.AccelerationAt(obj.transform.position);
            obj.Body.AddForce(acceleration, ForceMode.Acceleration);
        }
    }
}
