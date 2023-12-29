using System.Linq;
using GameManagement;
using TMPro;
using UI.ShopItemData;
using UnityEngine;
using UnityEngine.UI;
using Util;
using Vehicles.Controllers;

namespace UI
{
    /// <summary>
    /// A tile for a specific vehicle on the main page of the shop
    /// </summary>
    public class UpgradeItem : AbstractShopItem
    {
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Transform _upgradeDotsParent;
        [SerializeField] private GameObject _upgradeDotPrefab;
        
        private UpgradeLayerInfo[] _upgradeLayers;

        private GameObject[] _dotFillImages;
        private int _currentUpgradeLevel; //TODO: load from player data when save system is added

        protected override void OnItemClicked() {
            if (_currentUpgradeLevel >= _upgradeLayers.Length)
                return;
            
            if (PlayerData.EggCount < _upgradeLayers[_currentUpgradeLevel]._cost)
            {
                Debug.Log("insignificant eggs");
                return;
            }
            
            PlayerData.EggCount -= _upgradeLayers[_currentUpgradeLevel]._cost;

            _dotFillImages[_currentUpgradeLevel].SetActive(true);
            _currentUpgradeLevel++;
            
            if (_currentUpgradeLevel >= _upgradeLayers.Length - 1)
                return;

            _priceText.text = _upgradeLayers[_currentUpgradeLevel]._cost.ToString();
        }

        protected override void SetupCustomItemData(AbstractShopItemData abstractItemData) {
            var statUpgradeData = (UpgradeItemData)abstractItemData;
            _upgradeLayers = statUpgradeData._upgradeLayers;
            _priceText.text = _upgradeLayers[0]._cost.ToString();
            
            // Spawn upgrade dots
            _dotFillImages = new GameObject[_upgradeLayers.Length];

            _dotFillImages = EnumeratorUtil.Select(_upgradeLayers, _ => {
                var obj = Instantiate(_upgradeDotPrefab, _upgradeDotsParent);
                return obj.transform.Find("fill").gameObject;
            }).ToArray();
        }
    }
}
