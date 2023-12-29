using Collidable;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Vehicles.Controllers
{
    public abstract class VehicleController : MonoBehaviour
    {
        public static VehicleController Instance;

        public static readonly UnityEvent<AbstractCollidableObject> OnHitCollidable = new();

        [Header("Refs")]
        [SerializeField] private WheelCollider backLeft;
        [SerializeField] private WheelCollider backRight;
        [SerializeField] private WheelCollider frontLeft;
        [SerializeField] private WheelCollider frontRight;

        [SerializeField] private Transform backLeftTransform;
        [SerializeField] private Transform backRightTransform;
        [SerializeField] private Transform frontLeftTransform;
        [SerializeField] private Transform frontRightTransform;

        [SerializeField] private Transform camTarget;

        [SerializeField] private Rigidbody rb;

        [Space]
        [Header("Settings")]
        [SerializeField, Range(0f, 400f)] private float polloMultiplier = 200f;
        [SerializeField, Range(0f, 1f)] private float boostMultiplier = .25f;
        [SerializeField, Range(0f, 1500f)] private float acceleration = 750f;
        [SerializeField, Range(0f, 500f)] private float breakingForce = 300f;
        [SerializeField, Range(0f, 180f)] private float maxTurnAngle = 15f;
        [SerializeField] private Vector3 wheelOffset;
        [SerializeField] private bool invertLeftWheels;

        [SerializeField] private Vector3 carSpawn = new(0, 2, 0);

        private float currentAcceleration;
        private float currentBreakForce;
        private float currentTurnAngle;
        private Quaternion wheelShift;
        private float speed;

        private void Awake()
        {
            Instance = this;
            OnHitCollidable.AddListener(collidable => { PlayerData.EggCount += collidable.eggsWhenHit; });

            if (SceneManager.GetActiveScene().name == "TestingMap")
                gameObject.SetActive(false);
        }

        private void Start()
        {
            if (Camera.main!.TryGetComponent(out CameraFollow camFollow))
            {
                camFollow.target = camTarget;
            }
            else Debug.LogError("Could Not Find CameraFollow Instance");
            
            Respawn();
            
            CustomVehicleStart();
        }

        protected virtual void CustomVehicleStart() { }

        private void FixedUpdate()
        {
            UpdateCarForces();
        }

        private void UpdateCarForces()
        {
            currentAcceleration = acceleration * Input.GetAxis("Vertical");
            speed = rb.velocity.magnitude * 3.6f;

            backRight.motorTorque = currentAcceleration;
            backLeft.motorTorque = currentAcceleration;

            frontRight.brakeTorque = currentBreakForce;
            frontLeft.brakeTorque = currentBreakForce;
            backRight.brakeTorque = currentBreakForce;
            backLeft.brakeTorque = currentBreakForce;
            currentTurnAngle = maxTurnAngle * Input.GetAxis("Horizontal");
            frontRight.steerAngle = currentTurnAngle;
            frontLeft.steerAngle = currentTurnAngle;

            UpdateWheel(frontRight, frontRightTransform, invertLeftWheels);
            UpdateWheel(backRight, backRightTransform, invertLeftWheels);

            UpdateWheel(frontLeft, frontLeftTransform, false);
            UpdateWheel(backLeft, backLeftTransform, false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentBreakForce = breakingForce;
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                currentBreakForce = 0f;
            }

            if (Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.Delete))
                Respawn();

            HUDController.Instance.UpdateSpeedText(speed);

            CustomVehicleUpdate();
        }
        
        protected virtual void CustomVehicleUpdate() { }


        void Respawn()
        {
            transform.position = carSpawn;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            rb.velocity = Vector3.zero;
        }

        void Boost()
        {
            rb.AddForce(transform.forward * boostMultiplier * 3.6f, ForceMode.VelocityChange);
        }

        void UpdateWheel(WheelCollider col, Transform trans, bool invertLeft)
        {
            col.GetWorldPose(out Vector3 position, out Quaternion rotation);
            wheelShift = Quaternion.Euler(wheelOffset.x, invertLeft ? wheelOffset.y : -wheelOffset.y, wheelOffset.z);
            Quaternion newRotation = rotation * wheelShift;

            trans.position = position;
            trans.rotation = newRotation;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.TryGetComponent(out AbstractCollidableObject collidableObject))
            {
                if (!collidableObject.onHitByPlayer())
                    return;

                OnHitCollidable.Invoke(collidableObject);

                if (collidableObject.boostWhenHit)
                    Boost();

                Vector3 carVel = rb.velocity;
                collision.transform.GetComponent<Rigidbody>().AddForce(carVel * polloMultiplier
                                                                       + transform.up * (polloMultiplier / 2 * carVel.magnitude));
            }

            if (collision.transform.TryGetComponent(out BreakableObject breakableObject))
            {
                breakableObject.TriggerDestroy();
            }
        }
    }
}
