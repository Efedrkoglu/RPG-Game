using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySlot inventorySlotPrefab;
    [SerializeField] private RectTransform content;
    [SerializeField] private Image itemDescriptionImage;
    [SerializeField] private TextMeshProUGUI itemDescriptionName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private MouseFollower mouseFollower;


    public Sprite testItem1, testItem2;
    public int testAmount1, testAmount2;
    public string testItemName1, testItemName2, testItemDescription;

    private List<InventorySlot> inventorySlotsList;
    private int currentlyDraggedItemIndex = -1;

    private void Start() {
        inventorySlotsList = new List<InventorySlot>();
        mouseFollower.Toggle(false);
        InitializeInventoryUI(Player.Instance.InventorySize);
        ResetItemDescription();
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

        inventorySlotsList[0].SetSlotItem(testItem1, testAmount1);
        inventorySlotsList[1].SetSlotItem(testItem2, testAmount2);
    }

    private void OnInventroyItemClicked(InventorySlot slot)
    {
        for(int i = 0; i < inventorySlotsList.Count; i++) {
            inventorySlotsList[i].Deselect();
        }

        int selectedIndex = inventorySlotsList.IndexOf(slot);
        if(selectedIndex == 0)
            SetItemDescription(testItem1, testItemName1, testItemDescription);
        else
            SetItemDescription(testItem2, testItemName2, testItemDescription);
        slot.Select();
    }

    private void OnInventoryItemBeginDrag(InventorySlot slot)
    {
        int index = inventorySlotsList.IndexOf(slot);
        if(index == -1)
            return;

        currentlyDraggedItemIndex = index;
        mouseFollower.Toggle(true);
        mouseFollower.SetData(index == 0 ? testItem1 : testItem2, index == 0 ? testAmount1 : testAmount2);
    }

    private void OnInventoryItemEndDrag(InventorySlot slot)
    {
        Debug.Log("End drag");
        mouseFollower.Toggle(false);
    }

    private void OnInventoryItemDropped(InventorySlot slot)
    {
        int index = inventorySlotsList.IndexOf(slot);
        if(index == -1) {
            currentlyDraggedItemIndex = -1;
            mouseFollower.Toggle(false);
            return;
        }

        inventorySlotsList[currentlyDraggedItemIndex].SetSlotItem(index == 0 ? testItem1 : testItem2, index == 0 ? testAmount1 : testAmount2);
        inventorySlotsList[index].SetSlotItem(currentlyDraggedItemIndex == 0 ? testItem1 : testItem2, currentlyDraggedItemIndex == 0 ? testAmount1 : testAmount2);
        currentlyDraggedItemIndex = -1;
        mouseFollower.Toggle(false);
    }

    private void ResetItemDescription() {
        itemDescriptionImage.gameObject.SetActive(false);
        itemDescriptionName.text = "";
        itemDescription.text = "";
    }

    private void SetItemDescription(Sprite sprite, string itemName, string itemDescription) {
        itemDescriptionImage.sprite = sprite;
        itemDescriptionImage.gameObject.SetActive(true);
        itemDescriptionName.text = itemName;
        this.itemDescription.text = itemDescription;
    }

}
