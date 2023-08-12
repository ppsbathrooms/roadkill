using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector3 offset;
    [SerializeField] private float translateSpeed;
    [SerializeField] private float rotationSpeedX;
    [SerializeField] private float rotationSpeedY;

    [Space][Header("Refs")]
    [SerializeField] private Transform target;

    
    private float yMin = -25f;
    private float yMax = 90f;
    private float yRotation;

    private float backDistance;
    private float cameraOffset = 0f;

    void Update()
    {
        yRotation += -Input.GetAxis("Mouse Y") * rotationSpeedY;
        yRotation = Mathf.Clamp(yRotation, yMin, yMax);
        RayGroundCollision();
    }

    private void FixedUpdate()
    {
        var targetPosition = target.TransformPoint(offset.x  - cameraOffset, offset.y, offset.z);
        transform.position = Vector3.Slerp(transform.position, targetPosition, translateSpeed * Time.fixedDeltaTime);

        var direction = target.position - transform.position;
        var rotation = Quaternion.LookRotation(direction,Vector3.up);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeedX * Time.fixedDeltaTime);
        target.transform.localRotation = Quaternion.Euler(0, 90, yRotation);
    }

    private void RayGroundCollision()
    {
        int groundMask = 1 << 3;
        float distance = Vector3.Distance(transform.position, target.position);
        Vector3 rayEnding = Camera.main.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit hit;
        RaycastHit backHit;
        
        Vector3 dir = rayEnding - target.position;
        Debug.DrawRay(target.position, dir, Color.red);

        //going back raycast
        if (Physics.Raycast (target.position, dir, out backHit, 10f, groundMask))
        {
            backDistance = backHit.distance;
        }

        if (Physics.Raycast (target.position, dir, out hit, distance, groundMask))
        {
            if(cameraOffset < 1.15)
            {                
                cameraOffset = Mathf.Lerp(cameraOffset, cameraOffset + (distance - hit.distance), Time.fixedDeltaTime * 10f);
            }
        }
        else
        {
            if(cameraOffset > 0) {               
                cameraOffset = Mathf.Lerp(cameraOffset, cameraOffset - (backDistance - distance), Time.fixedDeltaTime * 3f);
            }
        }
    }
}