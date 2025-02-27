using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlacksmithUI : BaseShopUI
{
    [SerializeField] private GameObject cantUpgradeMessage;
    [SerializeField] private UpgradeRow level2UpgradeRow, level3UpgradeRow;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI goldCoinText, silverCoinText, silverIngotText, goldIngotText, leatherText;

    [SerializeField] private List<EquipmentInventorySlotUI> slotsUI;

    [SerializeField] private ItemSO leatherItem, silverIngotItem, goldIngotItem;

    [SerializeField] private GameObject errorTextPrefab;

    private EquipmentInventorySO playerEquipmentInventory;
    private InventorySO playerInventory;

    private int silverIngot, playerSilverIngot, goldIngot, playerGoldIngot, leather, playerLeather;
    private int selectedItemIndex, selectedItemLevel;

    private void Awake() {
        playerEquipmentInventory = Player.Instance.gameObject.GetComponent<Inventory>().getEquipmentInventorySO();
        playerInventory = Player.Instance.gameObject.GetComponent<Inventory>().getInventorySO();

        selectedItemIndex = -1;
        silverIngot = 0;
        goldIngot = 0;
        leather = 0;
        playerSilverIngot = 0;
        playerGoldIngot = 0;
        playerLeather = 0;

        level2UpgradeRow.OnUpgradeRowClicked += UpgradeRowClicked;
        level3UpgradeRow.OnUpgradeRowClicked += UpgradeRowClicked;
        level2UpgradeRow.gameObject.SetActive(false);
        level3UpgradeRow.gameObject.SetActive(false);

        foreach(var item in slotsUI) {
            item.OnEquipmentSlotClicked += EquipmentSlotClicked;
        }
    }

    private void OnDestroy() {
        level2UpgradeRow.OnUpgradeRowClicked -= UpgradeRowClicked;
        level3UpgradeRow.OnUpgradeRowClicked -= UpgradeRowClicked;
        foreach (var item in slotsUI) {
            item.OnEquipmentSlotClicked -= EquipmentSlotClicked;
        }
    }

    private void OnEnable() {
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

                else if(item.Value.Item.itemName == "Gold Ingot")
                    goldIngot += item.Value.Amount;

                else if(item.Value.Item.itemName == "Leather")
                    leather += item.Value.Amount;
            }
        }

        playerLeather = leather;
        playerSilverIngot = silverIngot;
        playerGoldIngot = goldIngot;

        upgradeButton.interactable = false;
        UpdateUI();
    }

    private void OnDisable() {
        selectedItemIndex = -1;
        level2UpgradeRow.Deselect();
        level3UpgradeRow.Deselect();
        level2UpgradeRow.gameObject.SetActive(false);
        level3UpgradeRow.gameObject.SetActive(false);
        cantUpgradeMessage.SetActive(false);

        foreach (var item in slotsUI) {
            item.Deselect();
        }

        if(playerLeather != leather) {
            foreach(var item in playerInventory.GetCurrentInventoryState()) {
                if(!item.Value.IsEmpty && item.Value.Item.itemName == "Leather") {
                    playerInventory.ClearSlot(item.Key);
                }
            }

            if(leather > 0)
                playerInventory.AddItem(leatherItem, leather);
        }

        if(playerSilverIngot != silverIngot) {
            foreach (var item in playerInventory.GetCurrentInventoryState()) {
                if (!item.Value.IsEmpty && item.Value.Item.itemName == "Silver Ingot") {
                    playerInventory.ClearSlot(item.Key);
                }
            }

            if(silverIngot > 0)
                playerInventory.AddItem(silverIngotItem, silverIngot);
        }

        if(playerGoldIngot != goldIngot) {
            foreach (var item in playerInventory.GetCurrentInventoryState()) {
                if (!item.Value.IsEmpty && item.Value.Item.itemName == "Gold Ingot") {
                    playerInventory.ClearSlot(item.Key);
                }
            }

            if(goldIngot > 0)
                playerInventory.AddItem(goldIngotItem, goldIngot);
        }

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

        void setRow(string upgradeKey1, string upgradeKey2) {
            Upgrade upgrade1 = upgrades[upgradeKey1];
            Upgrade upgrade2 = upgrades[upgradeKey2];

            level2UpgradeRow.SetResources(upgrade1.resource, upgrade1.goldAmount, upgrade1.silverAmount, upgrade1.resourceAmount, upgradeKey1);
            level3UpgradeRow.SetResources(upgrade2.resource, upgrade2.goldAmount, upgrade2.silverAmount, upgrade2.resourceAmount, upgradeKey2);
        }

        foreach (var item in slotsUI) {
            item.Deselect();
        }
        equipmentInventorySlot.Select();
        upgradeButton.interactable = false;

        selectedItemIndex = slotsUI.IndexOf(equipmentInventorySlot);
        EquipmentItemSO selectedItem = playerEquipmentInventory.getItemAt(selectedItemIndex);
        selectedItemLevel = selectedItem.level;
        level2UpgradeRow.SetItem(selectedItem.itemImage, selectedItem.itemName);
        level3UpgradeRow.SetItem(selectedItem.itemImage, selectedItem.itemName);

        bool canUpgradable = true;

        if(selectedItem.itemName == "Silver Sword") {
            setRow("Silver Sword 1", "Silver Sword 2");
        }
        else if(selectedItem.itemName == "Golden Sword") {
            setRow("Golden Sword 1", "Golden Sword 2");
        }
        else if(selectedItem.itemName == "Leather Armor") {
            setRow("Leather Armor 1", "Leather Armor 2");
        }
        else if(selectedItem.itemName == "Leather Helmet") {
            setRow("Leather Helmet 1", "Leather Helmet 2");
        }
        else if(selectedItem.itemName == "Leather Boots") {
            setRow("Leather Boots 1", "Leather Boots 2");
        }
        else if(selectedItem.itemName == "Iron Armor") {
            setRow("Iron Armor 1", "Iron Armor 2");
        }
        else if(selectedItem.itemName == "Iron Helmet") {
            setRow("Iron Helmet 1", "Iron Helmet 2");
        }
        else if(selectedItem.itemName == "Iron Boots") {
            setRow("Iron Boots 1", "Iron Boots 2");
        }
        else if(selectedItem.itemName == "Iron Shield") {
            setRow("Iron Shield 1", "Iron Shield 2");
        }
        else {
            canUpgradable = false;
        }

        cantUpgradeMessage.SetActive(!canUpgradable);
        level2UpgradeRow.gameObject.SetActive(canUpgradable);
        level3UpgradeRow.gameObject.SetActive(canUpgradable);
        level2UpgradeRow.Deselect();
        level3UpgradeRow.Deselect();
    }

    public void UpgradeRowClicked(UpgradeRow upgradeRow) {
        level2UpgradeRow.Deselect();
        level3UpgradeRow.Deselect();
        upgradeRow.SelectRow();

        if(level2UpgradeRow.IsSelected) {
            upgradeButton.interactable = selectedItemLevel == 1;
        }
        else if(level3UpgradeRow.IsSelected) {
            upgradeButton.interactable = selectedItemLevel == 2;
        }
    }

    public void CloseButton() {
        base.OpenGameUI();
    }

    public void UpgradeButton() {
        if (selectedItemIndex == -1)
            return;

        Upgrade upgrade = null;
        if(level2UpgradeRow.IsSelected) {
            upgrade = upgrades[level2UpgradeRow.UpgradeKey];
        }
        else if(level3UpgradeRow.IsSelected) {
            upgrade = upgrades[level3UpgradeRow.UpgradeKey];
        }

        if(upgrade != null) {
            if (upgrade.resource == "Leather") {
                if (leather >= upgrade.resourceAmount && Player.Instance.GoldCoin >= upgrade.goldAmount && Player.Instance.SilverCoin >= upgrade.silverAmount) {
                    EquipmentItemSO upgradedEquipment = playerEquipmentInventory.getItemAt(selectedItemIndex).upgradedVersion;
                    playerEquipmentInventory.ClearSlot(selectedItemIndex);
                    playerEquipmentInventory.EquipItem(upgradedEquipment);
                    upgradedEquipment.UseItem();

                    leather -= upgrade.resourceAmount;
                    Player.Instance.GoldCoin -= upgrade.goldAmount;
                    Player.Instance.SilverCoin -= upgrade.silverAmount;

                    selectedItemLevel++;
                    upgradeButton.interactable = false;
                }
                else {
                    GameObject errorText = Instantiate(errorTextPrefab, upgradeButton.gameObject.transform);
                    errorText.GetComponentInChildren<ErrorText>().SetErrorText("Can't afford to upgrade");
                }
            }
            else if (upgrade.resource == "Silver Ingot") {
                if (silverIngot >= upgrade.resourceAmount && Player.Instance.GoldCoin >= upgrade.goldAmount && Player.Instance.SilverCoin >= upgrade.silverAmount) {
                    EquipmentItemSO upgradedEquipment = playerEquipmentInventory.getItemAt(selectedItemIndex).upgradedVersion;
                    playerEquipmentInventory.ClearSlot(selectedItemIndex);
                    playerEquipmentInventory.EquipItem(upgradedEquipment);
                    upgradedEquipment.UseItem();

                    silverIngot -= upgrade.resourceAmount;
                    Player.Instance.GoldCoin -= upgrade.goldAmount;
                    Player.Instance.SilverCoin -= upgrade.silverAmount;

                    selectedItemLevel++;
                    upgradeButton.interactable = false;
                }
                else {
                    GameObject errorText = Instantiate(errorTextPrefab, upgradeButton.gameObject.transform);
                    errorText.GetComponentInChildren<ErrorText>().SetErrorText("Can't afford to upgrade");
                }
            }
            else if (upgrade.resource == "Gold Ingot") {
                if (goldIngot >= upgrade.resourceAmount && Player.Instance.GoldCoin >= upgrade.goldAmount && Player.Instance.SilverCoin >= upgrade.silverAmount) {
                    EquipmentItemSO upgradedEquipment = playerEquipmentInventory.getItemAt(selectedItemIndex).upgradedVersion;
                    playerEquipmentInventory.ClearSlot(selectedItemIndex);
                    playerEquipmentInventory.EquipItem(upgradedEquipment);
                    upgradedEquipment.UseItem();

                    goldIngot -= upgrade.resourceAmount;
                    Player.Instance.GoldCoin -= upgrade.goldAmount;
                    Player.Instance.SilverCoin -= upgrade.silverAmount;

                    selectedItemLevel++;
                    upgradeButton.interactable = false;
                }
                else {
                    GameObject errorText = Instantiate(errorTextPrefab, upgradeButton.gameObject.transform);
                    errorText.GetComponentInChildren<ErrorText>().SetErrorText("Can't afford to upgrade");
                }
            }
        }

        UpdateUI();
    }

    private class Upgrade
    {
        public string resource;
        public int goldAmount, silverAmount, resourceAmount;
    }

    private static Dictionary<string, Upgrade> upgrades = new Dictionary<string, Upgrade> {
        {
            "Silver Sword 1",
            new Upgrade {
                resource = "Silver Ingot",
                goldAmount = 2,
                silverAmount = 50,
                resourceAmount = 1
            }
        },
        {
            "Silver Sword 2",
            new Upgrade {
                resource = "Silver Ingot",
                goldAmount = 3,
                silverAmount = 50,
                resourceAmount = 2
            }
        },
        {
            "Golden Sword 1",
            new Upgrade {
                resource = "Gold Ingot",
                goldAmount = 3,
                silverAmount = 50,
                resourceAmount = 1
            }
        },
        {
            "Golden Sword 2",
            new Upgrade {
                resource = "Gold Ingot",
                goldAmount = 4,
                silverAmount = 50,
                resourceAmount = 2
            }
        },
        {
            "Iron Shield 1",
            new Upgrade {
                resource = "Silver Ingot",
                goldAmount = 2,
                silverAmount = 50,
                resourceAmount = 1
            }
        },
        {
            "Iron Shield 2",
            new Upgrade {
                resource = "Silver Ingot",
                goldAmount = 3,
                silverAmount = 50,
                resourceAmount = 2
            }
        },
        {
            "Leather Armor 1",
            new Upgrade {
                resource = "Leather",
                goldAmount = 1,
                silverAmount = 50,
                resourceAmount = 1
            }
        },
        {
            "Leather Armor 2",
            new Upgrade {
                resource = "Leather",
                goldAmount = 2,
                silverAmount = 50,
                resourceAmount = 2
            }
        },
        {
            "Leather Boots 1",
            new Upgrade {
                resource = "Leather",
                goldAmount = 0,
                silverAmount = 75,
                resourceAmount = 1
            }
        },
        {
            "Leather Boots 2",
            new Upgrade {
                resource = "Leather",
                goldAmount = 1,
                silverAmount = 25,
                resourceAmount = 1
            }
        },
        {
            "Leather Helmet 1",
            new Upgrade {
                resource = "Leather",
                goldAmount = 1,
                silverAmount = 25,
                resourceAmount = 1
            }
        },
        {
            "Leather Helmet 2",
            new Upgrade {
                resource = "Leather",
                goldAmount = 2,
                silverAmount = 0,
                resourceAmount = 2
            }
        },
        {
            "Iron Armor 1",
            new Upgrade {
                resource = "Silver Ingot",
                goldAmount = 2,
                silverAmount = 50,
                resourceAmount = 1
            }
        },
        {
            "Iron Armor 2",
            new Upgrade {
                resource = "Silver Ingot",
                goldAmount = 3,
                silverAmount = 50,
                resourceAmount = 2
            }
        },
        {
            "Iron Boots 1",
            new Upgrade {
                resource = "Silver Ingot",
                goldAmount = 1,
                silverAmount = 0,
                resourceAmount = 1
            }
        },
        {
            "Iron Boots 2",
            new Upgrade {
                resource = "Silver Ingot",
                goldAmount = 1,
                silverAmount = 75,
                resourceAmount = 1
            }
        },
        {
            "Iron Helmet 1",
            new Upgrade {
                resource = "Silver Ingot",
                goldAmount = 1,
                silverAmount = 75,
                resourceAmount = 1
            }
        },
        {
            "Iron Helmet 2",
            new Upgrade {
                resource = "Silver Ingot",
                goldAmount = 2,
                silverAmount = 75,
                resourceAmount = 2
            }
        },
    };
}
