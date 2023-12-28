using Collidable;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vehicles.Controllers;

[System.Serializable]
public class HotbarData
{
    public string itemName;
    public Sprite sprite;
}

public class HUDController : MonoBehaviour
{
    public static HUDController Instance;

    private void Awake() { Instance = this; }

    [Header("Refs")]
    [SerializeField] private TMP_Text speedText;
    [SerializeField] private TMP_Text eggText;

    [SerializeField] private GameObject _startGameButton;
    [SerializeField] private GameObject _endGameButton;
    [SerializeField] private GameObject _gameOverScreen;

    public GameObject StartGameButton => _startGameButton;
    public GameObject EndGameButton => _endGameButton;
    public GameObject GameOverScreen => _gameOverScreen;

    [SerializeField] private GameObject hotbarItem;
    [SerializeField] private GameObject shopItem;

    [SerializeField] private Transform equipmentContainer;
    [SerializeField] public Transform shopItemsContainer;
    [SerializeField] private NewShopItem[] shopItems;
    [SerializeField] private HotbarData[] hotbarItems;

    void Start()
    {
        instantiateShop();
    }
    public void UpdateEggText()
    {
        eggText.text = PlayerData.EggCount.ToString();
    }

    public void UpdateSpeedText(float speed)
    {
        speedText.text = Mathf.RoundToInt(speed).ToString();
    }

    public void UpdateEquipment(Sprite equipmentImage)
    {
        if (hotbarItem == null || equipmentContainer == null)
        {
            Debug.LogError("prefab or equipment container not defined");
            return;
        }

        GameObject equipmentUI = Instantiate(hotbarItem, equipmentContainer);

        Image[] images = equipmentUI.GetComponentsInChildren<Image>(true);

        foreach (Image imageComponent in images)
        {
            if (imageComponent.gameObject.CompareTag("hotbarImage"))
            {
                imageComponent.sprite = equipmentImage;
                return;
            }
        }

        Debug.LogWarning("image component not found in the prefab");
    }

    private void instantiateShop()
    {
        foreach (NewShopItem item in shopItems)
        {
            GameObject newItem = Instantiate(shopItem, shopItemsContainer);
            UI.ShopItem itemScript = newItem.GetComponent<UI.ShopItem>();

            if (itemScript != null)
            {
                itemScript.SetupShopItemData(item);
            }
            else
            {
                Debug.LogError("shopitem script not found on the instantiated object");
            }
        }
    }

}
