using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootMenuRow : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemAmount;

    public void SetRow(ItemSO item, int amount) {
        itemImage.sprite = item.itemImage;
        itemName.text = item.itemName;
        itemAmount.text = amount.ToString();
    }
}
