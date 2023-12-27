using System;
using UnityEngine;
using Vehicles.Controllers;

namespace GameManagement
{
    public class GameRunner : MonoBehaviour {
        public static GameRunner Instance;

        private void Awake() {
            Instance = this;
        }

        [SerializeField] private VehicleController normalCar;
        [SerializeField] private VehicleController combine;

        private VehicleController _activeVehicle;

        private bool combineActive = true;

        void Start()
        {
            _activeVehicle = combine;
            //GameStateMachine.SetState<GameStateMachine.PreGame>(); TODO: fix state machine for new gameplay
        }

        void Update()
        {
            //GameStateMachine.UpdateCurrentState(); TODO: fix state machine for new gameplay
        }

        public void SpawnVehicle(GameObject prefab) {
            RemoveCurrentVehicle();
            _activeVehicle = Instantiate(prefab).GetComponent<VehicleController>();
        }

        public void RemoveCurrentVehicle() {
            if (_activeVehicle != null) 
                Destroy(_activeVehicle.gameObject);
        }

        /*private void ToggleVehicle()
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
        }*/
    }
}
