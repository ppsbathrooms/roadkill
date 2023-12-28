using UnityEngine;

[CreateAssetMenu(fileName = "NewShopItem", menuName = "Shop/ShopItem")]
public class NewShopItem : ScriptableObject
{
    [Header("Item Config")]
    [SerializeField] public string _itemName;

    [SerializeField] public Sprite _itemImage;
    [SerializeField] public GameObject _itemPrefab;
    [SerializeField] public int _itemCost;
    [SerializeField] public bool _combineAttachment;

}
