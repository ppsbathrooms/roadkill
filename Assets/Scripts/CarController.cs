using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    [SerializeField] private WheelCollider backLeft;
    [SerializeField] private WheelCollider backRight;
    [SerializeField] private WheelCollider frontLeft;
    [SerializeField] private WheelCollider frontRight;

    [SerializeField] private Transform backLeftTransform;
    [SerializeField] private Transform backRightTransform;
    [SerializeField] private Transform frontLeftTransform;
    [SerializeField] private Transform frontRightTransform;

    [SerializeField] private GameObject brakeLights;

    [SerializeField] private float polloMultiplier;

    public float acceleration = 500f;
    public float breakingForce = 300f;
    public float maxTurnAngle = 15f;

    [SerializeField] private float motorForce = 1f;
    private float currentAcceleration = 0f;
    private float currentBreakForce = 0f;
    private float currentTurnAngle = 0f;
    private Quaternion wheelShift;

    private void FixedUpdate()
    {
        currentAcceleration = acceleration * Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.Space))
        {
            currentBreakForce = breakingForce;
            brakeLights.SetActive(true);
        }
        else
        {
            currentBreakForce = 0f;
            brakeLights.SetActive(false);
        }

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
        if (Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.Delete))
            Respawn();
        if (GetComponent<Rigidbody>().position.y < -3f)
            Respawn();
    }

    void Respawn()
    {
        transform.position = new Vector3(0, .1f, 0);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    void UpdateWheel(WheelCollider col, Transform trans, Boolean left)
    {
        Vector3 position;
        Quaternion rotation;
        col.GetWorldPose(out position, out rotation);

        if (left)
            wheelShift = Quaternion.Euler(0, 270, 0);
        else
            wheelShift = Quaternion.Euler(0, 90, 0);
        Quaternion newRotation = rotation * wheelShift;

        trans.position = position;
        trans.rotation = newRotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("pollo"))
        {
            collision.transform.GetComponent<Rigidbody>().AddForce(GetComponent<Rigidbody>().velocity * polloMultiplier 
                                                                   + new Vector3(0, polloMultiplier/2f, 0)*GetComponent<Rigidbody>().velocity.magnitude);
            
            Destroy(Instantiate(Settings.instance.deathEffect, collision.transform.position, Quaternion.identity, Settings.instance.effectsContainer), 2f);
            Destroy(collision.gameObject, 0.5f);
        }
    }
}
