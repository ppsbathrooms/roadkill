using UI.ShopItemData;
using UnityEngine;

public class VehicleItemData : AbstractShopItemData
{
    [Header("Type Specific Config")]
    [SerializeField] public GameObject _itemPrefab;
    [SerializeField] public int _itemCost;
    [SerializeField] public bool _combineAttachment;
}
