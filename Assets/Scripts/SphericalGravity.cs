using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalGravity : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    
    private void Awake()
    {
        rb.useGravity = false;
    }

    private void FixedUpdate()
    {
        rb.velocity += -transform.position.normalized * (9.81f * Time.fixedDeltaTime);
    }
}
