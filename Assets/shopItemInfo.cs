using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UI.Event_Listeners;
using UI;

public class shopItemInfo : MonoBehaviour
{
    [SerializeField] public TMP_Text _itemName;
    [SerializeField] public TMP_Text _itemDescription;
    [SerializeField] public TMP_Text _itemPrice;
    [SerializeField] private GameObject puchaseButton;
    [SerializeField] private GameObject selector;
    public static shopItemInfo Instance;

    private HoverEventListener _purchaseHoverListner;
    private HoverEventListener _backgroundHoverListner;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _purchaseHoverListner = puchaseButton.GetComponent<HoverEventListener>();
        _backgroundHoverListner = selector.GetComponent<HoverEventListener>();

        _purchaseHoverListner.OnClickEvents.AddListener(buttonClicked);
        _backgroundHoverListner.OnClickEvents.AddListener(ShopController.Instance.HideInfo);
    }

    private void buttonClicked()
    {
        VehicleItem.Instance.purchaseItem();
    }
}
