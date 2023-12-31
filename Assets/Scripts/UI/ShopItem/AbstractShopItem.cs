using UI.Event_Listeners;
using UI.ShopItemData;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// An abstract class for any clickable tile in the shop
    /// </summary>
    public abstract class AbstractShopItem : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Image _imageUI;
        [SerializeField] private HoverEventListener _itemHoverListener;

        private string _name;
        private string _uiDescription;

        private void Start()
        {
            _itemHoverListener.OnClickEvents.AddListener(OnItemClicked);
        }

        /// <summary>Called when the player click the item tile</summary>

        void OnItemClicked()
        {
            ShopController.Instance.DisplayInfo();

            shopItemInfo info = shopItemInfo.Instance;

            info._itemDescription.text = _uiDescription;
            info._itemName.text = _name;

            setCustomItemInfo();
        }

        /// <summary>Setup shared things like the item image</summary>
        public void SetupShopItemData(AbstractShopItemData abstractItemData)
        {
            _name = abstractItemData._name;
            _imageUI.sprite = abstractItemData._image;
            _uiDescription = abstractItemData._uiDescription;

            SetupCustomItemData(abstractItemData);
        }

        /// <summary>Setup item type specific data</summary>
        protected abstract void SetupCustomItemData(AbstractShopItemData abstractItemData);
        protected abstract void setCustomItemInfo();
    }
}
