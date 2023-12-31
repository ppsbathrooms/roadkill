using UI.ShopItemData;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{

    public class currentItemData
    {
        public GameObject prefab;
        public int price;
    }

    public class ShopController : MonoBehaviour
    {
        public static ShopController Instance;

        [Header("Obj Refs")]
        [SerializeField] private GameObject shopObj;
        [SerializeField] private GameObject shopInfoObj;
        [SerializeField] private GameObject hotbarItemPrefab;

        [SerializeField] private Transform equipmentContainer;
        [SerializeField] public Transform shopItemsContainer;
        [SerializeField] private AbstractShopItemData[] shopVehicles;

        public currentItemData _currentItemData = new currentItemData();

        public bool ShopEnabled { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            ShopEnabled = shopObj.activeSelf;
            CloseShop();
            HideInfo();
            PopulateShopWithVehicles();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab)) ToggleShop();
            if (Input.GetKeyDown(KeyCode.Escape)) CloseShop();
        }

        public void ToggleShop()
        {
            if (ShopEnabled)
                CloseShop();
            else OpenShop();
        }

        public void OpenShop()
        {
            HideInfo();
            ShopEnabled = true;
            shopObj.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            PopulateShopWithVehicles();
        }

        public void DisplayInfo()
        {
            shopInfoObj.SetActive(true);
        }

        public void HideInfo()
        {
            shopInfoObj.SetActive(false);
        }

        public void CloseShop()
        {
            ShopEnabled = false;
            shopObj.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            ClearShopContents();
        }

        public void UpdateEquipment(Sprite equipmentImage)
        {
            if (hotbarItemPrefab == null || equipmentContainer == null)
            {
                Debug.LogError("prefab or equipment container not defined");
                return;
            }

            GameObject equipmentUI = Instantiate(hotbarItemPrefab, equipmentContainer);

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


        private void PopulateShopWithVehicles()
        {
            PopulateShop(shopVehicles);
        }

        public void PopulateShop(AbstractShopItemData[] items)
        {
            ClearShopContents();

            foreach (var item in items)
            {
                var newItem = Instantiate(item._uiItemPrefab, shopItemsContainer);
                var vehicleItemScript = newItem.GetComponent<AbstractShopItem>();

                if (vehicleItemScript != null)
                {
                    vehicleItemScript.SetupShopItemData(item);
                }
                else
                {
                    Debug.LogError("shop item script not found on the instantiated object");
                }
            }
        }

        private void ClearShopContents()
        {
            foreach (Transform child in shopItemsContainer)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
