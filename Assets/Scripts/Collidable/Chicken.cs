using UnityEngine;

namespace Collidable
{
    public class Chicken : AbstractCollidableObject
    {
        protected override void SpawnDeathEffects()
        {
            Instantiate(Settings.instance.deathEffect, transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)), transform);
            Destroy(
                Instantiate(Settings.instance.featherEffect, transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)), Settings.instance.effectsContainer), 2f
                );
        }
    }
}
