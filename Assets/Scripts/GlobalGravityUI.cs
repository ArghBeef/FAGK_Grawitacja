using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlobalGravityUI : MonoBehaviour
{
    public GravityManager manager;
    public Toggle enabledToggle;
    public TMP_InputField directionX, directionY, directionZ, strengthInput;

    void Start()
    {
        enabledToggle.isOn = manager.globalGravityEnabled;
        directionX.text = manager.gravityDirection.x.ToString("F2");
        directionY.text = manager.gravityDirection.y.ToString("F2");
        directionZ.text = manager.gravityDirection.z.ToString("F2");
        strengthInput.text = manager.gravityStrength.ToString("F2");
        enabledToggle.onValueChanged.AddListener(value => manager.globalGravityEnabled = value);
        directionX.onEndEdit.AddListener(_ => UpdateDirection());
        directionY.onEndEdit.AddListener(_ => UpdateDirection());
        directionZ.onEndEdit.AddListener(_ => UpdateDirection());
        strengthInput.onEndEdit.AddListener(UpdateStrength);
    }
    void UpdateDirection()
    {
        if (float.TryParse(directionX.text, out float x) &&
            float.TryParse(directionY.text, out float y) &&
            float.TryParse(directionZ.text, out float z))
            manager.gravityDirection = new Vector3(x, y, z);
    }
    void UpdateStrength(string value)
    {
        if (float.TryParse(value, out float strength)) manager.gravityStrength = Mathf.Max(0, strength);
        strengthInput.SetTextWithoutNotify(manager.gravityStrength.ToString("F2"));
    }
}
