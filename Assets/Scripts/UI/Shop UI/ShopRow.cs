using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopRow : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Sprite row, selectedRow;
    [SerializeField] private Image background;

    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName, goldCoinText, silverCoinText;

    private ItemSO item;
    private int goldAmount, silverAmount;

    public event Action<ShopRow> OnRowClick;

    public void InitRow(ItemSO item, int goldAmount, int silverAmount) {
        this.item = item;
        this.goldAmount = goldAmount;
        this.silverAmount = silverAmount;

        background.sprite = row;
        itemImage.sprite = item.itemImage;
        itemName.text = item.itemName;
        goldCoinText.text = goldAmount.ToString();
        silverCoinText.text = silverAmount.ToString();
    }

    public void OnPointerClick(PointerEventData eventData) {
        OnRowClick?.Invoke(this);
        SelectRow();
    }

    public void SelectRow() {
        background.sprite = selectedRow;
    }

    public void DeselectRow() {
        background.sprite = row;
    }
}
