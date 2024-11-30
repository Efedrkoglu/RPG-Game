using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Items/Item")]
public class ItemSO : ScriptableObject
{
    public int ID => GetInstanceID();
    public ItemType type;
    public Sprite itemImage;
    public string itemName;
    [TextArea] public string description;
    public bool isStackable;
    public int stackAmount = 1;
    
    private void Awake() {
        type = ItemType.Default;
    }
}

public enum ItemType
{
    Default,
    Consumable,
    ConsumableOnlyInCombat,
    Equipment
}
