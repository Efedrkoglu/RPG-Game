using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "ScriptableObjects/Inventory")]
public class InventorySO : ScriptableObject
{
    public List<InventorySlot> slots;
    public int size;

    public event Action<Item, int> OnAddItem;

    public void Initialize(int _size) {
        size = _size;

        slots = new List<InventorySlot>();
        for(int i = 0; i < size; i++) {
            slots.Add(new InventorySlot());
        }
    }

    public int AddItem(Item item) {
        for(int i = 0; i < size; i++) {
            if(slots[i].IsEmpty) {
                slots[i].Item = item.GetItem;
                slots[i].Amount = item.Amount;
                OnAddItem?.Invoke(item, item.Amount);
                return 0;
            }
            else if (slots[i].item.ID == item.GetItem.ID && item.GetItem.isStackable) {
                int maxStackAmount = item.GetItem.stackAmount;
                if (slots[i].amount == maxStackAmount)
                    continue;

                int currentStackAmount = slots[i].amount + item.Amount;
                if(currentStackAmount > maxStackAmount) {
                    slots[i].amount = maxStackAmount;
                    OnAddItem?.Invoke(item, item.Amount - (currentStackAmount % maxStackAmount));
                    item.Amount = currentStackAmount % maxStackAmount;
                }
                else {
                    slots[i].amount = currentStackAmount;
                    OnAddItem?.Invoke(item, item.Amount);
                    return 0;
                }
            }
        }
        return item.Amount;
    }

    public int DropItem(int dropAmount, int index) {
        if(dropAmount > slots[index].amount) {
            dropAmount = slots[index].amount;
            slots[index].amount -= dropAmount;
        }
        else {
            slots[index].amount -= dropAmount;
        }

        if (slots[index].amount == 0) {
            slots[index] = new InventorySlot();
        }

        return dropAmount;
    }

    public void UseItem(int index) {
        if(slots[index].item.UseItem()) 
            DropItem(1, index);
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

    public void StackItems(int itemIndex1, int itemIndex2) {
        //index1 = suruklenen item, index2 = droplanan item
        int maxStackAmount = slots[itemIndex2].item.stackAmount;
        if(slots[itemIndex1].amount == maxStackAmount  || slots[itemIndex2].amount == maxStackAmount) {
            SwapItems(itemIndex1, itemIndex2);
            return;
        }

        int currentStackAmount = slots[itemIndex1].amount + slots[itemIndex2].amount;

        if(currentStackAmount > maxStackAmount) {
            slots[itemIndex1].amount = currentStackAmount % maxStackAmount;
            slots[itemIndex2].amount = maxStackAmount;
        }
        else {
            slots[itemIndex1] = new InventorySlot();
            slots[itemIndex2].amount = currentStackAmount;
        }
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
