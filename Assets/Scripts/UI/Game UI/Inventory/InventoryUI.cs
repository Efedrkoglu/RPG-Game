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
    [SerializeField] private Button dropItemMenuButton;
    [SerializeField] private Button cancelDropItemButton;
    [SerializeField] private Button dropItemButton;
    [SerializeField] private Button useItemButton;

    [SerializeField] private GameObject inventoryBG;
    [SerializeField] private GameObject dropItemMenu;
    [SerializeField] private TMP_InputField dropInput;

    private List<InventorySlotUI> inventorySlotsList;
    private int currentlyDraggedItemIndex = -1;
    private int selectedItemIndex = -1;

    public event Action InventoryUpdateRequested;
    public event Action<int> OnDescriptionRequested, OnStartDragging, OnUseButton;
    public event Action<int, int> OnSwapItems, OnDropButton;

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
        InventoryUpdateRequested?.Invoke();
    }

    private void OnDisable() {
        ResetSelection();
        ResetItemDescription();
        CancelDropButton();
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
        selectedItemIndex = index2;
    }  

    private void ResetSelection() {
        if(inventorySlotsList == null) {
            Debug.Log("Inventory slots list is null.");
            return;
        }

        foreach (InventorySlotUI slot in inventorySlotsList) {
            slot.Deselect();
        }
        selectedItemIndex = -1;
    }

    private void OnInventroyItemClicked(InventorySlotUI slot)
    {
        ResetSelection();
        ResetItemDescription();
        if(slot.isEmpty())
            return;
        int index = inventorySlotsList.IndexOf(slot);
        slot.Select();
        selectedItemIndex = index;
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
        dropItemMenuButton.interactable = false;
        useItemButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Use";
    }

    public void SetItemDescription(ItemSO item) {
        itemDescriptionImage.sprite = item.itemImage;
        itemDescriptionImage.gameObject.SetActive(true);
        itemDescriptionName.text = item.itemName;
        itemDescription.text = item.description;

        if (Player.Instance.IsInCombat)
            dropItemMenuButton.interactable = false;
        else
            dropItemMenuButton.interactable = true;

        switch(item.Type) {
            case ItemType.Default:
                useItemButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Use";
                useItemButton.interactable = false;
                break;

            case ItemType.Consumable:
                if(Player.Instance.IsInCombat) {
                    useItemButton.interactable = true;
                    itemDescription.text += "\nConsuming Cost: " + ((ConsumableItemSO)item).consumingCost;
                }
                else {
                    if (((ConsumableItemSO)item).onlyConsumableDuringCombat)
                        useItemButton.interactable = false;
                    else
                        useItemButton.interactable = true;
                }
                break;

            case ItemType.Equipment:
                if(Player.Instance.IsInCombat)
                    useItemButton.interactable = false;
                else
                    useItemButton.interactable = true;

                if (((EquipmentItemSO)item).Equipped)
                    useItemButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Unequip";
                else
                    useItemButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Equip";
                break;

            default:
                Debug.Log("Item type error in InventoryUI.SetItemDescription()");
                break;
        }
    }

    private void ResetInventoryUI() {
        ResetSelection();
        ResetItemDescription();
    }

    public void DropMenuButton() {
        if (!inventorySlotsList[selectedItemIndex].isStackable()) {
            OnDropButton?.Invoke(1, selectedItemIndex);
            if (inventorySlotsList[selectedItemIndex].isEmpty())
                ResetInventoryUI();
            return;
        }
        inventoryBG.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        dropItemMenu.SetActive(true);
    }

    public void DropButton() {
        int dropAmount = Convert.ToInt32(dropInput.text);
        OnDropButton?.Invoke(dropAmount, selectedItemIndex);
        if (inventorySlotsList[selectedItemIndex].isEmpty())
            ResetInventoryUI();
        CancelDropButton();
    }

    public void CancelDropButton() {
        inventoryBG.GetComponent<Image>().color = new Color(0, 0, 0, .92f);
        dropInput.text = "1";
        dropItemMenu.SetActive(false);
    }

    public void UseButton() {
        OnUseButton?.Invoke(selectedItemIndex);
        if (inventorySlotsList[selectedItemIndex].isEmpty())
            ResetInventoryUI();
    }

}
