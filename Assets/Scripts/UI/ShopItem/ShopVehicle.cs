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
    public class ShopVehicle : AbstractShopItem
    {
        [SerializeField] private TextMeshProUGUI _priceText;

        public GameObject _itemPrefab;
        public bool _combineAttachment;
        public int _itemCost;

        protected override void OnItemClicked() {
            if (PlayerData.EggCount < _itemCost)
            {
                Debug.Log("insignificant eggs");
                return;
            }

            if (_combineAttachment)
            {
                if (GameRunner.Instance._activeVehicle == null)
                {
                    return;
                }
                if (GameRunner.Instance._activeVehicle.GetComponent<CombineController>())
                {
                    GameRunner.Instance.BuyAttachment(_itemPrefab, _itemImage);
                    return;
                }

                Debug.Log("must own combine first");
                return;
            }

            VehicleController newVC = _itemPrefab.GetComponent<VehicleController>();
            if (GameRunner.Instance._activeVehicle != null &&
                newVC != null &&
                GameRunner.Instance._activeVehicle.GetType() == newVC.GetType())
            {
                Debug.Log("Can't buy the same vehicle type");
                return;
            }

            PlayerData.EggCount -= _itemCost;
            GameRunner.Instance.SpawnVehicle(_itemPrefab);
            ShopController.Instance.CloseShop();
        }

        protected override void SetupCustomItemData(AbstractShopItemData abstractItemData) {
            var vehicleItemData = (VehicleItemData)abstractItemData;
            
            _itemPrefab = vehicleItemData._itemPrefab;
            _combineAttachment = vehicleItemData._combineAttachment;
            _itemCost = vehicleItemData._itemCost;
            _priceText.text = _itemCost.ToString();
        }
    }
}
