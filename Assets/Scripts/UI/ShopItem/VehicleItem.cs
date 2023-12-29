using GameManagement;
using TMPro;
using UI.ShopItemData;
using UnityEngine;
using Vehicles.Controllers;

namespace UI
{
    /// <summary>
    /// A tile for a specific vehicle on the main page of the shop
    /// </summary>
    public class VehicleItem : AbstractShopItem
    {
        [SerializeField] private TextMeshProUGUI _priceText;

        private GameObject _prefab;
        private int _cost;
        private UpgradeItemData[] _upgrades;

        protected override void OnItemClicked() {
            if (PlayerData.EggCount < _cost)
            {
                Debug.Log("insignificant eggs");
                return;
            }

            VehicleController newVC = _prefab.GetComponent<VehicleController>();
            if (GameRunner.Instance._activeVehicle != null &&
                newVC != null &&
                GameRunner.Instance._activeVehicle.GetType() == newVC.GetType())
            {
                Debug.Log("Can't buy the same vehicle type");
                return;
            }

            PlayerData.EggCount -= _cost;
            GameRunner.Instance.SpawnVehicle(_prefab);
            
            ShopController.Instance.PopulateShop(_upgrades);
        }

        protected override void SetupCustomItemData(AbstractShopItemData abstractItemData) {
            var vehicleItemData = (VehicleItemData)abstractItemData;
            
            _prefab = vehicleItemData._prefab;
            _cost = vehicleItemData._cost;
            _upgrades = vehicleItemData._upgrades;
            _priceText.text = _cost.ToString();
        }
    }
}