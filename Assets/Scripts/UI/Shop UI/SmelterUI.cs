using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SmelterUI : BaseShopUI
{
    [SerializeField] private TextMeshProUGUI playerSilverCoinText, playerSilverNuggetText, playerGoldNuggetText;

    [SerializeField] private ItemSO silverNuggetItemSO, goldNuggetItemSO, silverIngotItemSO, goldIngotItemSO;

    [SerializeField] private GameObject buySilverIngotButton, buyGoldIngotButton;
    [SerializeField] private GameObject errorMessagePrefab;

    private InventorySO playerInventory;
    private int playerSilverNugget, silverNugget, playerGoldNugget, goldNugget;

    private void Awake() {
        playerInventory = Player.Instance.gameObject.GetComponent<Inventory>().getInventorySO();
        playerSilverNugget = 0;
        playerGoldNugget = 0;
        silverNugget = 0;
        goldNugget = 0;
    }

    private void OnEnable() {
        if(playerInventory == null)
            playerInventory = Player.Instance.gameObject.GetComponent<Inventory>().getInventorySO();

        foreach(var item in playerInventory.GetCurrentInventoryState()) {
            if(!item.Value.IsEmpty) {
                if (item.Value.item.itemName == "Silver Nugget")
                    playerSilverNugget += item.Value.amount;

                if (item.Value.item.itemName == "Gold Nugget")
                    playerGoldNugget += item.Value.amount;
            }
        }
        silverNugget = playerSilverNugget;
        goldNugget = playerGoldNugget;

        UpdateUI();
    }

    private void OnDisable() {
        if(silverNugget != playerSilverNugget) {
            foreach(var item in playerInventory.GetCurrentInventoryState()) {
                if(!item.Value.IsEmpty) {
                    if (item.Value.item.itemName == "Silver Nugget")
                        playerInventory.ClearSlot(item.Key);
                }
            }

            if(silverNugget > 0)
                playerInventory.AddItem(silverNuggetItemSO, silverNugget);
        }

        if (goldNugget != playerGoldNugget) {
            foreach (var item in playerInventory.GetCurrentInventoryState()) {
                if (!item.Value.IsEmpty) {
                    if (item.Value.item.itemName == "Gold Nugget")
                        playerInventory.ClearSlot(item.Key);
                }
            }

            if(goldNugget > 0)
                playerInventory.AddItem(goldNuggetItemSO, goldNugget);
        }

        playerSilverNugget = 0;
        playerGoldNugget = 0;
        silverNugget = 0;
        goldNugget = 0;
    }

    private void UpdateUI() {
        playerSilverCoinText.text = Player.Instance.SilverCoin.ToString();
        playerSilverNuggetText.text = silverNugget.ToString();
        playerGoldNuggetText.text = goldNugget.ToString();
    }

    public void BuySilverIngot() {
        if(Player.Instance.SilverCoin >= 25 && silverNugget >= 3) {
            int result = playerInventory.AddItem(silverIngotItemSO, 1);

            if(result == 0) {
                Player.Instance.SilverCoin -= 25;
                silverNugget -= 3;
                UpdateUI();
            }
            else {
                GameObject errorMessage = Instantiate(errorMessagePrefab, buySilverIngotButton.gameObject.transform);
                errorMessage.GetComponentInChildren<ErrorText>().SetErrorText("Not enough space in inventory!");
            }
        }
        else {
            GameObject errorMessage = Instantiate(errorMessagePrefab, buySilverIngotButton.gameObject.transform);
            errorMessage.GetComponentInChildren<ErrorText>().SetErrorText("Can't afford to buy this!");
        }
    }

    public void BuyGoldIngot() {
        if (Player.Instance.SilverCoin >= 35 && goldNugget >= 3) {
            int result = playerInventory.AddItem(goldIngotItemSO, 1);

            if (result == 0) {
                Player.Instance.SilverCoin -= 35;
                goldNugget -= 3;
                UpdateUI();
            }
            else {
                GameObject errorMessage = Instantiate(errorMessagePrefab, buyGoldIngotButton.gameObject.transform);
                errorMessage.GetComponentInChildren<ErrorText>().SetErrorText("Not enough space in inventory!");
            }
        }
        else {
            GameObject errorMessage = Instantiate(errorMessagePrefab, buyGoldIngotButton.gameObject.transform);
            errorMessage.GetComponentInChildren<ErrorText>().SetErrorText("Can't afford to buy this!");
        }
    }

    public void CloseButton() {
        base.OpenGameUI();
    }
}
