using UnityEngine;

public class ChickenController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Range(0f, 50f)] private float moveSpeed;
    [SerializeField, Range(0f, 150f)] private float rotationSpeed;
    [SerializeField] private Vector3 resetPos;
    [SerializeField] private float aimSpeedMultiplier;
    [SerializeField] private GameObject pistol;
    [SerializeField] private GameObject pistolEmmitter;

    private Rigidbody rb;
    private Animator animator;

    private float newAimWeight = 0f;
    private float verticalInput;

    private bool canFire = true;
    private bool isAiming;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        verticalInput = Input.GetAxis("Vertical");
        Inputs();
    }

    void Inputs()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        
        transform.Rotate(Vector3.up * mouseX);

        if(verticalInput == 0f) {
            animator.SetFloat("Speed", 0f);
        }
        else {
            animator.SetFloat("Speed", 1f);
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = resetPos;
        }

        if (Input.GetMouseButton(1))
        {
            isAiming = true;
            newAimWeight = Mathf.Lerp(newAimWeight, 1f, aimSpeedMultiplier * Time.deltaTime);
            animator.SetLayerWeight(1, newAimWeight);
            pistol.SetActive(true);

        }
        else
        {
            isAiming = false;
            newAimWeight = Mathf.Lerp(newAimWeight, 0f, aimSpeedMultiplier * Time.deltaTime);
            animator.SetLayerWeight(1, newAimWeight);
            pistol.SetActive(false);
        }

        if (Input.GetMouseButtonDown(0) && isAiming && canFire)
        {
            animator.SetFloat("fire", 1);           
            GameObject muzzleFlash = Instantiate(Settings.instance.muzzleFlash, pistolEmmitter.transform.position, pistolEmmitter.transform.rotation, Settings.instance.effectsContainer);
            GameObject smoke = Instantiate(Settings.instance.smoke, pistolEmmitter.transform.position, pistolEmmitter.transform.rotation, Settings.instance.effectsContainer);
            Destroy(muzzleFlash, 1f);
            Destroy(smoke, 1f);
        
        }
        else
        {
            animator.SetFloat("fire", 0);
        }
    }

    void FixedUpdate()
    {
        Vector3 moveDirection = transform.forward * verticalInput;
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }
}
