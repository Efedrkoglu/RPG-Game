using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment Item", menuName = "ScriptableObjects/Items/Equipment Item")]
public class EquipmentItemSO : ItemSO
{
    public int attackDamage;
    public int defValue;
    public int blockChance;
    public string equipmentStats;
    public EquipmentType equipmentType;

    private bool equipped;

    private void Awake() {
        type = ItemType.Equipment;
        equipped = false;
    }

    public override bool UseItem() {
        switch (equipmentType) {
            case EquipmentType.Weapon:
                Player.Instance.Damage += attackDamage;
                break;

            case EquipmentType.Shield:
                Player.Instance.BlockChance += blockChance;
                break;

            case EquipmentType.BodyArmor:
                Player.Instance.DefPercent += defValue;
                break;

            case EquipmentType.Helmet:
                Player.Instance.DefPercent += defValue;
                break;

            case EquipmentType.Boots:
                Player.Instance.DefPercent += defValue;
                break;

            case EquipmentType.Trinket:
                break;

            default:
                return false;
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
    Boots,
    Trinket
}
