using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPropertiesUI : MonoBehaviour
{
    public GameObject panel;
    public Toggle physicsToggle;
    public TMP_InputField massInput;
    public Toggle gravityToggle;
    public GameObject gravityFields;
    public TMP_InputField strengthInput;
    public TMP_InputField rangeInput;
    public TMP_InputField minimumDistanceInput;
    private PhysicsObject selected;
    private GravitySource source;
    private bool loading;

    void Start()
    {
        physicsToggle.onValueChanged.AddListener(ChangePhysics);
        massInput.onEndEdit.AddListener(ChangeMass);
        gravityToggle.onValueChanged.AddListener(ChangeGravity);
        strengthInput.onEndEdit.AddListener(ChangeStrength);
        rangeInput.onEndEdit.AddListener(ChangeRange);
        minimumDistanceInput.onEndEdit.AddListener(ChangeMinimumDistance);
        Hide();
    }

    public void Show(PhysicsObject obj)
    {
        selected = obj;
        source = obj.GetComponent<GravitySource>();
        if (!source) source = obj.gameObject.AddComponent<GravitySource>();
        loading = true;
        panel.SetActive(true);
        physicsToggle.isOn = selected.isPhysics;
        massInput.text = selected.mass.ToString("F2");
        gravityToggle.isOn = source.enabledGravity;
        strengthInput.text = source.strength.ToString("F2");
        rangeInput.text = source.range.ToString("F2");
        minimumDistanceInput.text = source.minimumDistance.ToString("F2");
        gravityFields.SetActive(source.enabledGravity);
        massInput.interactable = selected.isPhysics;
        loading = false;
    }

    public void Hide() { selected = null; source = null; if (panel) panel.SetActive(false); }
    void ChangePhysics(bool value)
    {
        if (loading || !selected) return;
        selected.isPhysics = value;
        selected.ApplySettings();
        massInput.interactable = value;
    }
    void ChangeMass(string text)
    {
        if (loading || !selected) return;
        if (float.TryParse(text, out float value)) selected.mass = Mathf.Max(0.01f, value);
        selected.ApplySettings();
        massInput.SetTextWithoutNotify(selected.mass.ToString("F2"));
    }
    void ChangeGravity(bool value)
    {
        if (loading || !source) return;
        source.enabledGravity = value;
        gravityFields.SetActive(value);
    }
    void ChangeStrength(string text)
    {
        if (loading || !source) return;
        if (float.TryParse(text, out float value)) source.strength = Mathf.Max(0, value);
        strengthInput.SetTextWithoutNotify(source.strength.ToString("F2"));
    }
    void ChangeRange(string text)
    {
        if (loading || !source) return;
        if (float.TryParse(text, out float value)) source.range = Mathf.Max(0, value);
        rangeInput.SetTextWithoutNotify(source.range.ToString("F2"));
    }
    void ChangeMinimumDistance(string text)
    {
        if (loading || !source) return;
        if (float.TryParse(text, out float value)) source.minimumDistance = Mathf.Max(0.01f, value);
        minimumDistanceInput.SetTextWithoutNotify(source.minimumDistance.ToString("F2"));
    }
}
