using System.Collections.Generic;
using UnityEngine;
using Vehicles.Controllers;

namespace GameManagement
{
    public class GameRunner : MonoBehaviour
    {
        public static GameRunner Instance;

        private void Awake()
        {
            Instance = this;
        }
        [SerializeField] private VehicleController normalCar;
        [SerializeField] private VehicleController combine;

        public VehicleController _activeVehicle;

        // private bool combineActive = true;

        void Start()
        {
            _activeVehicle = combine;
            //GameStateMachine.SetState<GameStateMachine.PreGame>(); TODO: fix state machine for new gameplay
        }

        void Update()
        {
            //GameStateMachine.UpdateCurrentState(); TODO: fix state machine for new gameplay
        }

        public void SpawnVehicle(GameObject prefab)
        {
            RemoveCurrentVehicle();
            _activeVehicle = Instantiate(prefab).GetComponent<VehicleController>();
        }

        public void RemoveCurrentVehicle()
        {
            if (_activeVehicle != null)
                Destroy(_activeVehicle.gameObject);
        }

        public void BuyAttachment(GameObject prefab, Sprite image)
        {
            var combineController = _activeVehicle.GetComponent<CombineController>();

            if (combineController != null && combineController.attachmentPoint != null)
            {
                GameObject attachmentPoint = combineController.attachmentPoint;

                if (combineController.attachments == null)
                {
                    combineController.attachments = new Dictionary<GameObject, GameObject>();
                }

                Dictionary<GameObject, GameObject> attachments = combineController.attachments;

                if (attachments.ContainsKey(prefab))
                {
                    Debug.Log("attachment already bought");

                    if (combineController._activeAttachment == attachments[prefab])
                    {
                        return;
                    }
                }
                else
                {
                    HUDController.Instance.UpdateEquipment(image);
                    UI.ShopController.Instance.CloseShop();
                }

                foreach (var attachment in attachments.Values)
                {
                    attachment.SetActive(false);
                }

                GameObject combineAttachment = Instantiate(prefab, attachmentPoint.transform.position, attachmentPoint.transform.rotation);
                combineAttachment.transform.parent = attachmentPoint.transform;
                combineController._activeAttachment = combineAttachment;
                combineAttachment.SetActive(true);

                attachments[prefab] = combineAttachment;
            }
            else
            {
                Debug.LogError("combinecontroller or related components are not properly set up");
            }
        }


    }
}