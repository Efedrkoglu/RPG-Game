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
        
        //inventorySO.Initialize(Player.Instance.InventorySize);
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

    private void UpdateInventory() {
        foreach (var item in inventorySO.GetCurrentInventoryState()) {
            inventoryUI.UpdateSlotUI(item.Key, item.Value);
        }
    }

    public int AddItem(Item item) {
        return inventorySO.AddItem(item);
    }

    public void OnInventoryOpen() {
        UpdateInventory();
    }

    public void OnInventoryClose() {
        
    }

    public void HandleDesriptionRequest(int index) {
        ItemSO requestedItem = inventorySO.GetItemAt(index)?.item;
        if(requestedItem != null) {
            ItemType type = requestedItem.type;
            if(type == ItemType.Equipment) {
                bool equipped = ((EquipmentItemSO)requestedItem).Equipped;
                inventoryUI.SetItemDescription(requestedItem.itemImage, requestedItem.itemName, requestedItem.description, type, equipped);
            }
            else {
                inventoryUI.SetItemDescription(requestedItem.itemImage, requestedItem.itemName, requestedItem.description, type, false);
            }
        }
    }

    public void OnStartDragging(int index) {
        InventorySlot draggingSlot = inventorySO.GetItemAt(index);
        if(draggingSlot != null) {
            inventoryUI.SetMouseFollower(draggingSlot.item.itemImage, draggingSlot.amount, draggingSlot.item.isStackable);
        }
    }

    public void OnSwapItems(int itemIndex1, int itemIndex2) {
        if(itemIndex1 == itemIndex2)
            return;

        bool swap = false;
        if(inventorySO.GetItemAt(itemIndex1)?.item.ID == inventorySO.GetItemAt(itemIndex2)?.item.ID &&
         inventorySO.GetItemAt(itemIndex1).item.isStackable) 
        {
            inventorySO.StackItems(itemIndex1, itemIndex2);
        }
        else {
            swap = true;
        }

        if(swap) {
            inventorySO.SwapItems(itemIndex1, itemIndex2);
        }

        UpdateInventory();
        inventoryUI.SwapSelection(itemIndex1, itemIndex2);
    }
}
