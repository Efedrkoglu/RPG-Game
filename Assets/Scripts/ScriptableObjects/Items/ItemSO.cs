using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Items/Item")]
public class ItemSO : ScriptableObject
{
    public int ID => GetInstanceID();
    public Sprite itemImage;
    public string itemName;
    [TextArea] public string description;
    public bool isStackable;
    public int stackAmount = 1;

    protected ItemType type;

    private void Awake() {
        type = ItemType.Default;
    }

    public virtual bool UseItem() {
        return false;
    }

    public ItemType Type {
        get { return type; }
    }
}

public enum ItemType
{
    Default,
    Consumable,
    Equipment
}
