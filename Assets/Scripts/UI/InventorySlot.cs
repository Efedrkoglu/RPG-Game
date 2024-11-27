using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemAmount;
    [SerializeField] private Sprite slotImage, selectedSlotImage;

    public event Action<InventorySlot> OnItemClicked, OnItemDropped, OnItemBeginDrag, OnItemEndDrag;
    
    private bool empty;

    void Awake() {
        ResetSlotData();
        Deselect();
    }

    public void ResetSlotData() {
        itemImage.gameObject.SetActive(false);
        itemAmount.gameObject.SetActive(false);
        empty = true;
    }

    public void SetSlotItem(Sprite sprite, int amount) {
        if(!itemImage.gameObject.activeInHierarchy)
            itemImage.gameObject.SetActive(true);
        
        if(!itemAmount.gameObject.activeInHierarchy)
            itemAmount.gameObject.SetActive(true);

        itemImage.sprite = sprite;
        itemAmount.text = amount.ToString();
        empty = false;
    }

    public void Select() {
        gameObject.GetComponent<Image>().sprite = selectedSlotImage;
    }

    public void Deselect() {
        gameObject.GetComponent<Image>().sprite = slotImage;
    }

    public void OnBeginDrag() {
        if(empty)
            return;

        OnItemBeginDrag?.Invoke(this);
    }

    public void OnEndDrag() {
        OnItemEndDrag?.Invoke(this);
    }

    public void OnDrop() {
        OnItemDropped?.Invoke(this);
    }

    public void OnPointerClick(BaseEventData data) {
        if(empty)
            return;

        PointerEventData pointerData = (PointerEventData)data;
        if(pointerData.button == PointerEventData.InputButton.Left) {
            OnItemClicked?.Invoke(this);
        }
    }
}
