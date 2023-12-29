using GameManagement;
using TMPro;
using UI.Event_Listeners;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Vehicles.Controllers;

namespace UI
{
    public class ShopItem : MonoBehaviour
    {
        [Header("Item Config")]
        [SerializeField] public string _itemName;

        [SerializeField] public Sprite _itemImage;
        [SerializeField] public GameObject _itemPrefab;
        [SerializeField] public int _itemCost;
        [SerializeField] public bool _combineAttachment;

        [Header("Refs")]
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Image _image;
        [SerializeField] private HoverEventListener _hoverEventListener;

        private void Start()
        {
            _hoverEventListener.OnClickEvents.AddListener(AttemptBuyItem);
            _priceText.text = _itemCost.ToString();
            _image.sprite = _itemImage;
        }

        private void AttemptBuyItem() // TODO: check for already bought items
        {
            // Debug.Log($"Trying to buy {_itemName}");

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
                else
                {
                    Debug.Log("must own combine first");
                    return;
                }

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

        public void SetupShopItemData(NewShopItem newItem)
        {
            _itemName = newItem._itemName;
            _itemCost = newItem._itemCost;
            _itemImage = newItem._itemImage;
            _itemPrefab = newItem._itemPrefab;
            _combineAttachment = newItem._combineAttachment;

            _priceText.text = _itemCost.ToString();
            _image.sprite = _itemImage;
        }

    }
}
