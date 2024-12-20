using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentInventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image emptySlotImage;
    [SerializeField] private Image itemImage;
    
    [SerializeField] private Image slotImage;
    [SerializeField] private Sprite slotSprite, selectedSlotSprite;

    [SerializeField] private EquipmentType equipmentType;

    private bool isEmpty = true;

    public event Action<EquipmentInventorySlotUI> OnEquipmentSlotClicked;

    public void SetSlot(Sprite _itemImage) {
        emptySlotImage.gameObject.SetActive(false);
        itemImage.gameObject.SetActive(true);
        itemImage.sprite = _itemImage;
        isEmpty = false;
    }

    public void ResetSlot() {
        emptySlotImage.gameObject.SetActive(true);
        itemImage.gameObject.SetActive(false);
        isEmpty = true;
    }

    public void Select() {
        slotImage.sprite = selectedSlotSprite;
    }

    public void Deselect() {
        slotImage.sprite = slotSprite;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if(eventData.button == PointerEventData.InputButton.Left) {
            OnEquipmentSlotClicked?.Invoke(this);
        }
    }

    public EquipmentType EquipmentType {
        get { return equipmentType; }
        set { equipmentType = value; }
    }

    public bool IsEmpty() {
        return isEmpty;
    }
}
