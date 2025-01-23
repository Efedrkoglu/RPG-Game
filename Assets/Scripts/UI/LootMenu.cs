using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootMenu : MonoBehaviour
{
    private LootBag currentLootBag;
    [SerializeField] private LootMenuRow[] rows;
    [SerializeField] private GameObject lootMenu;

    private Inventory playerInventory;

    public event Action CurrentLootBagLooted;

    private void Start() {
        for(int i = 0; i < rows.Length; i++) {
            rows[i].RowSelected += OnRowSelected;
            rows[i].gameObject.SetActive(false);
        }
        lootMenu.SetActive(false);

        playerInventory = Player.Instance.gameObject.GetComponent<Inventory>();
    }

    private void OnDestroy() {
        for(int i = 0; i < rows.Length; i++) {
            rows[i].RowSelected -= OnRowSelected;
        }
    }

    public void ShowMenu(LootBag lootBag) {
        lootBag.ListenInputs = false;
        currentLootBag = lootBag;
        for(int i = 0; i < currentLootBag.Loots.Count; i++) {
            rows[i].SetRow(currentLootBag.Loots[i].Item, currentLootBag.Loots[i].Amount);
        }

        lootMenu.SetActive(true);

        for(int i = 0; i < currentLootBag.Loots.Count; i++) {
            if (!currentLootBag.Loots[i].IsEmpty()) {
                rows[i].gameObject.SetActive(true);
            }
        }
    }

    public void Grab() {
        for(int i = 0; i < rows.Length; i++) {
            if(rows[i].isSelected) {
                currentLootBag.Loots[i].Amount = playerInventory.AddItem(currentLootBag.Loots[i].Item, currentLootBag.Loots[i].Amount);
                if (!currentLootBag.Loots[i].IsEmpty()) {
                    rows[i].SetRow(currentLootBag.Loots[i].Item, currentLootBag.Loots[i].Amount);
                    rows[i].SelectRow();
                }
                else {
                    if(currentLootBag.IsEmpty()) {
                        CurrentLootBagLooted?.Invoke();
                        CloseLootMenu();
                        break;
                    }
                    else {
                        UpdateMenu();
                    }
                }
            }
        }
    }

    public void GrabAll() {
        foreach(var loot in currentLootBag.Loots) {
            if(!loot.IsEmpty()) {
                loot.Amount = playerInventory.AddItem(loot.Item, loot.Amount);
            }
        }

        if(currentLootBag.IsEmpty()) {
            CurrentLootBagLooted?.Invoke();
            CloseLootMenu();
        }
        else {
            UpdateMenu();
        }
    }
    
    private void UpdateMenu() {
        for(int i = 0; i < rows.Length; i++) {
            rows[i].DeselectRow();
            rows[i].gameObject.SetActive(false);
        }

        for(int i = 0; i < currentLootBag.Loots.Count; i++) {
            rows[i].SetRow(currentLootBag.Loots[i].Item, currentLootBag.Loots[i].Amount);
        }

        for(int i = 0; i < currentLootBag.Loots.Count; i++) {
            if(!currentLootBag.Loots[i].IsEmpty()) {
                rows[i].gameObject.SetActive(true);
            }
        }
    }

    public void CloseLootMenu() {
        if(currentLootBag != null) {
            currentLootBag.ListenInputs = true;
        }
        currentLootBag = null;
        for(int i = 0; i < rows.Length; i++) {
            rows[i].DeselectRow();
            rows[i].gameObject.SetActive(false);
        }
        lootMenu.SetActive(false);
    }

    public void OnRowSelected() {
        for(int i = 0; i < rows.Length; i++) {
            rows[i].DeselectRow();
        }
    }
}
