using UnityEngine;

namespace Vehicles.Controllers {
    public class CombineController : VehicleController {
        [Header("Vehicle Specific Settings")] [SerializeField, Range(0f, 360f)]
        private float rollerRotationSpeed = 180f;

        [Space] [SerializeField] private GameObject rollerFront;
        [SerializeField] private GameObject rollerBack;

        protected override void CustomVehicleUpdate() {
            if (rollerFront == null)
                return;

            rollerFront.transform.Rotate(Vector3.right, rollerRotationSpeed * Time.deltaTime);
            rollerBack.transform.Rotate(Vector3.right, -rollerRotationSpeed * Time.deltaTime);
        }
    }
}
