using UnityEngine;

namespace Vehicles.Controllers {
    public class BasicCarController : VehicleController
    {
        [SerializeField] private GameObject brakeLights;

        protected new void Start() {
            brakeLights.SetActive(false);
        }
    
        protected new void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) {
                brakeLights.SetActive(true);
            }
            else if (Input.GetKeyUp(KeyCode.Space)) {
                brakeLights.SetActive(false);
            }
        }
    }
}
