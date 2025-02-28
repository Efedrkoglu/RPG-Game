using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JewelleryShopUI : BaseShopUI
{
    [SerializeField] private TextMeshProUGUI goldCoinText, silverCoinText, rubyPartsText, sapphirePartsText, emeraldPartsText;
    [SerializeField] private GameObject errorMessagePrefab;

    [SerializeField] private ItemSO rubyPartsItemSO, sapphirePartsItemSO, emeraldPartsItemSO, rubyItemSO, sapphireItemSO, emeraldItemSO;

    [SerializeField] private GameObject buyRubyButton, buySapphireButton, buyEmeraldButton;

    private InventorySO playerInventory;
    private int playerRubyParts, playerSapphireParts, playerEmeraldParts, rubyParts, sapphireParts, emeraldParts;

    private void Awake() {
        playerInventory = Player.Instance.gameObject.GetComponent<Inventory>().getInventorySO();
        playerRubyParts = 0;
        playerSapphireParts = 0;
        playerEmeraldParts = 0;
        rubyParts = 0;
        sapphireParts = 0;
        emeraldParts = 0;
    }

    private void OnEnable() {
        if(playerInventory == null)
            playerInventory = Player.Instance.gameObject.GetComponent<Inventory>().getInventorySO();

        foreach(var item in playerInventory.GetCurrentInventoryState()) {
            if(!item.Value.IsEmpty) {
                if (item.Value.item.itemName == "Ruby Parts")
                    playerRubyParts += item.Value.amount;

                if (item.Value.item.itemName == "Sapphire Parts")
                    playerSapphireParts += item.Value.amount;

                if (item.Value.item.itemName == "Emerald Parts")
                    playerEmeraldParts += item.Value.amount;
            }
        }

        rubyParts = playerRubyParts;
        sapphireParts = playerSapphireParts;
        emeraldParts = playerEmeraldParts;

        UpdateUI();
    }

    private void OnDisable() {
        if(rubyParts != playerRubyParts) {
            foreach(var item in playerInventory.GetCurrentInventoryState()) {
                if(!item.Value.IsEmpty) {
                    if (item.Value.item.itemName == "Ruby Parts")
                        playerInventory.ClearSlot(item.Key);
                }
            }

            if (rubyParts > 0)
                playerInventory.AddItem(rubyPartsItemSO, rubyParts);
        }

        if (sapphireParts != playerSapphireParts) {
            foreach (var item in playerInventory.GetCurrentInventoryState()) {
                if (!item.Value.IsEmpty) {
                    if (item.Value.item.itemName == "Sapphire Parts")
                        playerInventory.ClearSlot(item.Key);
                }
            }

            if (sapphireParts > 0)
                playerInventory.AddItem(sapphirePartsItemSO, sapphireParts);
        }

        if (emeraldParts != playerEmeraldParts) {
            foreach (var item in playerInventory.GetCurrentInventoryState()) {
                if (!item.Value.IsEmpty) {
                    if (item.Value.item.itemName == "Emerald Parts")
                        playerInventory.ClearSlot(item.Key);
                }
            }

            if (emeraldParts > 0)
                playerInventory.AddItem(emeraldPartsItemSO, emeraldParts);
        }

        playerRubyParts = 0;
        playerSapphireParts = 0;
        playerEmeraldParts = 0;
        rubyParts = 0;
        sapphireParts = 0;
        emeraldParts = 0;
    }

    private void UpdateUI() {
        goldCoinText.text = Player.Instance.GoldCoin.ToString();
        silverCoinText.text = Player.Instance.SilverCoin.ToString();
        rubyPartsText.text = rubyParts.ToString();
        sapphirePartsText.text = sapphireParts.ToString();
        emeraldPartsText.text = emeraldParts.ToString();
    }

    public void BuyRubyButton() {
        if(Player.Instance.GoldCoin >= 3 && Player.Instance.SilverCoin >= 25 && rubyParts >= 3) {
            int result = playerInventory.AddItem(rubyItemSO, 1);

            if(result == 0) {
                Player.Instance.GoldCoin -= 3;
                Player.Instance.SilverCoin -= 25;
                rubyParts -= 3;
                UpdateUI();
            }
            else {
                GameObject errorMessage = Instantiate(errorMessagePrefab, buyRubyButton.gameObject.transform);
                errorMessage.GetComponentInChildren<ErrorText>().SetErrorText("Not enough space in inventory!");
            }
        }
        else {
            GameObject errorMessage = Instantiate(errorMessagePrefab, buyRubyButton.gameObject.transform);
            errorMessage.GetComponentInChildren<ErrorText>().SetErrorText("Can't afford to buy this!");
        }
    }

    public void BuySapphireButton() {
        if (Player.Instance.GoldCoin >= 5 && Player.Instance.SilverCoin >= 25 && sapphireParts >= 3) {
            int result = playerInventory.AddItem(sapphireItemSO, 1);

            if (result == 0) {
                Player.Instance.GoldCoin -= 5;
                Player.Instance.SilverCoin -= 25;
                sapphireParts -= 3;
                UpdateUI();
            } else {
                GameObject errorMessage = Instantiate(errorMessagePrefab, buySapphireButton.gameObject.transform);
                errorMessage.GetComponentInChildren<ErrorText>().SetErrorText("Not enough space in inventory!");
            }
        } else {
            GameObject errorMessage = Instantiate(errorMessagePrefab, buySapphireButton.gameObject.transform);
            errorMessage.GetComponentInChildren<ErrorText>().SetErrorText("Can't afford to buy this!");
        }
    }

    public void BuyEmeraldButton() {
        if (Player.Instance.GoldCoin >= 7 && Player.Instance.SilverCoin >= 50 && emeraldParts >= 3) {
            int result = playerInventory.AddItem(emeraldItemSO, 1);

            if (result == 0) {
                Player.Instance.GoldCoin -= 7;
                Player.Instance.SilverCoin -= 50;
                emeraldParts -= 3;
                UpdateUI();
            } else {
                GameObject errorMessage = Instantiate(errorMessagePrefab, buyEmeraldButton.gameObject.transform);
                errorMessage.GetComponentInChildren<ErrorText>().SetErrorText("Not enough space in inventory!");
            }
        } else {
            GameObject errorMessage = Instantiate(errorMessagePrefab, buyEmeraldButton.gameObject.transform);
            errorMessage.GetComponentInChildren<ErrorText>().SetErrorText("Can't afford to buy this!");
        }
    }

    public void CloseButton() {
        base.OpenGameUI();
    }
}
