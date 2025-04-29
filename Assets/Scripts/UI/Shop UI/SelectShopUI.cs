using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectShopUI : BaseShopUI
{
    [SerializeField] private GameObject bankUI, jewelryUI;

    public void JewelryButton() {
        jewelryUI.SetActive(true);
        gameObject.SetActive(false);
    }

    public void BankButton() {
        bankUI.SetActive(true);
        gameObject.SetActive(false);
    }

    public void CloseButton() {
        base.OpenGameUI();
    }
}
