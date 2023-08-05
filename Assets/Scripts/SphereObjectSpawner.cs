using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Util;
using Random = UnityEngine.Random;

public class SphereObjectSpawner : MonoBehaviour
{
    public static SphereObjectSpawner Instance;

    private void Awake() { Instance = this; }

    [Header("General")]
    [SerializeField] private Transform _planet;
    [SerializeField] private float _maxSpawnDistanceFromPlayer;

    [Serializable]
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
        planetRadius = _planet.transform.localScale.x*5f;
        maxSpawnAngleFromPlayer = _maxSpawnDistanceFromPlayer / planetRadius * Mathf.Rad2Deg;
        player = CarController.Instance.transform;
    }

    private List<Coroutine> _spawnCycles = new();

    public void StartSpawning() {
        objectsToSpawn.ForEach(objToSpawn => {
            _spawnCycles.Add(StartCoroutine(objToSpawn.SpawnCycle()));
        });
    }

    public void StopSpawning() {
        _spawnCycles.ForEach(StopCoroutine);
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