using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentInventoryUI : MonoBehaviour
{
    [SerializeField] private List<EquipmentInventorySlotUI> equipmentSlotsList;
    [SerializeField] private Button unequipButton;

    private int selectedSlotIndex;

    public event Action EquipmentInventoryUpdateRequested;
    public event Action<int> UnEquipButtonClicked;

    private void Start() {
        foreach(var slot in equipmentSlotsList) {
            slot.OnEquipmentSlotClicked += OnEquipmentSlotClicked;
        }
        selectedSlotIndex = -1;
    }

    private void OnEnable() {
        EquipmentInventoryUpdateRequested?.Invoke();
        ResetSelection();
    }

    private void OnDestroy() {
        foreach (var slot in equipmentSlotsList) {
            slot.OnEquipmentSlotClicked -= OnEquipmentSlotClicked;
        }
    }

    private void ResetSelection() {
        foreach(var slot in equipmentSlotsList) {
            slot.Deselect();
        }
        unequipButton.interactable = false;
        selectedSlotIndex = -1;
    }

    public void UpdateUI(int slotIndex, InventorySlot slot) {
        if(slotIndex < equipmentSlotsList.Count) {
            if(!slot.IsEmpty)
                equipmentSlotsList[slotIndex].SetSlot(slot.item.itemImage);
            else
                equipmentSlotsList[slotIndex].ResetSlot();
        }
        gameObject.GetComponent<PlayerStatsUI>()?.SetUI(Player.Instance);
    }

    public void OnEquipmentSlotClicked(EquipmentInventorySlotUI equipmentSlotUI) {
        ResetSelection();
        if (equipmentSlotUI.IsEmpty())
            return;
        
        equipmentSlotUI.Select();
        selectedSlotIndex = equipmentSlotsList.IndexOf(equipmentSlotUI);
        unequipButton.interactable = true;
    }

    public void UnequipButton() {
        if (selectedSlotIndex == -1)
            return;

        UnEquipButtonClicked?.Invoke(selectedSlotIndex);
        ResetSelection();
    }
}
