using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalGravity : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;
    
    void Start()
    {
        rigidbody.useGravity = false;
    }

    void FixedUpdate()
    {
        rigidbody.velocity += -transform.position.normalized * (9.81f * Time.fixedDeltaTime);
    }
}
