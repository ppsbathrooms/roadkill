using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenSpawner : MonoBehaviour
{
    [SerializeField] private GameObject chicken;
    [SerializeField] private float spawnRadius;

    [SerializeField] private float maxChickens;
    [SerializeField] private float spawnTime;
    
    private void Start()
    {
        InvokeRepeating(nameof(SpawnChicken), spawnTime, spawnTime);
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
}
