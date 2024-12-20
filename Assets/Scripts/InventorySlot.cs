using System;

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
