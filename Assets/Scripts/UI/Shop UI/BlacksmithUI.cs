using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlacksmithUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldCoinText, silverCoinText, silverIngotText, goldIngotText, leatherText;

    [SerializeField] private List<EquipmentInventorySlotUI> slotsUI;
    [SerializeField] private EquipmentItemSO upgrades;

    private ToggleBlacksmithUI toggleBlacksmithUI;
    private EquipmentInventorySO playerEquipmentInventory;
    private InventorySO playerInventory;

    private int silverIngot, goldIngot, leather;

    private void Start() {
        silverIngot = 0;
        goldIngot = 0;
        leather = 0;
        foreach(var item in slotsUI) {
            item.OnEquipmentSlotClicked += EquipmentSlotClicked;
        }
    }

    private void OnDestroy() {
        foreach (var item in slotsUI) {
            item.OnEquipmentSlotClicked -= EquipmentSlotClicked;
        }
    }

    private void OnEnable() {
        if(playerEquipmentInventory == null)
            playerEquipmentInventory = Player.Instance.gameObject.GetComponent<Inventory>().getEquipmentInventorySO();

        if (playerInventory == null)
            playerInventory = Player.Instance.gameObject.GetComponent<Inventory>().getInventorySO();

        foreach (var item in playerEquipmentInventory.GetInventoryState()) {
            if(item.Key < 5) {
                if (!item.Value.IsEmpty)
                    slotsUI[item.Key].SetSlot(item.Value.Item.itemImage);
                else
                    slotsUI[item.Key].ResetSlot();
            }
        }

        foreach(var item in playerInventory.GetCurrentInventoryState()) {
            if(!item.Value.IsEmpty) {
                if (item.Value.Item.itemName == "Silver Ingot")
                    silverIngot += item.Value.Amount;

                if (item.Value.Item.itemName == "Gold Ingot")
                    goldIngot += item.Value.Amount;

                if (item.Value.Item.itemName == "Leather")
                    leather += item.Value.Amount;
            }
        }

        UpdateUI();
    }

    private void OnDisable() {
        silverIngot = 0;
        goldIngot = 0;
        leather = 0;
    }

    private void UpdateUI() {
        goldCoinText.text = Player.Instance.GoldCoin.ToString();
        silverCoinText.text = Player.Instance.SilverCoin.ToString();
        goldIngotText.text = goldIngot.ToString();
        silverIngotText.text = silverIngot.ToString();
        leatherText.text = leather.ToString();
    }

    public void EquipmentSlotClicked(EquipmentInventorySlotUI equipmentInventorySlot) {
        int selectedIndex = slotsUI.IndexOf(equipmentInventorySlot);
        if(selectedIndex == 3) {
            playerEquipmentInventory.ClearSlot(selectedIndex);
            playerEquipmentInventory.EquipItem(upgrades);
            upgrades.UseItem();
        }
    }

    public void setToggleBlacksmithUI(ToggleBlacksmithUI toggleBlacksmithUI) {
        this.toggleBlacksmithUI = toggleBlacksmithUI;
    }

    public void CloseButton() {
        toggleBlacksmithUI.OpenGameUI();
    }
}
