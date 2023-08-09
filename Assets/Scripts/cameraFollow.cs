using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform target;
    [SerializeField] private float translateSpeed;
    [SerializeField] private float rotationSpeedX;
    [SerializeField] private float rotationSpeedY;
    
    private float yMin = -20f;
    private float yMax = 60f;
    private float yRotation;



    void Update()
    {
        yRotation += -Input.GetAxis("Mouse Y") * rotationSpeedY;
        yRotation = Mathf.Clamp(yRotation, yMin, yMax);

    }
    private void FixedUpdate()
    {
        var targetPosition = target.TransformPoint(offset);
        transform.position = Vector3.Slerp(transform.position, targetPosition, translateSpeed * Time.fixedDeltaTime);

        var direction = target.position - transform.position;
        var rotation = Quaternion.LookRotation(direction,Vector3.up);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeedX * Time.fixedDeltaTime);
        target.transform.localRotation = Quaternion.Euler(0, 90, yRotation);
    }
}