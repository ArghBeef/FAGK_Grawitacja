using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ObjectSpawner : MonoBehaviour
{
    public Camera sceneCamera;
    public GameObject spawnPanel;
    public Transform objectsParent;
    public LayerMask placementMask;
    public Material previewMaterial;
    public float placementOffset = 0.02f;

    [Header("Keys")]
    public Key spawnMenuKey = Key.Tab;
    public Key cancelKey = Key.Escape;

    private GameObject selectedPrefab;
    private GameObject preview;
    private bool selectedPhysics;

    void Start() { if (spawnPanel != null) spawnPanel.SetActive(false); }

    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        Mouse mouse = Mouse.current;
        if (keyboard != null)
        {
            if (keyboard[spawnMenuKey].wasPressedThisFrame && spawnPanel != null)
                spawnPanel.SetActive(!spawnPanel.activeSelf);
            if (keyboard[cancelKey].wasPressedThisFrame) CancelPlacement();
        }
        if (selectedPrefab == null || mouse == null) return;

        Camera cam = sceneCamera != null ? sceneCamera : Camera.main;
        if (cam == null) return;
        bool valid = Physics.Raycast(cam.ScreenPointToRay(mouse.position.ReadValue()), out RaycastHit hit, 1000f, placementMask);
        if (preview != null) preview.SetActive(valid);
        if (!valid) return;

        Vector3 position = hit.point + hit.normal * placementOffset;
        if (preview != null) preview.transform.position = position;
        if (mouse.leftButton.wasPressedThisFrame && !mouse.rightButton.isPressed &&
            !(EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()))
        {
            GameObject obj = Instantiate(selectedPrefab, position, Quaternion.identity, objectsParent);
            PhysicsObject properties = obj.GetComponent<PhysicsObject>();
            if (properties == null) properties = obj.AddComponent<PhysicsObject>();
            properties.isPhysics = selectedPhysics;
            properties.ApplySettings();
            if (!obj.TryGetComponent<GravitySource>(out _)) obj.AddComponent<GravitySource>();
        }
    }

    public void SelectPrefab(GameObject prefab, bool physicsEnabled)
    {
        CancelPlacement();
        if (prefab == null) return;
        selectedPrefab = prefab;
        selectedPhysics = physicsEnabled;
        preview = Instantiate(prefab);
        foreach (Rigidbody rb in preview.GetComponentsInChildren<Rigidbody>()) { rb.isKinematic = true; rb.useGravity = false; }
        foreach (Collider collider in preview.GetComponentsInChildren<Collider>()) collider.enabled = false;
        foreach (PhysicsObject obj in preview.GetComponentsInChildren<PhysicsObject>()) obj.enabled = false;
        foreach (GravitySource source in preview.GetComponentsInChildren<GravitySource>()) source.enabled = false;
        foreach (Renderer renderer in preview.GetComponentsInChildren<Renderer>())
            if (previewMaterial != null) renderer.material = previewMaterial;
        if (spawnPanel != null) spawnPanel.SetActive(false);
    }

    public void CancelPlacement()
    {
        selectedPrefab = null;
        if (preview != null) Destroy(preview);
        preview = null;
    }

    public bool IsPlacing => selectedPrefab != null;
}
