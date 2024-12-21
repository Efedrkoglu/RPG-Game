using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private InventorySO inventorySO;

    [SerializeField] private EquipmentInventorySO equipmentInventorySO;
    [SerializeField] private EquipmentInventoryUI equipmentInventoryUI;
    [SerializeField] private Item itemPrefab;

    public event Action OnInventoryItemUsed;

    private void Start() {
        //InventoryPanel.OnInventoryToggleOpen += OnInventoryOpen;

        //inventorySO.Initialize(Player.Instance.InventorySize);
        inventoryUI.InitializeInventoryUI(inventorySO.size);

        inventoryUI.InventoryUpdateRequested += UpdateInventory;
        inventoryUI.OnDescriptionRequested += HandleDesriptionRequest;
        inventoryUI.OnStartDragging += OnStartDragging;
        inventoryUI.OnSwapItems += OnSwapItems;
        inventoryUI.OnDropButton += OnDropItem;
        inventoryUI.OnUseButton += OnUseItem;

        inventorySO.EquipmentEquipped += OnEquipmentEquipped;

        equipmentInventoryUI.EquipmentInventoryUpdateRequested += UpdateEquipmentInventory;
        equipmentInventoryUI.UnEquipButtonClicked += UnequipItem;

        equipmentInventorySO.OnEquipmentUnequipped += OnEquipmentUnequipped;
    }

    private void OnDestroy() {
        //InventoryPanel.OnInventoryToggleOpen -= OnInventoryOpen;

        inventoryUI.InventoryUpdateRequested -= UpdateInventory;
        inventoryUI.OnDescriptionRequested -= HandleDesriptionRequest;
        inventoryUI.OnStartDragging -= OnStartDragging;
        inventoryUI.OnSwapItems -= OnSwapItems;
        inventoryUI.OnDropButton -= OnDropItem;
        inventoryUI.OnUseButton -= OnUseItem;

        inventorySO.EquipmentEquipped -= OnEquipmentEquipped;

        equipmentInventoryUI.EquipmentInventoryUpdateRequested -= UpdateEquipmentInventory;
        equipmentInventoryUI.UnEquipButtonClicked -= UnequipItem;

        equipmentInventorySO.OnEquipmentUnequipped -= OnEquipmentUnequipped;
    }

    private void UpdateInventory() {
        foreach(var item in inventorySO.GetCurrentInventoryState()) {
            inventoryUI.UpdateSlotUI(item.Key, item.Value);
        }
    }

    private void UpdateEquipmentInventory() {
        foreach(var item in equipmentInventorySO.GetInventoryState()) {
            equipmentInventoryUI.UpdateUI(item.Key, item.Value);
        }
    }

    public int AddItem(Item item) {
        return inventorySO.AddItem(item);
    }

    public void OnInventoryOpen() {
        UpdateInventory();
    }

    public void HandleDesriptionRequest(int index) {
        ItemSO requestedItem = inventorySO.GetItemAt(index)?.item;
        if(requestedItem != null) {
            inventoryUI.SetItemDescription(requestedItem);
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

    public void OnDropItem(int dropAmount, int index) {
        ItemSO droppedItem = inventorySO.GetItemAt(index)?.item;
        int droppedItemAmount = inventorySO.DropItem(dropAmount, index);

        if(droppedItem != null && droppedItemAmount > 0) {
            Item item = Instantiate(itemPrefab, transform.position, Quaternion.identity);
            item.InitializeItem(droppedItem, droppedItemAmount);
        }
        UpdateInventory();
    }

    public void UnequipItem(int index) {
        equipmentInventorySO.UnequipItem(index);
    } 

    public void OnEquipmentUnequipped(ItemSO unequippedItem) {
        if(inventorySO.HasEmptySlot()) {
            Item item = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            item.InitializeItem(unequippedItem, 1);
            inventorySO.AddItem(item);
            Destroy(item.gameObject);
        }
        else {
            Item item = Instantiate(itemPrefab, transform.position, Quaternion.identity);
            item.InitializeItem(unequippedItem, 1);
        }
        UpdateEquipmentInventory();
    }

    public void OnEquipmentEquipped(EquipmentItemSO equipmentItem) {
        equipmentInventorySO.EquipItem(equipmentItem);
        UpdateEquipmentInventory();
    }

    public void OnUseItem(int index) {
        inventorySO.UseItem(index);
        UpdateInventory();

        if(Player.Instance.IsInCombat)
            OnInventoryItemUsed?.Invoke();
    }

    public InventorySO getInventorySO() {
        return inventorySO;
    }
}
