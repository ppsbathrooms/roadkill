using UnityEngine;

namespace Collidable
{
    public class Chicken : AbstractCollidableObject
    {
        protected override void SpawnDeathEffects()
        {
            Instantiate(Settings.instance.deathEffect, transform.position, Quaternion.identity, transform);
            Destroy(
                Instantiate(Settings.instance.featherEffect, transform.position, Quaternion.identity, Settings.instance.effectsContainer), 2f
                );
        }
    }
}
