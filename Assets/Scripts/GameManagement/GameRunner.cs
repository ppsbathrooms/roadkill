using UnityEngine;
using Vehicles.Controllers;

namespace GameManagement
{
    public class GameRunner : MonoBehaviour
    {
        [SerializeField] private VehicleController normalCar;
        [SerializeField] private VehicleController combine;

        private VehicleController _activeVehicle;

        private bool combineActive = true;

        void Start()
        {
            _activeVehicle = combine;
            //GameStateMachine.SetState<GameStateMachine.PreGame>(); TODO: fix for new gameplay
        }

        void Update()
        {
            //GameStateMachine.UpdateCurrentState(); TODO: fix for new gameplay

            if (Input.GetKeyDown(KeyCode.Tab)) ToggleVehicle();
        }

        private void ToggleVehicle()
        {
            combineActive = !combineActive;
            _activeVehicle.Disable();

            if (combineActive)
            {
                _activeVehicle = combine;
            }
            else
            {
                _activeVehicle = normalCar;
            }

            _activeVehicle.Enable();
        }
    }
}
