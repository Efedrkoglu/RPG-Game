using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private InventorySO inventorySO;

    private void Start() {
        InventoryPanel.OnInventoryToggleOpen += OnInventoryOpen;
        InventoryPanel.OnInventoryToggleClose += OnInventoryClose;
        
        //inventorySO.Initialize();
        inventoryUI.InitializeInventoryUI(inventorySO.size);

        inventoryUI.OnDescriptionRequested += HandleDesriptionRequest;
        inventoryUI.OnStartDragging += OnStartDragging;
        inventoryUI.OnSwapItems += OnSwapItems;
    }

    private void OnDestroy() {
        InventoryPanel.OnInventoryToggleOpen -= OnInventoryOpen;
        InventoryPanel.OnInventoryToggleClose -= OnInventoryClose;

        inventoryUI.OnDescriptionRequested -= HandleDesriptionRequest;
        inventoryUI.OnStartDragging -= OnStartDragging;
        inventoryUI.OnSwapItems -= OnSwapItems;
    }

    public void OnInventoryOpen() {
        foreach(var item in inventorySO.GetCurrentInventoryState()) {
            inventoryUI.UpdateSlotUI(item.Key, item.Value);
        }
    }

    public void OnInventoryClose() {

    }

    public void HandleDesriptionRequest(int index) {
        ItemSO requestedItem = inventorySO.GetItemAt(index)?.item;
        if(requestedItem != null) {
            inventoryUI.SetItemDescription(requestedItem.itemImage, requestedItem.itemName, requestedItem.description);
        }
    }

    public void OnStartDragging(int index) {
        InventorySlot draggingSlot = inventorySO.GetItemAt(index);
        if(draggingSlot != null) {
            inventoryUI.SetMouseFollower(draggingSlot.item.itemImage, draggingSlot.amount);
        }
    }

    public void OnSwapItems(int itemIndex1, int itemIndex2) {
        inventorySO.SwapItems(itemIndex1, itemIndex2);

        foreach(var item in inventorySO.GetCurrentInventoryState()) {
            inventoryUI.UpdateSlotUI(item.Key, item.Value);
        }
        inventoryUI.SwapSelection(itemIndex1, itemIndex2);
    }
}
