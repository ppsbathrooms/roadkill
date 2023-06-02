using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenSpawner : MonoBehaviour
{
    [SerializeField] private GameObject chicken;
    [SerializeField] private Vector3 boundsMin;
    [SerializeField] private Vector3 boundsMax;

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

        Vector3 pos = new Vector3(Random.Range(boundsMin.x, boundsMax.x), Random.Range(boundsMin.y, boundsMax.y),
                Random.Range(boundsMin.z, boundsMax.z));

        Instantiate(chicken, pos, Quaternion.Euler(-90f, Random.Range(0f, 360f), 0f), Settings.instance.chickenContainer);
    }
}
