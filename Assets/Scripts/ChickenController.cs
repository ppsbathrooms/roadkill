using Collidable;
using UnityEngine;
using UnityEngine.Events;

public class ChickenController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Range(0f, 50f)] private float moveSpeed;
    [SerializeField, Range(0f, 350f)] private float rotationSpeed;
    [SerializeField] private Vector3 resetPos;
    [SerializeField] private float aimSpeedMultiplier;
    [SerializeField] private float pistolRange;
    
    [Header("Refs")]
    [SerializeField] private GameObject pistol;
    [SerializeField] private GameObject pistolEmitter;

    public static readonly UnityEvent<AbstractCollidableObject> OnHitCollidable = new();

    private Rigidbody rb;
    private Animator animator;

    private float newAimWeight = 0f;
    private float verticalInput;
    private float horizontalInput;
    private float aimFOV = 50f;

    private float currentFOV;
    private bool canFire = true;
    private bool isAiming;

    private float mouseX;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        currentFOV = Camera.main.fieldOfView;
    }

    void Update()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.fixedDeltaTime;

        Inputs();

        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, currentFOV, Time.deltaTime * 5);
    }

    void Inputs()
    {
        
        if(verticalInput == 0f) {
            animator.SetFloat("Speed", 0f);
        }
        else {
            animator.SetFloat("Speed", 1f);
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = resetPos;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.LeftShift))
        {
            isAiming = true;
            newAimWeight = Mathf.Lerp(newAimWeight, 1f, aimSpeedMultiplier * Time.fixedDeltaTime);
            animator.SetLayerWeight(1, newAimWeight);
            pistol.SetActive(true);
            
            currentFOV = aimFOV;
        }
        else
        {
            isAiming = false;
            newAimWeight = Mathf.Lerp(newAimWeight, 0f, aimSpeedMultiplier * Time.fixedDeltaTime);
            animator.SetLayerWeight(1, newAimWeight);
            pistol.SetActive(false);

            currentFOV = 60f;
        }

        if (Input.GetMouseButtonDown(0) && isAiming && canFire)
        {
            animator.SetFloat("fire", 1);           
            GameObject muzzleFlash = Instantiate(Settings.instance.muzzleFlash, pistolEmitter.transform.position, pistolEmitter.transform.rotation, Settings.instance.effectsContainer);
            GameObject smoke = Instantiate(Settings.instance.smoke, pistolEmitter.transform.position, pistolEmitter.transform.rotation, Settings.instance.effectsContainer);
            Destroy(muzzleFlash, 1f);
            Destroy(smoke, 1f);

            Shoot();        
        }
        else
        {
            animator.SetFloat("fire", 0);
        }
    }

    void FixedUpdate()
    {
        Vector3 moveDirection = (transform.forward * verticalInput) + (transform.right * horizontalInput * 0.25f);
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);

        transform.Rotate(Vector3.up * mouseX);
    }

    void Shoot() 
    {
        Vector3 rayOrigin = Camera.main.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit hit;


        if (Physics.Raycast (rayOrigin, Camera.main.transform.forward, out hit, pistolRange))
        {
            if (hit.collider.tag == "Player")
                return;
            else if(hit.collider.tag == "pollo") {
                GameObject bloodHit = Instantiate(Settings.instance.bloodHit, hit.point, Quaternion.Euler(-90, 0, 0), Settings.instance.effectsContainer);
                Destroy(bloodHit, 1f);

                if (hit.collider.transform.TryGetComponent<AbstractCollidableObject>(out AbstractCollidableObject collidableObject))
                {
                    if (!collidableObject.onHitByPlayer())
                        return;
                    
                    OnHitCollidable.Invoke(collidableObject);
                    hit.collider.transform.GetComponent<Rigidbody>().AddForce(transform.forward * 5000 + transform.up * 500);
                    hit.collider.transform.GetComponent<Rigidbody>().AddTorque(new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360)));
                }
            }
            else if (hit.collider.tag == "farmer") 
            {
                GameObject bloodHit = Instantiate(Settings.instance.bloodHit, hit.point, Quaternion.Euler(-90, 0, 0), Settings.instance.effectsContainer);
                Destroy(bloodHit, 1f);
            }
            else {
                GameObject grassHit = Instantiate(Settings.instance.grassHit, hit.point, Quaternion.Euler(-90, 0, 0), Settings.instance.effectsContainer);
                Destroy(grassHit, 1f); 
            }
        }
    }
}
