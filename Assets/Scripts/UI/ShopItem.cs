using GameManagement;
using TMPro;
using UI.Event_Listeners;
using UnityEngine;

namespace UI {
    public class ShopItem : MonoBehaviour {
        [Header("Item Config")] 
        [SerializeField] private string _itemName;

        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private int _itemCost;

        [Header("Refs")] 
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private HoverEventListener _hoverEventListener;

        private void Start() {
            _hoverEventListener.OnClickEvents.AddListener(AttemptBuyItem);
            _priceText.text = _itemCost.ToString();
        }

        private void AttemptBuyItem() {
            Debug.Log($"Trying to buy {_itemName}");

            if (PlayerData.EggCount < _itemCost) {
                Debug.Log("Card declined, insignificant funds!");
                return;
            }
            
            PlayerData.EggCount -= _itemCost;
            GameRunner.Instance.SpawnVehicle(_itemPrefab);
            ShopController.Instance.CloseShop();
        }
    }
}
