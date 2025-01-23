using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot
{
    private ItemSO item;
    private int amount;

    public Loot(ItemSO _item, int _amount) {
        this.item = _item;
        this.amount = _amount;
    }

    public ItemSO Item {
        get { return item; }
    }

    public int Amount {
        get { return amount; }
        set { amount = value; }
    }

    public bool IsEmpty() {
        if (amount == 0)
            return true;

        return false;
    }
}
