using Unity.VisualScripting;
using UnityEngine;

namespace UI.ShopItemData
{
    public abstract class AbstractShopItemData : ScriptableObject
    {
        [Header("Basic Config")]
        public string _name;
        public Sprite _image;
        public GameObject _uiItemPrefab;
        [TextArea(3, 10)]
        public string _uiDescription;
        public static AbstractShopItemData Instance;

        private void Awake()
        {
            Instance = this;
        }
    }

}