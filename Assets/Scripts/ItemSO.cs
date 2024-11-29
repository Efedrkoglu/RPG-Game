using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Items/Item")]
public class ItemSO : ScriptableObject
{
    public Sprite itemImage;
    public string itemName;
    [TextArea] public string description;
    public int stackAmount = 1;
}
