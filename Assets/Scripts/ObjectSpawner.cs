using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectSpawner : MonoBehaviour {
    public static ObjectSpawner Instance;

    private void Awake() {
        Instance = this;
    }

    [SerializeField] private GameObject chicken;
    [SerializeField] private Vector3 boundsMin;
    [SerializeField] private Vector3 boundsMax;

    [SerializeField] private float maxChickens;
    [SerializeField] private float spawnTime;

    public void StartSpawning() =>
        InvokeRepeating(nameof(SpawnChicken), spawnTime, spawnTime);

    public void StopSpawning() {
        Debug.Log("stop spawning");
        CancelInvoke(nameof(SpawnChicken));
    }

    private void SpawnChicken() {
        Debug.Log(Time.time);
        if (Settings.instance.chickenContainer.hierarchyCount >= maxChickens)
            return;

        Vector3 pos = new Vector3(Random.Range(boundsMin.x, boundsMax.x), Random.Range(boundsMin.y, boundsMax.y),
            Random.Range(boundsMin.z, boundsMax.z));
        float distance = Vector3.Distance(new Vector3(0, 0, 0), pos);
        if (distance < 48.5f)
            Instantiate(chicken, pos, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f),
                Settings.instance.chickenContainer);
    }
}
