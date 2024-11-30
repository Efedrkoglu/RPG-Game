using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumable Item", menuName = "ScriptableObjects/Items/Consumable Item")]
public class ConsumableItemSO : ItemSO
{
    public int healAmount;
    public bool onlyConsumableDuringCombat;

    private void Awake() {
        type = ItemType.Consumable;
    }
}
