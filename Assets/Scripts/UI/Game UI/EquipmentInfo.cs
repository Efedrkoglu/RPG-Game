using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EquipmentInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;

    private void Start() {
        gameObject.SetActive(false);
    }

    public void SetInfo(EquipmentItemSO equipmentItem, Vector3 position) {
        itemName.text = equipmentItem.itemName;
        itemDescription.text = equipmentItem.equipmentStats;
        transform.position = position;
    }
}
