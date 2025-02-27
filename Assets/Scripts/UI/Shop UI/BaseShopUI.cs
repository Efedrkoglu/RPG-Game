using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseShopUI : MonoBehaviour
{
    private ToggleShopUI toggleShopUI;

    public void setToggleShopUI(ToggleShopUI toggleShopUI) {
        this.toggleShopUI = toggleShopUI;
    }

    public void OpenGameUI() {
        if(toggleShopUI != null)
            toggleShopUI.OpenGameUI();
    }
}
