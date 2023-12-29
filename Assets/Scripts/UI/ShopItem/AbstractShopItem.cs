using GameManagement;
using TMPro;
using UI.Event_Listeners;
using UI.ShopItemData;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Vehicles.Controllers;

namespace UI
{
    /// <summary>
    /// An abstract class for any clickable tile in the shop
    /// </summary>
    public abstract class AbstractShopItem : MonoBehaviour
    {
        [Header("Item Config")]
        [SerializeField] public string _itemName;
        [SerializeField] public Sprite _itemImage;


        [Header("Refs")]
        [SerializeField] private Image _image;
        [SerializeField] private HoverEventListener _hoverEventListener;

        private void Start()
        {
            _hoverEventListener.OnClickEvents.AddListener(OnItemClicked);

            CustomStart();
        }

        /// <summary>For any tile type specific start logic</summary>
        protected virtual void CustomStart() { }

        /// <summary>Called when the player click the item tile</summary>
        protected abstract void OnItemClicked();

        /// <summary>Setup shared things like the item image</summary>
        public void SetupShopItemData(AbstractShopItemData abstractItemData)
        {
            _itemName = abstractItemData._itemName;
            _itemImage = abstractItemData._itemImage;

            _image.sprite = _itemImage;
            
            SetupCustomItemData(abstractItemData);
        }

        /// <summary>Setup item type specific data</summary>
        protected abstract void SetupCustomItemData(AbstractShopItemData abstractItemData);
    }
}
