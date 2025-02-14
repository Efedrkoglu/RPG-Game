using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentInventoryUI : MonoBehaviour
{
    [SerializeField] private List<EquipmentInventorySlotUI> equipmentSlotsList;
    [SerializeField] private Button unequipButton;
    [SerializeField] private GameObject equipmentInfoPanel;

    private int selectedSlotIndex;
    private EquipmentInfo equipmentInfo;
    private EquipmentInventorySlotUI lastPointerEnteredSlot;

    public event Action EquipmentInventoryUpdateRequested;
    public event Action<int> UnEquipButtonClicked, EquipmentDescriptionRequested;

    private void Start() {
        foreach(var slot in equipmentSlotsList) {
            slot.OnEquipmentSlotClicked += OnEquipmentSlotClicked;
            slot.OnEquipmentSlotPointerEnter += OnEquipmentSlotPointerEnter;
            slot.OnEquipmentSlotPointerExit += OnEquipmentSlotPointerExit;
        }
        selectedSlotIndex = -1;
        equipmentInfo = equipmentInfoPanel.GetComponent<EquipmentInfo>();
    }

    private void OnEnable() {
        EquipmentInventoryUpdateRequested?.Invoke();
        ResetSelection();
    }

    private void OnDestroy() {
        foreach (var slot in equipmentSlotsList) {
            slot.OnEquipmentSlotClicked -= OnEquipmentSlotClicked;
            slot.OnEquipmentSlotPointerEnter -= OnEquipmentSlotPointerEnter;
            slot.OnEquipmentSlotPointerExit -= OnEquipmentSlotPointerExit;
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

    public void OnEquipmentSlotPointerEnter(EquipmentInventorySlotUI equipmentSlotUI) {
        lastPointerEnteredSlot = equipmentSlotUI;
        EquipmentDescriptionRequested?.Invoke(equipmentSlotsList.IndexOf(equipmentSlotUI));
    }

    public void OnEquipmentSlotPointerExit(EquipmentInventorySlotUI equipmentSlotUI) {
        lastPointerEnteredSlot = null;
        HideEquipmentInfoPanel();
    }

    public void ShowEquipmentInfoPanel(EquipmentItemSO equipmentItem) {
        Vector3 position = lastPointerEnteredSlot.transform.position + new Vector3(0, 100, 0);
        equipmentInfo.SetInfo(equipmentItem, position);
        equipmentInfoPanel.SetActive(true);
    }

    public void HideEquipmentInfoPanel() {
        equipmentInfoPanel.SetActive(false);
    }

    public void UnequipButton() {
        if (selectedSlotIndex == -1)
            return;

        UnEquipButtonClicked?.Invoke(selectedSlotIndex);
        ResetSelection();
    }
}
