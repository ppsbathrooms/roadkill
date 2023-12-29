using UnityEngine;

namespace UI.ShopItemData
{
    [CreateAssetMenu(fileName = "ShopItemData", menuName = "Shop/VehicleData")]
    public class VehicleItemData : AbstractShopItemData
    {
        [Header("Type Specific Config")]
        public GameObject _prefab;
        public int _cost;
        public UpgradeItemData[] _upgrades;
    }
}
