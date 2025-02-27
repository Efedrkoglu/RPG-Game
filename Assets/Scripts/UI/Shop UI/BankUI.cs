using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BankUI : BaseShopUI
{
    [SerializeField] private TextMeshProUGUI playerGoldCoinText, playerSilverCoinText;
    [SerializeField] private Button buyGoldButton, buySilverButton;

    private void OnEnable() {
        UpdatePlayerCoinTexts();
        UpdateButtons();
    }

    public void UpdatePlayerCoinTexts() {
        playerGoldCoinText.text = Player.Instance.GoldCoin.ToString();
        playerSilverCoinText.text = Player.Instance.SilverCoin.ToString();
    }

    public void UpdateButtons() {
        buyGoldButton.interactable = Player.Instance.SilverCoin >= 100;
        buySilverButton.interactable = Player.Instance.GoldCoin >= 1;
    }

    public void CloseButton() {
        base.OpenGameUI();
    }

    public void BuyGoldButton() {
        Player.Instance.GoldCoin += 1;
        Player.Instance.SilverCoin -= 100;
        UpdatePlayerCoinTexts();
        UpdateButtons();
    }

    public void BuySilverButton() {
        Player.Instance.SilverCoin += 100;
        Player.Instance.GoldCoin -= 1;
        UpdatePlayerCoinTexts();
        UpdateButtons();
    }
}
