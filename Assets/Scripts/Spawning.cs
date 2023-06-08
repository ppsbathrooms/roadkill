using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawning : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private GameObject chicken;
    [SerializeField] private GameObject coop;
    [SerializeField] private float spawnRadius;

    [Space][Header("Settings")]
    [SerializeField, Range(0f, 10000f)] private float maxChickens;
    [SerializeField, Range(0f, 1500f)] private float maxCoops;
    [SerializeField, Range(0f, 0.5f)] private float spawnTime;
    
    private void Start()
    {
        InvokeRepeating(nameof(SpawnChicken), spawnTime, spawnTime);
        InvokeRepeating(nameof(SpawnCoops), spawnTime, spawnTime);
    }

    private void SpawnChicken()
    {
        if (Settings.instance.chickenContainer.hierarchyCount >= maxChickens)
            return;

        Vector3 pos = Random.onUnitSphere * spawnRadius;
        
        Vector3 up = pos.normalized;
        Vector3 forward = Random.onUnitSphere;
        
        Quaternion rotation = Quaternion.LookRotation(up, forward);
        
        
        Instantiate(chicken, pos, rotation, Settings.instance.chickenContainer);
    }
    private void SpawnCoops()
    {
        if (Settings.instance.coopContainer.hierarchyCount >= maxCoops)
            return;

        Vector3 pos = Random.onUnitSphere * spawnRadius;

        Vector3 up = pos.normalized;
        Vector3 forward = Random.onUnitSphere;

        Quaternion rotation = Quaternion.LookRotation(up, forward);


        Instantiate(coop, pos, rotation, Settings.instance.chickenContainer);
    }
}
