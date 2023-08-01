using UnityEngine;
using UnityEngine.EventSystems;

public class chickenController : MonoBehaviour
{   
    [Header("Settings")]
    [SerializeField, Range(0f, 50f)] private float moveSpeed;
    [SerializeField, Range(0f, 150f)] private float rotationSpeed;

    private Rigidbody rb;
    private Animator animator;

    private float horizontalInput;
    private float verticalInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

       if(verticalInput == 0f) {
            animator.SetFloat("Speed", 0);
        }
        else {
            animator.SetFloat("Speed", 1);
        }
    }

    void FixedUpdate()
    {
        if (horizontalInput != 0f)
        {
            Quaternion targetRotation = Quaternion.Euler(0f, transform.eulerAngles.y + rotationSpeed * horizontalInput * Time.fixedDeltaTime, 0f);
            transform.rotation = targetRotation;
        }

        Vector3 moveDirection = transform.forward * verticalInput;
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);

    }
}
