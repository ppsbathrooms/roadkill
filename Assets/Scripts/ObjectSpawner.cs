using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectSpawner : MonoBehaviour
{
    public static ObjectSpawner instance;

    private void Awake() { instance = this; }

    [Header("General")]
    [SerializeField] private Transform planet = null;
    [SerializeField] private float maxSpawnDistanceFromPlayer;
    
    [System.Serializable]
    private class ObjectToSpawn
    {
        public GameObject prefab;
        public Transform parent;
        public int maxAllowed;
        public float timeBetweenSpawns;

        public IEnumerator SpawnCycle()
        {
            if (parent.childCount >= maxAllowed)
                yield break;

            while (true)
            {
                yield return new WaitForSeconds(timeBetweenSpawns);
                Vector3 pos = GenerateSpawnPosition();
                Quaternion rotation = GenerateSpawnRotaiton(pos);

                Instantiate(prefab, pos, rotation, parent);
            }
        }
    }
    [Space, Header("ObjectsToSpawn")] 
    [SerializeField] private ObjectToSpawn[] objectsToSpawn;
    
    private static float planetRadius;
    private static float maxSpawnAngleFromPlayer;
    private static Transform player;

    private void Start()
    {
        planetRadius = planet.transform.localScale.x*5f;
        maxSpawnAngleFromPlayer = maxSpawnDistanceFromPlayer / planetRadius * Mathf.Rad2Deg;
        player = CarController.instance.transform;
        
        foreach (ObjectToSpawn objectToSpawn in objectsToSpawn)
            StartCoroutine(objectToSpawn.SpawnCycle());
    }

    private static Vector3 GenerateSpawnPosition()
    {
        Vector3 randomPoint = Random.insideUnitSphere;
        randomPoint *= maxSpawnAngleFromPlayer;

        Quaternion randomRotation = Quaternion.Euler(randomPoint);
        return randomRotation * player.transform.position; // vec4 * vec3 to apply rotation
    }

    private static Quaternion GenerateSpawnRotaiton(Vector3 spawnPosition)
    {
        Vector3 up = spawnPosition.normalized;
        Vector3 forward = Random.onUnitSphere;
        
        return Quaternion.LookRotation(up, forward);
    }
}
