using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SpawnItemUI : MonoBehaviour
{
    public ObjectSpawner spawner;
    public GameObject prefab;
    public bool physicsEnabled = true;

    void Awake() { GetComponent<Button>().onClick.AddListener(Spawn); }
    void Spawn() { if (spawner && prefab) spawner.SelectPrefab(prefab, physicsEnabled); }
}
