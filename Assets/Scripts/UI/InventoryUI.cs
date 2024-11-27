using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySlot inventorySlotPrefab;
    [SerializeField] private RectTransform content;

    private List<InventorySlot> inventorySlotsList;

    private void Start() {
        inventorySlotsList = new List<InventorySlot>();
        InitializeInventoryUI(Player.Instance.InventorySize);
    }

    private void OnDestroy() {
        foreach(InventorySlot slot in inventorySlotsList) {
            slot.OnItemClicked -= OnInventroyItemClicked;
            slot.OnItemBeginDrag -= OnInventoryItemBeginDrag;
            slot.OnItemEndDrag -= OnInventoryItemEndDrag;
            slot.OnItemDropped -= OnInventoryItemDropped;
        }
    }

    public void InitializeInventoryUI(int inventorySize) {
        for(int i = 0; i < inventorySize; i++) {
            InventorySlot inventorySlot = Instantiate(inventorySlotPrefab, Vector3.zero, Quaternion.identity);
            inventorySlot.transform.SetParent(content);
            inventorySlotsList.Add(inventorySlot);

            inventorySlot.OnItemClicked += OnInventroyItemClicked;
            inventorySlot.OnItemBeginDrag += OnInventoryItemBeginDrag;
            inventorySlot.OnItemEndDrag += OnInventoryItemEndDrag;
            inventorySlot.OnItemDropped += OnInventoryItemDropped;
        }
    }

    private void OnInventroyItemClicked(InventorySlot slot)
    {
        Debug.Log("Clicked on an inventory slot: " + slot.gameObject.name);
    }

    private void OnInventoryItemBeginDrag(InventorySlot slot)
    {
        
    }

    private void OnInventoryItemEndDrag(InventorySlot slot)
    {
        
    }

    private void OnInventoryItemDropped(InventorySlot slot)
    {
        
    }

}
