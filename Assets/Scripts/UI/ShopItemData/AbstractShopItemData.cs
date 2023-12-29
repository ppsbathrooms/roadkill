using UnityEngine;

namespace UI.ShopItemData {
    public abstract class AbstractShopItemData : ScriptableObject {
        [Header("Basic Config")]
        public string _name;
        public Sprite _image;
        public GameObject _uiItemPrefab;
    }
}