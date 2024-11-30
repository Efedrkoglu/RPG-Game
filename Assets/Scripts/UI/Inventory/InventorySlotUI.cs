using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemAmount;
    [SerializeField] private Sprite slotImage, selectedSlotImage;

    public event Action<InventorySlotUI> OnItemClicked, OnItemDropped, OnItemBeginDrag, OnItemEndDrag;
    
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

    public void SetSlotItem(Sprite sprite, int amount, bool isStackable) {
        if(!itemImage.gameObject.activeInHierarchy)
            itemImage.gameObject.SetActive(true);
        
        if(isStackable) {
            itemAmount.gameObject.SetActive(true);
        }
        else {
            itemAmount.gameObject.SetActive(false);
        }

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

    public void OnPointerClick(PointerEventData eventData)
    {   
        if(eventData.button == PointerEventData.InputButton.Left) {
            OnItemClicked?.Invoke(this);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(empty)
            return;
        
        OnItemBeginDrag?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnItemEndDrag?.Invoke(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnItemDropped?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public bool isEmpty() {
        return empty;
    }
}
