using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "ScriptableObjects/Inventory")]
public class InventorySO : ScriptableObject
{
    public List<InventorySlot> slots;
    public int size;

    public void Initialize() {
        if(size == 0)
            size = Player.Instance.InventorySize;

        slots = new List<InventorySlot>();
        for(int i = 0; i < size; i++) {
            slots.Add(new InventorySlot());
        }
    }

    public void AddItem(ItemSO item, int amount) {
        for(int i = 0; i < size; i++) {
            if(slots[i].IsEmpty) {
                slots[i].Item = item;
                slots[i].Amount = amount;
            }
        }
    }

    public Dictionary<int, InventorySlot> GetCurrentInventoryState() {
        Dictionary<int, InventorySlot> inventoryState = new Dictionary<int, InventorySlot>();

        for(int i = 0; i < size; i++) {
            inventoryState[i] = slots[i];
        }
        return inventoryState;
    }

    public InventorySlot GetItemAt(int index) {
        return slots[index].IsEmpty ? null : slots[index];
    }

    public void SwapItems(int itemIndex1, int itemIndex2) {
        InventorySlot temp = slots[itemIndex1];
        slots[itemIndex1] = slots[itemIndex2];
        slots[itemIndex2] = temp;
    }
}

[Serializable]
public class InventorySlot
{
    public ItemSO item;
    public int amount;
    public bool IsEmpty => item == null;

    public InventorySlot() {
        item = null;
        amount = 0;
    }

    public ItemSO Item {
        get { return item; }
        set { item = value; }
    }

    public int Amount {
        get { return amount; }
        set { amount = value; }
    }
}
