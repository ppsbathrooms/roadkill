using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Collidable
{
    public class Chicken : AbstractCollidableObject
    {
        private NavMeshAgent agent;
        private Vector3 randomDestination;
        private float wanderRadius = 5f;

        private bool dead;

        private void Start()
        {
            InitializeChicken();
        }

        private void InitializeChicken()
        {
            agent = GetComponent<NavMeshAgent>();
            if (agent != null && agent.isActiveAndEnabled)
            {
                SetRandomDestination();
            }
            else
            {
                Debug.LogError("navmeshagent is not properly initialized or active.");
            }
        }

        private void Update()
        {
            if (!dead)
                WanderAround();
        }

        protected override void SpawnDeathEffects()
        {
            dead = true;
            agent.enabled = false;
            GetComponent<Animator>().enabled = false;

            base.SpawnDeathEffects();
            Instantiate(Settings.instance.deathEffect, transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)), transform);
            GameObject feather = Instantiate(Settings.instance.featherEffect, transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
            Destroy(feather, 2f);
        }

        private void WanderAround()
        {
            if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
            {
                if (agent.remainingDistance < 0.1f && !agent.pathPending)
                {
                    SetRandomDestination();
                }
            }
        }

        private void SetRandomDestination()
        {
            int maxAttempts = 5;
            for (int i = 0; i < maxAttempts; i++)
            {
                randomDestination = transform.position + Random.insideUnitSphere * wanderRadius;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomDestination, out hit, wanderRadius, NavMesh.AllAreas))
                {
                    Vector3 finalPosition = hit.position;
                    agent.SetDestination(finalPosition);
                    return;
                }
            }

            // Debug.LogWarning("failed to find a valid position for the destination after " + maxAttempts + " attempts.");
        }
    }
}
