using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CarController : MonoBehaviour
{

    [Header("Refs")]
    [SerializeField] private WheelCollider backLeft;
    [SerializeField] private WheelCollider backRight;
    [SerializeField] private WheelCollider frontLeft;
    [SerializeField] private WheelCollider frontRight;

    [SerializeField] private Transform backLeftTransform;
    [SerializeField] private Transform backRightTransform;
    [SerializeField] private Transform frontLeftTransform;
    [SerializeField] private Transform frontRightTransform;

    [SerializeField] private GameObject brakeLights;
    [SerializeField] private TMP_Text speedText;
    [SerializeField] private Rigidbody rb;

    [Space] [Header("Settings")]
    [SerializeField] private float polloMultiplier = 200;
    [SerializeField] private float boostMultiplier = 1f;
    [SerializeField] private float acceleration = 500f;
    [SerializeField] private float breakingForce = 300f;
    [SerializeField] private float maxTurnAngle = 15f;
    [SerializeField] private float motorForce = 1f;
    
    private float currentAcceleration = 0f;
    private float currentBreakForce = 0f;
    private float currentTurnAngle = 0f;
    private Quaternion wheelShift;
    private double speed;

    private void Start()
    {
        brakeLights.SetActive(false);
    }

    private void FixedUpdate()
    {
        currentAcceleration = acceleration * Input.GetAxisRaw("Vertical");
        speed = rb.velocity.magnitude * 3.6;

        frontRight.motorTorque = currentAcceleration * motorForce;
        frontLeft.motorTorque = currentAcceleration * motorForce;

        frontRight.brakeTorque = currentBreakForce;
        frontLeft.brakeTorque = currentBreakForce;
        backRight.brakeTorque = currentBreakForce;
        backLeft.brakeTorque = currentBreakForce;

        currentTurnAngle = maxTurnAngle * Input.GetAxisRaw("Horizontal");

        frontRight.steerAngle = currentTurnAngle;
        frontLeft.steerAngle = currentTurnAngle;

        UpdateWheel(frontRight, frontRightTransform, false);
        UpdateWheel(backRight, backRightTransform, false);

        UpdateWheel(frontLeft, frontLeftTransform, true);
        UpdateWheel(backLeft, backLeftTransform, true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentBreakForce = breakingForce;
            brakeLights.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            currentBreakForce = 0f;
            brakeLights.SetActive(false);
        }
        
        if (Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.Delete))
            Respawn();
        if (transform.position.y < -3f)
            Respawn();
        if (Input.GetKey(KeyCode.E))
            Boost();

        speedText.text = Math.Round(speed).ToString();
    }

    void Respawn()
    {
        transform.position = new Vector3(0, .1f, 0);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        rb.velocity = Vector3.zero;
    }

    void Boost()
    {
        rb.AddForce(transform.forward * boostMultiplier * 3.6f, ForceMode.VelocityChange);
    }

    void UpdateWheel(WheelCollider col, Transform trans, Boolean left)
    {
        col.GetWorldPose(out Vector3 position, out Quaternion rotation);

        wheelShift = Quaternion.Euler(0, left ? 270 : 90, 0);
        Quaternion newRotation = rotation * wheelShift;

        trans.position = position;
        trans.rotation = newRotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("pollo"))
        {
            Vector3 carVel = rb.velocity;
            collision.transform.GetComponent<Rigidbody>().AddForce(carVel * polloMultiplier 
                                                                   + new Vector3(0, polloMultiplier/2f, 0)*carVel.magnitude);

            Vector3 chickenPos = collision.transform.position;
            
            Instantiate(Settings.instance.deathEffect, chickenPos, Quaternion.identity, collision.transform);
            Destroy(Instantiate(Settings.instance.featherEffect, chickenPos, Quaternion.identity, Settings.instance.effectsContainer), 2f);
            Destroy(collision.gameObject, 2f);
        }
    }
}
