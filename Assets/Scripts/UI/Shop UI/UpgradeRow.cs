using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeRow : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Sprite rowBg, selectedRowBg;
    [SerializeField] private Image background;

    [SerializeField] private Sprite goldIngotSprite, silverIngotSprite, leatherSprite;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private Image resourceImage;
    [SerializeField] private TextMeshProUGUI goldCoinAmount, silverCoinAmount, resourceAmount;

    public event Action<UpgradeRow> OnUpgradeRowClicked;

    private bool isSelected = false;
    private string upgradeKey;

    public void SetItem(Sprite itemImage, string itemName) {
        this.itemImage.sprite = itemImage;
        this.itemName.text = itemName;
    }

    public void SetResources(string resource, int goldAmount, int silverAmount, int resourceAmount, string upgradeKey) {

        if (resource == "Silver Ingot")
            resourceImage.sprite = silverIngotSprite;
        else if (resource == "Leather")
            resourceImage.sprite = leatherSprite;
        else if (resource == "Gold Ingot")
            resourceImage.sprite = goldIngotSprite;

        goldCoinAmount.text = goldAmount.ToString();
        silverCoinAmount.text = silverAmount.ToString();
        this.resourceAmount.text = resourceAmount.ToString();
        this.upgradeKey = upgradeKey;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if(eventData.button == PointerEventData.InputButton.Left) {
            OnUpgradeRowClicked?.Invoke(this);
        }
    }

    public void Deselect() {
        background.sprite = rowBg;
        isSelected = false;
    }

    public void SelectRow() {
        background.sprite = selectedRowBg;
        isSelected = true;
    }

    public bool IsSelected {
        get { return isSelected; }
    }

    public string UpgradeKey {
        get { return upgradeKey; }
    }
}
