using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ObjectSelector : MonoBehaviour
{
    public Camera sceneCamera;
    public ObjectSpawner spawner;
    public ObjectPropertiesUI propertiesUI;
    public LayerMask selectableMask;

    void Update()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null || !mouse.leftButton.wasPressedThisFrame || mouse.rightButton.isPressed ||
            (spawner != null && spawner.IsPlacing)) return;
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

        Camera cam = sceneCamera != null ? sceneCamera : Camera.main;
        if (cam == null || propertiesUI == null) return;

        if (Physics.Raycast(cam.ScreenPointToRay(mouse.position.ReadValue()), out RaycastHit hit, 1000f, selectableMask))
        {
            PhysicsObject obj = hit.collider.GetComponentInParent<PhysicsObject>();
            if (obj != null) { propertiesUI.Show(obj); return; }
        }
        propertiesUI.Hide();
    }
}
