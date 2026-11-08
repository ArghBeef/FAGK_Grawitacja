
using UnityEngine;
using UnityEngine.UI;

public class SpawnPreview : MonoBehaviour
{
    [Header("Preview")]
    public GameObject prefab;
    public RawImage rawImage;

    [Header("Settings")]
    public int textureSize = 256;
    public float rotationSpeed = 20f;
    public float cameraDistance = 3f;

    private GameObject previewObject;
    private GameObject previewRoot;
    private Camera previewCamera;
    private RenderTexture renderTexture;

    void Start()
    {
        if (prefab == null || rawImage == null)
            return;

        previewRoot = new GameObject("Preview_" + prefab.name);

        // Keep each preview far from other previews
        int id = GetInstanceID() & 0xFFFF;
        previewRoot.transform.position =
            new Vector3(id * 10f, -10000f, 0f);

        previewObject = Instantiate(
            prefab,
            previewRoot.transform
        );

        previewObject.transform.localPosition = Vector3.zero;
        previewObject.transform.localRotation = Quaternion.identity;

        // Disable physics and simulation scripts
        foreach (Rigidbody rb in
            previewObject.GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        foreach (MonoBehaviour script in
            previewObject.GetComponentsInChildren<MonoBehaviour>())
        {
            script.enabled = false;
        }

        // Preview-only layer
        int layer = LayerMask.NameToLayer("Preview3D");

        if (layer < 0)
        {
            Debug.LogError("Create Preview3D layer first!");
            return;
        }

        SetLayer(previewObject, layer);

        // Create camera
        GameObject cameraObject = new GameObject("PreviewCamera");
        cameraObject.transform.SetParent(previewRoot.transform);

        previewCamera = cameraObject.AddComponent<Camera>();

        previewCamera.cullingMask = 1 << layer;
        previewCamera.clearFlags = CameraClearFlags.SolidColor;
        previewCamera.backgroundColor = new Color(0, 0, 0, 0);
        previewCamera.orthographic = true;
        previewCamera.orthographicSize = 1.5f;
        previewCamera.enabled = true;

        cameraObject.transform.localPosition =
            new Vector3(0, 1, -cameraDistance);

        cameraObject.transform.LookAt(previewObject.transform.position);

        // Create individual RenderTexture
        renderTexture = new RenderTexture(
            textureSize,
            textureSize,
            16
        );

        renderTexture.Create();

        previewCamera.targetTexture = renderTexture;
        rawImage.texture = renderTexture;
    }

    void Update()
    {
        if (previewObject != null)
        {
            previewObject.transform.Rotate(
                Vector3.up,
                rotationSpeed * Time.deltaTime,
                Space.World
            );
        }
    }

    void SetLayer(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach (Transform child in obj.transform)
            SetLayer(child.gameObject, layer);
    }

    void OnDestroy()
    {
        if (previewCamera != null)
            previewCamera.targetTexture = null;

        if (renderTexture != null)
        {
            renderTexture.Release();
            Destroy(renderTexture);
        }

        if (previewRoot != null)
            Destroy(previewRoot);
    }
}
