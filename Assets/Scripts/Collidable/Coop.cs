using Collidable;
using UnityEngine;

public class Coop : AbstractCollidableObject
{
    protected override void SpawnDeathEffects()
    {
        Destroy(
            Instantiate(Settings.instance.eggEffect, transform.position, Quaternion.identity, Settings.instance.effectsContainer), 2f
            );

    }
}
