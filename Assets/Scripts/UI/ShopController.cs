using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ShopController : MonoBehaviour
    {
        public static ShopController Instance;

        [Header("Obj Refs")][SerializeField] private GameObject shopObj;
        [SerializeField] private GameObject hotbarItemPrefab;
        [SerializeField] private GameObject shopItemPrefab;

        [SerializeField] private Transform equipmentContainer;
        [SerializeField] public Transform shopItemsContainer;
        [SerializeField] private VehicleItemData[] shopItems;

        public bool ShopEnabled { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            ShopEnabled = shopObj.activeSelf;
            OpenShop(); // TODO: start with a basic vehicle and closed shop
            PopulateShop();
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
            ShopEnabled = true;
            shopObj.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            PopulateShop();
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

        private void PopulateShop()
        {
            ClearShopContents();

            foreach (VehicleItemData item in shopItems)
            {
                GameObject newItem = Instantiate(shopItemPrefab, shopItemsContainer);
                ShopVehicle vehicleScript = newItem.GetComponent<ShopVehicle>();

                if (vehicleScript != null)
                {
                    vehicleScript.SetupShopItemData(item);
                }
                else
                {
                    Debug.LogError("shopitem script not found on the instantiated object");
                }
            }
        }

        private void ClearShopContents() {
            foreach (Transform child in shopItemsContainer) {
                Destroy(child.gameObject);
            }
        }
    }
}
