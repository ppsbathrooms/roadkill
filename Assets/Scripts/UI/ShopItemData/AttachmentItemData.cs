using UnityEngine;

namespace UI.ShopItemData {
    [CreateAssetMenu(fileName = "ShopItemData", menuName = "Shop/AttachmentData")]
    public class AttachmentItemData : AbstractShopItemData {
        [Header("Type Specific Config")] 
        public GameObject _prefab;
    }
}