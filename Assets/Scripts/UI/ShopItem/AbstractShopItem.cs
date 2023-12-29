using GameManagement;
using TMPro;
using UI.Event_Listeners;
using UI.ShopItemData;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Vehicles.Controllers;

namespace UI
{
    /// <summary>
    /// An abstract class for any clickable tile in the shop
    /// </summary>
    public abstract class AbstractShopItem : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Image _imageUI;
        [SerializeField] private HoverEventListener _hoverEventListener;

        private string _name;

        private void Start()
        {
            _hoverEventListener.OnClickEvents.AddListener(OnItemClicked);
        }

        /// <summary>Called when the player click the item tile</summary>
        protected abstract void OnItemClicked();

        /// <summary>Setup shared things like the item image</summary>
        public void SetupShopItemData(AbstractShopItemData abstractItemData)
        {
            _name = abstractItemData._name;
            _imageUI.sprite = abstractItemData._image;

            SetupCustomItemData(abstractItemData);
        }

        /// <summary>Setup item type specific data</summary>
        protected abstract void SetupCustomItemData(AbstractShopItemData abstractItemData);
    }
}
