using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChickenController : MonoBehaviour
{   
    [Header("Settings")]
    [SerializeField, Range(0f, 50f)] private float moveSpeed;
    [SerializeField, Range(0f, 150f)] private float rotationSpeed;
    [SerializeField] private Vector3 resetPos;
    [SerializeField] private float aimSpeedMultiplier;
    [SerializeField] private GameObject pistol;

    private Rigidbody rb;
    private Animator animator;

    private float horizontalInput;
    private float verticalInput;

    private float newAimWeight;
    private bool canFire = true;
    private bool isAiming;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        Inputs();


    }

    void Inputs() {
       if(verticalInput == 0f) {
            animator.SetFloat("Speed", 0);
        }
        else {
            animator.SetFloat("Speed", 1);
        }

        if(Input.GetKeyDown(KeyCode.R)) {
            transform.position = resetPos;
        }

        if(Input.GetMouseButton(1)) {
            isAiming = true;
            newAimWeight = Mathf.Lerp(newAimWeight, 1f, aimSpeedMultiplier * Time.deltaTime);
            animator.SetLayerWeight(1, newAimWeight);
            pistol.SetActive(true);
            
        }
        else {
            isAiming = false;
            newAimWeight = Mathf.Lerp(newAimWeight, 0f, aimSpeedMultiplier * Time.deltaTime);
            animator.SetLayerWeight(1, newAimWeight);
            pistol.SetActive(false);
        }

        if(Input.GetMouseButtonDown(0) && isAiming && canFire) {
            animator.SetFloat("fire", 1);
        }
        else {
            animator.SetFloat("fire", 0);
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
