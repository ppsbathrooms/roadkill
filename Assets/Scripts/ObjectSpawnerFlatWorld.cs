using System.Collections.Generic;
using UnityEngine;
public class ObjectSpawnerFlatWorld : MonoBehaviour
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
        float distance = Vector3.Distance(new Vector3(0,0,0), pos);
        if(distance < 48.5f)
            Instantiate(chicken, pos, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f), Settings.instance.chickenContainer);
    }
}