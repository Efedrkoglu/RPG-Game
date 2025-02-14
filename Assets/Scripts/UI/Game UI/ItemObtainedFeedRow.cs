using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemObtainedFeedRow : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI info;

    public void Initialize(Sprite _itemImage, string itemName, int amount) {
        string infoString = itemName + " x " + amount.ToString();
        itemImage.sprite = _itemImage;
        info.text = infoString;
    }

    public void Disappear() {
        Destroy(gameObject);
    }
}
