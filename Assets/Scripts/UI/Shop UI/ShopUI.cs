using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [Serializable]
    private class Price {
        public int goldAmount;
        public int silverAmount;
    }

    [SerializeField] GameObject shopUIRowPrefab, shopList;

    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemName, itemDescription;

    [SerializeField] private ItemSO[] soldItems;
    [SerializeField] private Price[] prices;
    
    private ToggleShopUI toggleShopUI;
    private GameObject[] shopUIRow;
    private ShopRow selectedRow;


    private void Start() {
        shopUIRow = new GameObject[soldItems.Length];
        selectedRow = null;

        for(int i = 0; i < soldItems.Length; i++) {
            GameObject row = Instantiate(shopUIRowPrefab, shopList.transform);
            shopUIRow[i] = row;
            row.GetComponent<ShopRow>().InitRow(soldItems[i], prices[i].goldAmount, prices[i].silverAmount);
            row.GetComponent<ShopRow>().OnRowClick += RowClick;
        }
    }

    private void OnDestroy() {
        for(int i = 0; i < shopUIRow.Length; i++) {
            shopUIRow[i].GetComponent<ShopRow>().OnRowClick -= RowClick;
        }
    }

    public void setToggleShopUI(ToggleShopUI toggleShopUI) {
        this.toggleShopUI = toggleShopUI;
    }

    public void CloseButton() {
        if(toggleShopUI != null) {
            toggleShopUI.OpenGameUI();
        }
    }

    public void RowClick(ShopRow selectedRow) {
        for(int i = 0; i < shopUIRow.Length; i++) {
            shopUIRow[i].GetComponent<ShopRow>().DeselectRow();
        }
        this.selectedRow = selectedRow;
    }

    public void SetItemDescription(Sprite itemImage, string itemName, string itemDescription) {

    }
}
