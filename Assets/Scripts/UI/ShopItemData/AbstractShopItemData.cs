using UnityEngine;

namespace UI.ShopItemData {
    [CreateAssetMenu(fileName = "ShopItemData", menuName = "Shop/ShopItem")]
    public class AbstractShopItemData : ScriptableObject {
        [Header("Basic Config")]
        [SerializeField] public string _itemName;
        [SerializeField] public Sprite _itemImage;
    }
}