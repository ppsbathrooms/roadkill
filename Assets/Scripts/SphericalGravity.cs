using UnityEngine;

public class SphericalGravity : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    
    private void Awake()
    {
        rb.useGravity = true;
    }

    private void FixedUpdate()
    {
        //rb.velocity += -transform.position.normalized * (9.81f * Time.fixedDeltaTime);
    }
}
