using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour {
    [SerializeField] private GameObject brokenPrefab;

    public void TriggerDestroy() {
        var obj = Instantiate(brokenPrefab, transform.position, transform.rotation * Quaternion.Euler(0, 0, 22.5f));
        Destroy(obj, 5);
        foreach (Transform child in obj.transform.GetChild(0)) {
            if (child.TryGetComponent(out Rigidbody rb))
            {            
                Debug.Log(child.name);
                rb.AddForce(Random.insideUnitSphere*1000);
            }
        }
        
        Destroy(gameObject);
    }
}
