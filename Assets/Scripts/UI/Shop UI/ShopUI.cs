using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : BaseShopUI
{
    [Serializable]
    private class Price {
        public int goldAmount;
        public int silverAmount;
    }

    [SerializeField] private GameObject shopUIRowPrefab, shopList;

    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName, itemDescription;

    [SerializeField] private TextMeshProUGUI goldCoinText, silverCoinText;

    [SerializeField] private ItemSO[] soldItems;
    [SerializeField] private Price[] prices;

    [SerializeField] private GameObject errorTextPrefab;
    [SerializeField] private Button buyButton;

    private GameObject[] shopUIRow;
    private ShopRow selectedRow;

    private void Start() {
        shopUIRow = new GameObject[soldItems.Length];
        selectedRow = null;

        for(int i = 0; i < soldItems.Length; i++) {
            GameObject row = Instantiate(shopUIRowPrefab, shopList.transform);
            shopUIRow[i] = row;
            row.GetComponent<ShopRow>().InitRow(soldItems[i], prices[i].goldAmount, prices[i].silverAmount);
            row.GetComponent<ShopRow>().OnRowClick += RowClicked;
        }
        ClearItemDescription();
    }

    private void OnEnable() {
        UpdatePlayerCoins();
    }

    private void OnDisable() {
        ClearItemDescription();
        for (int i = 0; i < shopUIRow.Length; i++) {
            shopUIRow[i].GetComponent<ShopRow>().DeselectRow();
        }
        selectedRow = null;
        buyButton.interactable = false;
    }

    private void OnDestroy() {
        for(int i = 0; i < shopUIRow.Length; i++) {
            shopUIRow[i].GetComponent<ShopRow>().OnRowClick -= RowClicked;
        }
    }

    public void CloseButton() {
        base.OpenGameUI();
    }

    public void RowClicked(ShopRow selectedRow) {
        for(int i = 0; i < shopUIRow.Length; i++) {
            shopUIRow[i].GetComponent<ShopRow>().DeselectRow();
        }
        this.selectedRow = selectedRow;
        SetItemDescription(selectedRow.Item.itemImage, selectedRow.Item.itemName, selectedRow.Item.description);
        buyButton.interactable = true;
    }

    private void SetItemDescription(Sprite itemImage, string itemName, string itemDescription) {
        this.itemImage.sprite = itemImage;
        this.itemImage.gameObject.SetActive(true);
        this.itemName.text = itemName;
        this.itemDescription.text = itemDescription;
    }

    private void ClearItemDescription() {
        itemImage.gameObject.SetActive(false);
        itemName.text = "";
        itemDescription.text = "";
    }

    private void UpdatePlayerCoins() {
        goldCoinText.text = Player.Instance.GoldCoin.ToString();
        silverCoinText.text = Player.Instance.SilverCoin.ToString();
    }

    public void BuyButton() {
        if (selectedRow == null)
            return;

        if(Player.Instance.GoldCoin >= selectedRow.GoldAmount && Player.Instance.SilverCoin >= selectedRow.SilverAmount) {
            int result = Player.Instance.gameObject.GetComponent<Inventory>().AddItem(selectedRow.Item, 1);
            if(result == 1) {
                GameObject errorText = Instantiate(errorTextPrefab, buyButton.gameObject.transform);
                errorText.GetComponentInChildren<ErrorText>().SetErrorText("Not enough space in inventory");
            }
            else {
                Player.Instance.GoldCoin -= selectedRow.GoldAmount;
                Player.Instance.SilverCoin -= selectedRow.SilverAmount;
                UpdatePlayerCoins();
            }
        }
        else {
            GameObject errorText = Instantiate(errorTextPrefab, buyButton.gameObject.transform);
            errorText.GetComponentInChildren<ErrorText>().SetErrorText("Can't afford to buy this");
        }
    }
}
