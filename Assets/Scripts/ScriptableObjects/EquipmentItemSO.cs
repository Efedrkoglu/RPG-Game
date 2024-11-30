using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment Item", menuName = "ScriptableObjects/Items/Equipment Item")]
public class EquipmentItemSO : ItemSO
{
    public int attackDamage;
    public int defValue;

    private bool equipped;

    private void Awake() {
        type = ItemType.Equipment;
        equipped = false;
    }

    public bool Equipped {
        get { return equipped; }
        set { equipped = value; }
    }
}
