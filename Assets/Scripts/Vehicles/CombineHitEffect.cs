using Collidable;
using UnityEngine;

public class CombineHitEffect : MonoBehaviour
{
    [SerializeField] private Transform spawnPosition;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent<AbstractCollidableObject>(
                out AbstractCollidableObject collidableObject))
        {
            Destroy(Instantiate(Settings.instance.deathEffect,
                    spawnPosition.position, spawnPosition.rotation, transform), 2f
            );
            Destroy(Instantiate(Settings.instance.featherEffect,
                    spawnPosition.position, spawnPosition.rotation, Settings.instance.effectsContainer), 2f
            );
        }
    }
}
