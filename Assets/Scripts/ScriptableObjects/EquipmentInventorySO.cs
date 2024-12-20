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
                slots[0].Item = item;
                break;

            case EquipmentType.BodyArmor:
                slots[1].Item = item;
                break;

            case EquipmentType.Boots:
                slots[2].Item = item;
                break;

            case EquipmentType.Weapon:
                slots[3].Item = item;
                break;

            case EquipmentType.Shield:
                slots[4].Item = item;
                break;

            case EquipmentType.Trinket:
                slots[5].Item = item;
                break;

            default:
                return;
        }
    }

    public void UnequipItem(int index) {
        ItemSO unequippedItem = slots[index].item;
        slots[index] = new InventorySlot();
        OnEquipmentUnequipped?.Invoke(unequippedItem);
    }

    public Dictionary<int, InventorySlot> GetInventoryState() {
        Dictionary<int, InventorySlot> inventoryState = new Dictionary<int, InventorySlot>();

        for(int i = 0; i < slots.Count; i++) {
            inventoryState[i] = slots[i];
        }
        return inventoryState;
    }
}
