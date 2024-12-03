using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment Item", menuName = "ScriptableObjects/Items/Equipment Item")]
public class EquipmentItemSO : ItemSO
{
    public int attackDamage;
    public int defValue;
    public EquipmentType equipmentType;

    private bool equipped;

    private void Awake() {
        type = ItemType.Equipment;
        equipped = false;
    }

    public override bool UseItem() {
        switch(equipmentType) {
            case EquipmentType.Weapon:
                
                break;
        }
        return true;
    }

    public bool Equipped {
        get { return equipped; }
        set { equipped = value; }
    }
}

public enum EquipmentType
{
    Weapon,
    Shield,
    BodyArmor,
    Helmet,
    Boot
}
