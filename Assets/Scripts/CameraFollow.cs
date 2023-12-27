using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector3 offset;
    [SerializeField] private float translateSpeed;
    [SerializeField] private float rotationSpeedX;
    [SerializeField] private float rotationSpeedY;
    [SerializeField] private float yMin = -25f;
    [SerializeField] private float yMax = 90f;
    [SerializeField] private float xMin = -25f;
    [SerializeField] private float xMax = 90f;

    [Space]
    [Header("Refs")]
    public Transform target;


    private float yRotation;
    private float xRotation;

    private float backDistance;
    private float cameraOffset = 0f;

    void Update()
    {
        if (target == null)
            return;
        
        yRotation += -Input.GetAxis("Mouse Y") * rotationSpeedY;
        xRotation += Input.GetAxis("Mouse X") * rotationSpeedX;
        yRotation = Mathf.Clamp(yRotation, yMin, yMax);
        xRotation = Mathf.Clamp(xRotation, xMin, xMax);
        RayGroundCollision();
    }

    private void FixedUpdate() {
        if (target == null)
            return;
        
        var targetPosition = target.TransformPoint(offset.x - cameraOffset, offset.y, offset.z);
        transform.position = Vector3.Slerp(transform.position, targetPosition, translateSpeed * Time.fixedDeltaTime);

        var direction = target.position - transform.position;
        var rotation = Quaternion.LookRotation(direction, Vector3.up);

        // transform.rotation =  Quaternion.Slerp(transform.rotation, rotation, translateSpeed * Time.fixedDeltaTime);
        transform.rotation = rotation;
        target.transform.localRotation = Quaternion.Euler(0, xRotation, yRotation);
    }

    private void RayGroundCollision()
    {
        int groundMask = 1 << 3;
        float distance = Vector3.Distance(transform.position, target.position);
        Vector3 rayEnding = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit hit;
        RaycastHit backHit;

        Vector3 dir = rayEnding - target.position;
        Debug.DrawRay(target.position, dir, Color.red);

        //going back raycast
        if (Physics.Raycast(target.position, dir, out backHit, 10f, groundMask))
        {
            backDistance = backHit.distance;
        }

        if (Physics.Raycast(target.position, dir, out hit, distance, groundMask))
        {
            if (cameraOffset < 1.15)
            {
                cameraOffset = Mathf.Lerp(cameraOffset, cameraOffset + (distance - hit.distance), Time.fixedDeltaTime * 10f);
            }
        }
        else
        {
            if (cameraOffset > 0)
            {
                cameraOffset = Mathf.Lerp(cameraOffset, cameraOffset - (backDistance - distance), Time.fixedDeltaTime * 3f);
            }
        }
    }
}