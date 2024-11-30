using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySlotUI inventorySlotPrefab;
    [SerializeField] private RectTransform content;
    [SerializeField] private Image itemDescriptionImage;
    [SerializeField] private TextMeshProUGUI itemDescriptionName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private MouseFollower mouseFollower;
    [SerializeField] private Button dropItemButton;
    [SerializeField] private Button useItemButton;

    private List<InventorySlotUI> inventorySlotsList;
    private int currentlyDraggedItemIndex = -1;

    public event Action<int> OnDescriptionRequested, OnStartDragging;
    public event Action<int, int> OnSwapItems;

    private void Start() {
        mouseFollower.Toggle(false);
        ResetItemDescription();
    }

    private void OnDestroy() {
        foreach(InventorySlotUI slot in inventorySlotsList) {
            slot.OnItemClicked -= OnInventroyItemClicked;
            slot.OnItemBeginDrag -= OnInventoryItemBeginDrag;
            slot.OnItemEndDrag -= OnInventoryItemEndDrag;
            slot.OnItemDropped -= OnInventoryItemDropped;
        }
    }

    private void OnEnable() {
        
    }

    private void OnDisable() {
        ResetSelection();
        ResetItemDescription();
    }

    public void InitializeInventoryUI(int inventorySize) {
        if(inventorySlotsList == null)
            inventorySlotsList = new List<InventorySlotUI>();

        for(int i = 0; i < inventorySize; i++) {
            InventorySlotUI inventorySlot = Instantiate(inventorySlotPrefab, Vector3.zero, Quaternion.identity);
            inventorySlot.transform.SetParent(content);
            inventorySlotsList.Add(inventorySlot);

            inventorySlot.OnItemClicked += OnInventroyItemClicked;
            inventorySlot.OnItemBeginDrag += OnInventoryItemBeginDrag;
            inventorySlot.OnItemEndDrag += OnInventoryItemEndDrag;
            inventorySlot.OnItemDropped += OnInventoryItemDropped;
        }
    }

    public void UpdateSlotUI(int slotIndex, InventorySlot slot) {
        if(slotIndex < inventorySlotsList.Count) {
            if(slot.IsEmpty)
                inventorySlotsList[slotIndex].ResetSlotData();
            else
                inventorySlotsList[slotIndex].SetSlotItem(slot.item.itemImage, slot.amount, slot.item.isStackable);
        }
    }

    public void SwapSelection(int index1, int index2) {
        inventorySlotsList[index1].Deselect();
        inventorySlotsList[index2].Select();
    }  

    private void ResetSelection() {
        foreach(InventorySlotUI slot in inventorySlotsList) {
            slot.Deselect();
        }
    }

    private void OnInventroyItemClicked(InventorySlotUI slot)
    {
        ResetSelection();
        ResetItemDescription();
        if(slot.isEmpty())
            return;
        int index = inventorySlotsList.IndexOf(slot);
        slot.Select();
        
        OnDescriptionRequested?.Invoke(index);
    }

    private void OnInventoryItemBeginDrag(InventorySlotUI slot)
    {
        if(slot.isEmpty())
            return;

        int index = inventorySlotsList.IndexOf(slot);
        currentlyDraggedItemIndex = index;
        OnInventroyItemClicked(slot);
        OnStartDragging?.Invoke(index);
    }

    public void SetMouseFollower(Sprite sprite, int amount, bool isStackable) {
        mouseFollower.SetData(sprite, amount, isStackable);
        mouseFollower.Toggle(true);
    }

    private void OnInventoryItemEndDrag(InventorySlotUI slot)
    {
        currentlyDraggedItemIndex = -1;
        mouseFollower.Toggle(false);
    }

    private void OnInventoryItemDropped(InventorySlotUI slot)
    {
        if(currentlyDraggedItemIndex == -1)
            return;
        
        int index = inventorySlotsList.IndexOf(slot);
        OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
    }

    private void ResetItemDescription() {
        itemDescriptionImage.gameObject.SetActive(false);
        itemDescriptionName.text = "";
        itemDescription.text = "";
        useItemButton.interactable = false;
        dropItemButton.interactable = false;
        useItemButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "use";
    }

    public void SetItemDescription(Sprite sprite, string itemName, string itemDescription, ItemType type, bool equipped) {
        itemDescriptionImage.sprite = sprite;
        itemDescriptionImage.gameObject.SetActive(true);
        itemDescriptionName.text = itemName;
        this.itemDescription.text = itemDescription;

        switch(type) {
            case ItemType.Default:
                dropItemButton.interactable = true;
                useItemButton.interactable = false;
                useItemButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "use";
                break;

            case ItemType.Consumable:
                useItemButton.interactable = true;
                useItemButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "use";
                if (Player.Instance.IsInCombat)
                    dropItemButton.interactable = false;
                else
                    dropItemButton.interactable = true;
                break;

            case ItemType.ConsumableOnlyInCombat:
                if(Player.Instance.IsInCombat) {
                    useItemButton.interactable = true;
                    useItemButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "use";
                    dropItemButton.interactable = false;
                }
                else {
                    useItemButton.interactable = false;
                    useItemButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "use";
                    dropItemButton.interactable = true;
                }
                break;

            case ItemType.Equipment:
                if(Player.Instance.IsInCombat) {
                    useItemButton.interactable = false;
                    dropItemButton.interactable = false;
                }
                else {
                    useItemButton.interactable = true;
                    dropItemButton.interactable = true;
                }
                if (equipped)
                    useItemButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "unequip";
                else
                    useItemButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "equip";
                break;

            default:
                Debug.Log("Item type error in InventoryUI.SetItemDescription()");
                break;
        }
    }

}
