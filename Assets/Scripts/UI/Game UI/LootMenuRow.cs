using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LootMenuRow : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Sprite bg, bg_selected;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemAmount;

    public bool isSelected;
    public event Action RowSelected;

    public void SetRow(ItemSO item, int amount) {
        isSelected = false;
        itemImage.sprite = item.itemImage;
        itemName.text = item.itemName;
        itemAmount.text = "x" + amount.ToString();
    }

    public void SelectRow() {
        isSelected = true;
        backgroundImage.sprite = bg_selected;
    }

    public void DeselectRow() {
        isSelected = false;
        backgroundImage.sprite = bg;
    }

    public void OnPointerClick(PointerEventData eventData) {
        RowSelected?.Invoke();
        SelectRow();
    }
}
