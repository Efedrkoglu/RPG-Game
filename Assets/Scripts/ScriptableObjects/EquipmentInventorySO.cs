using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment Inventory", menuName = "ScriptableObjects/Inventory/Equipment Inventory")]
public class EquipmentInventorySO : ScriptableObject
{
    [SerializeField] private List<InventorySlot> slots;

    public event Action<ItemSO> OnEquipmentUnequipped;

    public void Initialize() {
        slots = new List<InventorySlot>();

        for(int i = 0; i < 6; i++) {
            slots.Add(new InventorySlot());
        }
    }

    public void EquipItem(EquipmentItemSO item) {
        switch(item.equipmentType) {
            case EquipmentType.Helmet:
                if (!slots[0].IsEmpty)
                    UnequipItem(0);
                slots[0].Item = item;
                break;

            case EquipmentType.BodyArmor:
                if (!slots[1].IsEmpty)
                    UnequipItem(1);
                slots[1].Item = item;
                break;

            case EquipmentType.Boots:
                if (!slots[2].IsEmpty)
                    UnequipItem(2);
                slots[2].Item = item;
                break;

            case EquipmentType.Weapon:
                if (!slots[3].IsEmpty)
                    UnequipItem(3);
                slots[3].Item = item;
                break;

            case EquipmentType.Shield:
                if (!slots[4].IsEmpty)
                    UnequipItem(4);
                slots[4].Item = item;
                break;

            case EquipmentType.Trinket:
                if (!slots[5].IsEmpty)
                    UnequipItem(5);
                slots[5].Item = item;
                break;

            default:
                return;
        }
    }

    public void UnequipItem(int index) {
        EquipmentItemSO unequippedItem = (EquipmentItemSO)slots[index].item;
        slots[index] = new InventorySlot();

        switch (unequippedItem.equipmentType) {
            case EquipmentType.Weapon:
                Player.Instance.Damage -= unequippedItem.attackDamage;
                break;

            case EquipmentType.Shield:
                Player.Instance.BlockChance -= unequippedItem.blockChance;
                break;

            case EquipmentType.BodyArmor:
                Player.Instance.DefPercent -= unequippedItem.defValue;
                break;

            case EquipmentType.Helmet:
                Player.Instance.DefPercent -= unequippedItem.defValue;
                break;

            case EquipmentType.Boots:
                Player.Instance.DefPercent -= unequippedItem.defValue;
                break;

            case EquipmentType.Trinket:
                break;

            default:
                break;
        }

        OnEquipmentUnequipped?.Invoke(unequippedItem);
    }

    public void ClearSlot(int index) {
        EquipmentItemSO unequippedItem = (EquipmentItemSO)slots[index].item;
        slots[index] = new InventorySlot();

        switch (unequippedItem.equipmentType) {
            case EquipmentType.Weapon:
                Player.Instance.Damage -= unequippedItem.attackDamage;
                break;

            case EquipmentType.Shield:
                Player.Instance.BlockChance -= unequippedItem.blockChance;
                break;

            case EquipmentType.BodyArmor:
                Player.Instance.DefPercent -= unequippedItem.defValue;
                break;

            case EquipmentType.Helmet:
                Player.Instance.DefPercent -= unequippedItem.defValue;
                break;

            case EquipmentType.Boots:
                Player.Instance.DefPercent -= unequippedItem.defValue;
                break;

            case EquipmentType.Trinket:
                break;

            default:
                break;
        }
    }

    public Dictionary<int, InventorySlot> GetInventoryState() {
        Dictionary<int, InventorySlot> inventoryState = new Dictionary<int, InventorySlot>();

        for(int i = 0; i < slots.Count; i++) {
            inventoryState[i] = slots[i];
        }
        return inventoryState;
    }

    public EquipmentItemSO getItemAt(int index) {
        if(index < slots.Count) {
            if (!slots[index].IsEmpty) {
                return (EquipmentItemSO)slots[index].Item;
            }
        }
        return null;
    }
}
