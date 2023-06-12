using UnityEngine;

namespace Collidable
{
    public abstract class AbstractCollidableObject : MonoBehaviour
    {
        public int eggsWhenHit;
        public bool boostWhenHit;
        
        private bool hitByPlayer;

        public virtual bool onHitByPlayer()
        {
            if (hitByPlayer)
                return false;
            
            hitByPlayer = true;
            
            Destroy(gameObject, 2);
            SpawnDeathEffects();

            gameObject.layer = 7;
            return true;
        }

        protected virtual void SpawnDeathEffects() { }
    }
}
