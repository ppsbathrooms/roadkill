using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class ObjectSpawner : MonoBehaviour {
    public static ObjectSpawner Instance;

    private void Awake() {
        Instance = this;
    }

    [Header("General"), SerializeField] private ObjectToSpawn[] objectsToSpawn;
    [SerializeField] private float maxSpawnDistanceFromPlayer;
    [SerializeField] private bool sphere = false;
    [SerializeField] private Transform player;

    [Space, Header("Flat World Spawning"), SerializeField]
    private Vector3 boundsMin;

    [SerializeField] private Vector3 boundsMax;

    [Space, Header("Sphere World Spawning"), SerializeField]
    private Transform planet;

    private float planetRadius;
    private float maxSpawnAngleFromPlayer;

    private void Start() {
        if (planet) {
            planetRadius = planet.transform.localScale.x * 5f;
            maxSpawnAngleFromPlayer = maxSpawnDistanceFromPlayer / planetRadius * Mathf.Rad2Deg;
        }

        foreach (ObjectToSpawn objectToSpawn in objectsToSpawn)
            StartCoroutine(objectToSpawn.SpawnCycle(GenerateSpawnPosition, GenerateSpawnRotation));
    }

    [Serializable]
    private class ObjectToSpawn {
        public GameObject prefab;
        public Transform parent;
        public int maxAllowed;
        public float timeBetweenSpawns;

        private ObjectPool<GameObject> objectPool;

        public ObjectToSpawn() {
            objectPool = new ObjectPool<GameObject>(() => Instantiate(prefab, parent),
                o => o.SetActive(true), o => o.SetActive(false),
                Destroy, defaultCapacity: 100);
        }

        public IEnumerator SpawnCycle(Func<Vector3> getNextPos, Func<Vector3, Quaternion> getNextRotation) {
            while (true) {
                yield return new WaitForSeconds(timeBetweenSpawns);

                if (parent.childCount >= maxAllowed)
                    continue;

                var obj = objectPool.Get();
                obj.transform.position = getNextPos.Invoke();
                obj.transform.rotation = getNextRotation.Invoke(obj.transform.position);
            }
        }
    }

    private Vector3 GenerateSpawnPosition() {
        if (sphere) {
            Vector3 randomPoint = Random.insideUnitSphere;
            randomPoint *= maxSpawnAngleFromPlayer;

            Quaternion randomRotation = Quaternion.Euler(randomPoint);
            return randomRotation * player.transform.position; // vec4 * vec3 to apply rotation
        }
        else {
            float distance = Random.Range(0, maxSpawnDistanceFromPlayer);

            Vector2 pos = Random.insideUnitCircle * distance;

            pos.x = Mathf.Clamp(pos.x, boundsMin.x, boundsMax.z);
            pos.y = Mathf.Clamp(pos.y, boundsMin.z, boundsMax.z);

            return new Vector3(pos.x, 0, pos.y);
        }
    }

    private Quaternion GenerateSpawnRotation(Vector3 spawnPosition) {
        if (sphere) {
            Vector3 up = spawnPosition.normalized;
            Vector3 forward = Random.onUnitSphere;

            return Quaternion.LookRotation(up, forward);
        }
        else return Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
    }
}
