using UnityEngine;

public class BreakableObject : MonoBehaviour {
    [SerializeField] private GameObject brokenPrefab;

    public void TriggerDestroy() {
        var trf = transform;
        var obj = Instantiate(brokenPrefab, trf.position, trf.rotation * Quaternion.Euler(0, 0, 22.5f));

        foreach (Transform child in obj.transform.GetChild(0))
            if (child.TryGetComponent(out Rigidbody rb))
                rb.AddForce(Random.insideUnitSphere*1500);

        Destroy(gameObject);
    }
}
