using UI;
using UnityEngine;

namespace Vehicles.Controllers
{
    public class BasicCarController : VehicleController
    {
        [Header("Vehicle Specific Settings")]

        [SerializeField] private GameObject brakeLights;

        protected override void CustomVehicleStart()
        {
            brakeLights.SetActive(false);
        }

        protected override void CustomVehicleUpdate()
        {
            if (ShopController.Instance.ShopEnabled)
            {
                brakeLights.SetActive(false);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                brakeLights.SetActive(true);
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                brakeLights.SetActive(false);
            }
        }
    }
}
