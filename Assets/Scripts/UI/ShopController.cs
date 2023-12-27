using System;
using UnityEngine;

namespace UI {
    public class ShopController : MonoBehaviour {
        public static ShopController Instance;

        [Header("Obj Refs")] [SerializeField] private GameObject shopObj;
        
        public bool ShopEnabled { get; private set; }

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            ShopEnabled = shopObj.activeSelf;
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Tab)) ToggleShop();
            if (Input.GetKeyDown(KeyCode.Escape)) CloseShop();
        }

        public void ToggleShop() {
            if (ShopEnabled)
                CloseShop();
            else OpenShop();
        }

        public void OpenShop() {
            ShopEnabled = true;
            shopObj.SetActive(true);

            Cursor.lockState = CursorLockMode.None;

        }

        public void CloseShop() {
            ShopEnabled = false;
            shopObj.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;

        }
    }
}
