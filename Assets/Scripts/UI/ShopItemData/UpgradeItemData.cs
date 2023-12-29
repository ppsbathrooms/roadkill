using System;
using UnityEngine;

namespace UI.ShopItemData {
    [CreateAssetMenu(fileName = "ShopItemData", menuName = "Shop/StatUpgradeData")]
    public class UpgradeItemData : AbstractShopItemData {
        [Header("Type Specific Config")] 
        public float _defaultValue;
        public UpgradeLayerInfo[] _upgradeLayers;
    }

    [Serializable]
    public struct UpgradeLayerInfo {
        public float _statValue;
        public int _cost;
    }
}